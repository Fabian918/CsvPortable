using System.ComponentModel;
using CsvPortable.Attributes;
using CsvPortable.Configuration;
using CsvPortable.Extensions;

namespace CsvPortable.Interfaces
{
   public interface ICsvPortable
   {
      public const char StringEnclosure = '"';

      private static readonly Type[] TypesForEnclosure = new[] { typeof(string), typeof(char) };

      public static string ExportDefinition<T>(T value, CsvParameter? parameter = null)
      {
         parameter ??= CsvParameter.Default;
         string export = "";
         foreach (var pro in GetPropertiesToMap(typeof(T), parameter.Configuration))
         {
            var propValue = pro.PropertyInfo.GetValue(value);
            if (propValue is ICsvPortable exportable)
            {
               export += ICsvPortable.ExportDefinition(exportable,
                  parameter.ParameterToUse(exportable.GetType(), false));
            }
            else
            {
               export += $"{pro.Name}{parameter.Delimiter}";
            }
         }

         if (parameter.CloseEnd)
         {
            // remove delimiter and add newline
            return export.Substring(0, export.Length - 1) + "\r\n";
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

         foreach (var prop in GetPropertiesToMap(typeof(T), parameter.Configuration))
         {
            var propValue = prop.PropertyInfo.GetValue(valObject);
            if (propValue is ICsvPortable exportable)
            {
               export += ICsvPortable.ExportToCsvLine(exportable,
                  parameter.ParameterToUse(exportable.GetType(), false));
            }
            else
            {
               string enclosure = TypesForEnclosure.Contains( 
                 Nullable.GetUnderlyingType(prop.PropertyInfo.PropertyType) ?? prop.PropertyInfo.PropertyType)
                  ? StringEnclosure.ToString()
                  : string.Empty;
               export +=
                  $"{enclosure}{prop.PerformManipulations(propValue?.ToString().ToCsvConform())}{enclosure}";
               export += parameter.Delimiter;
            }
         }

         if (parameter.CloseEnd)
         {
            // remove delimiter and add newline
            return export.Substring(0, export.Length - 1) + "\r\n";
         }
         else
         {
            return export;
         }
      }


      public static T? FromCsvRow<T>(string row, CsvParameter parameter) where T : class, new()
      {
         if (string.IsNullOrWhiteSpace(row))
         {
            return null;
         }

         row = row[^(parameter.Delimiter.Value.Length)..] == parameter.Delimiter
            ? row[..^parameter.Delimiter.Value.Length]
            : row;
         var items = SplitCsv(row, parameter.Delimiter);
         return FromCsvRow(typeof(T), items, parameter) as T;
      }


      private static object? FromCsvRow(Type type, List<string> items, CsvParameter parameter)
      {
         var itemsSave = items.GetRange(0, items.Count);
         var item = Activator.CreateInstance(type);

         foreach (var prop in GetPropertiesToMap(type, parameter.Configuration))
         {
            //ComplexObjects need to be instanced
            if (prop.PropertyInfo.GetValue(item) is ICsvPortable)
            {
               prop.PropertyInfo.SetValue(item,
                  FromCsvRow(prop.PropertyInfo.PropertyType, items,
                     parameter.ParameterToUse(prop.PropertyInfo.GetValue(item).GetType(), false)));
            }
            else
            {
               try
               {
                  // Item needs to be removed from the items list ( even if its not set) 

                  var itemForSet = items.Cut(0);

                  if (prop.PropertyInfo.GetValue(item) is string)
                  {
                     itemForSet = itemForSet.Trim(StringEnclosure);
                  }

                  if (prop.PropertyInfo.CanWrite)
                  {
                     prop.PropertyInfo.SetValue(item,
                        TypeDescriptor.GetConverter(prop.PropertyInfo.PropertyType)
                           .ConvertFromString(itemForSet));
                  }
               }
               catch (Exception ex)
               {
                  var exp = new Exception(ex.Message);
                  throw exp;
               }
            }
         }

         return item;
      }


      private static readonly List<(Type Type, CsvConfiguration? Configuration, List<CsvProperty> Properties)>
         CacheReflections =
            new List<(Type Type, CsvConfiguration? Configuration, List<CsvProperty> Properties)>();


      internal static List<CsvProperty> GetPropertiesToMap(Type T, CsvConfiguration? configuration)
      {
         if (CacheReflections.Exists(k => k.Type == T && k.Configuration == configuration))
         {
            return CacheReflections.First(k => k.Type == T && k.Configuration == configuration).Properties;
         }

         var maps = T
            .GetProperties()
            .ToList()
            .Where(k =>
               !k.GetCustomAttributes(false).Any(k => k.GetType() == typeof(CsvIgnoreAttribute)))
            .Select(k => new CsvProperty(k)).ToList();

         var t = T.GetProperties();

         maps = maps.OrderBy(k => k.Index).ToList();

         CacheReflections.Add((T, configuration, maps));
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

            if (cur.Length >= delimiter.Length && compareDelimiter == delimiter )
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
         cur = cur.TrimEnd('\n', '\r');
         AddEntry(cur);

         if (csvRow.Length >= delimiter.Length && csvRow[^delimiter.Length..] == delimiter)
         {
            entries.Add(string.Empty);
         }

         return entries;
      }
   }
}