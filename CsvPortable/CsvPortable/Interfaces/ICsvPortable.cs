using System.ComponentModel;
using CsvPortable.Attributes;
using CsvPortable.Configuration;
using CsvPortable.Exceptions;
using CsvPortable.Extensions;

namespace CsvPortable.Interfaces
{
   using System.Text;

   public interface ICsvPortable
   {
      public const char ValueEnclosure = '"';
      public const string CsvRowDelimiter = "\r\n";

      public static string ExportDefinition<T>(CsvParameter? parameter = null)
      {
         return ExportDefinition(typeof(T), parameter);
      }

      public static string ExportDefinition(Type t, CsvParameter? parameter = null)
      {
         parameter ??= CsvParameter.Default;
         string export = "";
         foreach (var prop in GetPropertiesToMap(t, parameter))
         {
            TypeConverter converter = TypeDescriptor.GetConverter(prop.PropertyInfo.PropertyType);

            //ComplexObjects need to be instanced
            if (!converter.CanConvertFrom(typeof(string)))
            {
               export += ICsvPortable.ExportDefinition(prop.PropertyInfo.PropertyType, parameter.ParameterToUse(prop.PropertyInfo.PropertyType, false));
            }
            else
            {
               export += $"{prop.Name}{parameter.Delimiter.Value}";
            }
         }

         if (parameter.CloseEnd)
         {
            // remove delimiter and add newline
            return export.Substring(0, export.Length - 1) + CsvRowDelimiter;
         }
         else
         {
            return export;
         }
      }

      public static string ExportToCsvLine<T>(T valObject, CsvParameter? parameter = null)
      {
         string export = "";
         parameter ??= CsvParameter.Default;

         foreach (var prop in GetPropertiesToMap(valObject!.GetType(), parameter))
         {
            var propValue = prop.PropertyInfo.GetValue(valObject);
            TypeConverter converter = TypeDescriptor.GetConverter(prop.PropertyInfo.PropertyType);

            //ComplexObjects need to be instanced
            if (!converter.CanConvertFrom(typeof(string)))
            {
               export += ICsvPortable.ExportToCsvLine(propValue,
                  parameter.ParameterToUse(prop.PropertyInfo.PropertyType, false));
            }
            else
            {
               // Right now everything is enclosed.
               export += $"{ValueEnclosure}{prop.PerformManipulations(propValue?.ToString()?.ToCsvConform() ?? string.Empty)}{ValueEnclosure}";
               export += parameter.Delimiter;
            }
         }

         if (parameter.CloseEnd)
         {
            // remove delimiter and add newline
            return export.Substring(0, export.Length - 1) + CsvRowDelimiter;
         }
         else
         {
            return export;
         }
      }

      public static T? TryFromCsvRow<T>(string row) where T : class, new()
      {
         return TryFromCsvRow<T>(row: row, parameter: CsvParameter.Default);
      }

      public static T FromCsvRow<T>(string row) where T : class, new()
      {
         return FromCsvRow<T>(row: row, parameter: CsvParameter.Default);
      }


      public static T? TryFromCsvRow<T>(string row, CsvParameter parameter) where T : class, new()
      {
         try
         {
            return FromCsvRow<T>(row, parameter);
         }
         catch
         {
            return null;
         }
      }

      public static T FromCsvRow<T>(string row, CsvParameter parameter) where T : class, new()
      {
         if (string.IsNullOrWhiteSpace(row))
         {
            throw DeserializationException(typeof(T), "Row is empty");
         }

         row = row[^(parameter.Delimiter.Value.Length)..] == parameter.Delimiter
            ? row[..^parameter.Delimiter.Value.Length]
            : row;
         var items = SplitCsv(row, parameter.Delimiter);
         try
         {
            return (T)FromCsvRow(typeof(T), items, parameter);
         }
         catch (CsvDeserializationException e)
         {
            e.CsvLine = row;
            throw;
         }
      }

      private static object FromCsvRow(Type type, List<string> items, CsvParameter parameter)
      {
         var itemsSave = items.GetRange(0, items.Count);
         var item = Activator.CreateInstance(type);

         foreach (var prop in GetPropertiesToMap(type, parameter))
         {
            // TODO:  Enable caching here
            TypeConverter converter = TypeDescriptor.GetConverter(prop.PropertyInfo.PropertyType);

            //ComplexObjects need to be instanced
            if (!converter.CanConvertFrom(typeof(string)))
            {
               prop.PropertyInfo.SetValue(item,
                  FromCsvRow(prop.PropertyInfo.PropertyType, items,
                     parameter.ParameterToUse(prop.PropertyInfo.PropertyType, false)));
            }
            else
            {
               try
               {
                  // Item needs to be removed from the items list ( even if its not set) 

                  var itemForSet = items.Cut(0);

                  if (prop.PropertyInfo.GetValue(item) is string)
                  {
                     itemForSet = itemForSet.Trim(ValueEnclosure);
                  }

                  if (prop.PropertyInfo.CanWrite)
                  {
                     // TODO: Enable caching here
                     prop.PropertyInfo.SetValue(item,
                        TypeDescriptor.GetConverter(prop.PropertyInfo.PropertyType)
                           .ConvertFromString(itemForSet));
                  }
               }
               catch (Exception ex)
               {
                  throw DeserializationException(type, $"Error while parsing '{prop.Name}' - '{ex.Message}'");
               }
            }
         }

         return item ?? DeserializationException(type, "Could not create instance of type");
      }

      private static CsvDeserializationException DeserializationException(Type type, string message, string? csvRow = null)
      {
         return new CsvDeserializationException($"Error while creating '{type.Name}' - '{message}'", csvRow);
      }

      private record CacheReflection
      {
         public CacheReflection(Type type, PropertyMode propertyMode, CsvConfiguration? configuration, List<CsvProperty> properties)
         {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.PropertyMode = propertyMode;
            this.Configuration = configuration;
            this.Properties = properties ?? throw new ArgumentNullException(nameof(properties));
         }

         public Type Type { get; set; }
         public PropertyMode PropertyMode { get; set; }
         public CsvConfiguration? Configuration { get; set; }
         public List<CsvProperty> Properties { get; set; }
      }

      private static readonly List<CacheReflection>
         CacheReflections =
            new List<CacheReflection>();


      internal static List<CsvProperty> GetPropertiesToMap(Type T, CsvParameter parameter)
      {
         var cacheReflection = CacheReflections.FirstOrDefault(k => k.Type == T && k.PropertyMode == parameter.PropertyMode && k.Configuration == parameter.Configuration);
         if (cacheReflection is not null)
         {
            return cacheReflection.Properties;
         }

         List<CsvProperty> maps;
         if (parameter.PropertyMode == PropertyMode.Explicit)
         {
            maps = T
               .GetProperties()
               .ToList()
               .Where(k =>
                  k.GetCustomAttributes(false).Any(k => k.GetType() == typeof(CsvPropertyAttribute)))
               .Select(k => new CsvProperty(k)).ToList();
         }
         else
         {
            maps = T
               .GetProperties()
               .ToList()
               .Where(k =>
                  !k.GetCustomAttributes(false).Any(k => k.GetType() == typeof(CsvIgnoreAttribute)))
               .Select(k => new CsvProperty(k)).ToList();
         }


         var t = T.GetProperties();

         maps = maps.OrderBy(k => k.Index).ToList();

         CacheReflections.Add(new CacheReflection(T, parameter.PropertyMode, parameter.Configuration, maps));
         return maps;
      }

      public static List<string> SplitCsv(string csvRow, string delimiter)
      {
         List<string> entries = new List<string>();

         var cur = "";
         bool lastQuoteWasHandled = false;


         void AddEntry(string entry)
         {
            if (entry.Length >= delimiter.Length && entry[^delimiter.Length..] == delimiter)
            {
               entry = entry[..^delimiter.Length];
            }

            if (entry.Any() && entry[0] == 0x22)
            {
               entry = entry[1..];
            }

            if (entry.Any() && entry[^1] == 0x22)
            {
               entry = entry[..^1];
            }

            cur = "";
            lastQuoteWasHandled = false;
            entries.Add(entry);
         }

         foreach (var t in csvRow)
         {
            string compareDelimiter = cur.Length >= delimiter.Length
               ? cur.Substring(cur.Length - delimiter.Length, delimiter.Length)
               : "";

            if (cur.Length >= delimiter.Length && compareDelimiter == delimiter)
            {
               if (cur[0] == 0x22)
               {
                  // Escaped entry - check on if Quote was the starting Quote
                  if (cur.Length - (compareDelimiter.Length + 1) != 0 && cur[^(compareDelimiter.Length + 1)] == 0x22 && !lastQuoteWasHandled)
                  {
                     AddEntry(cur);
                  }
               }
               else
               {
                  AddEntry(cur);
               }
            }

            cur += t;

            /*
             * Quoting Logic:
             * single " (0x22) is the quote character
             * double "" (0x22 0x22) is the escape character
             */


            if (cur[^1] == 0x22)
            {
               // Current Char is Quote 

               if (cur.Length == 1 && cur[0] == 0x22)
               {
                  // Current Char is the first Char and is a Quote -> IsInQuotes
               }
               else if (cur.Length > 2)
               {
                  // First or Second Char is Quote -> continue
                  if (cur[^2] == 0x22 && !lastQuoteWasHandled)
                  {
                     // Double Quote -> Single Quote into the string - need to remove current Quote
                     cur = cur[..^1];
                     lastQuoteWasHandled = true;
                  }
                  else
                  {
                     lastQuoteWasHandled = false;
                  }
               }
            }
         }

         // Trim CR CN of the last row 
         cur = cur.TrimEnd(CsvRowDelimiter.ToCharArray());
         AddEntry(cur);

         if (csvRow.Length >= delimiter.Length && csvRow[^delimiter.Length..] == delimiter)
         {
            entries.Add(string.Empty);
         }

         return entries;
      }

      public static IEnumerable<T> FromStream<T>(Stream stream, Action<CsvDeserializationException>? onErrorRow = null) where T : class, new()
      {
         string currentRow = string.Empty;
         bool isInQuotes = false;

         bool IsStreamEnd() => stream.Position >= stream.Length;
         bool IsRowComplete() => (currentRow.EndsWith(ICsvPortable.CsvRowDelimiter[^1]) && !isInQuotes) || IsStreamEnd();

         bool firstRow = true;

         while (stream.Position <= stream.Length)
         {
            if (IsRowComplete())
            {
               if (firstRow)
               {
                  firstRow = false;
                  currentRow = string.Empty;
                  continue;
               }

               T? item = null;
               try
               {
                  item = ICsvPortable.FromCsvRow<T>(currentRow);
               }
               catch (CsvDeserializationException e)
               {
                  e.CsvLine = currentRow;
                  if (onErrorRow is not null)
                  {
                     onErrorRow(e);
                  }
                  else
                  {
                     throw;
                  }
               }

               if (item is not null)
               {
                  yield return item;
               }

               currentRow = string.Empty;
               if (IsStreamEnd())
               {
                  break;
               }
            }
            else
            {
               currentRow += (char)stream.ReadByte();

               if (currentRow[^1] == 0x22)
               {
                  isInQuotes = !isInQuotes;
               }
            }
         }
      }

      public static async Task ToStream<T>(List<T> entries, Stream outputStream, bool skipDefinition = false)
      {
         if (!skipDefinition)
         {
            await outputStream.WriteAsync(Encoding.UTF8.GetBytes(ICsvPortable.ExportDefinition<T>()));
         }

         foreach (var entry in entries)
         {
            await outputStream.WriteAsync(Encoding.UTF8.GetBytes(ICsvPortable.ExportToCsvLine(entry)));
         }

         await outputStream.FlushAsync();
      }
   }
}