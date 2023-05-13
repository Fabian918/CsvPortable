using System.ComponentModel;
using CsvPortable.Attributes;
using CsvPortable.Dtos;
using CsvPortable.Extensions;

namespace CsvPortable.Interfaces
{
    public interface ICsvPortable
    {
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
                    export +=
                        $"{prop.Enclosure}{prop.PerformManipulations(propValue?.ToString().ToCsvConform())}{prop.Enclosure}";
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

            row = row[^(parameter.Delimiter.Length)..] == parameter.Delimiter
                ? row[..^parameter.Delimiter.Length]
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

        private static List<(Type Type, CsvConfiguration? Configuration, List<CsvProperty> Properties)>
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
                    k.GetCustomAttributes(false).FirstOrDefault(k => k.GetType() == typeof(CsvPropertyAttribute)) != null)
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
            bool isInQuotes = false;

            void AddEntry(string entry)
            {
                if (entry.Length >= delimiter.Length && entry[^delimiter.Length..] == delimiter)
                {
                    entry = entry.Remove((entry.Length - 1) - (delimiter.Length - 1), delimiter.Length);
                }

                entry = entry.Trim('\"');

                cur = "";
                entries.Add(entry);
            }

            for (int i = 0; i < csvRow.Length; i++)
            {
                string compareDelimiter = cur.Length >= delimiter.Length
                    ? cur.Substring(cur.Length - delimiter.Length, delimiter.Length)
                    : "";
                bool IsEnd() => i == csvRow.Length - 1;

                if (cur.Length >= delimiter.Length && compareDelimiter == delimiter & !isInQuotes)
                {
                    AddEntry(cur);
                }

                cur += csvRow[i];
                if (cur[^1] == '\"' && cur[^1] != '\\')
                {
                    isInQuotes = !isInQuotes;
                    continue;
                }
            }

            // Trim CR CN of the last row 
            cur = cur.TrimEnd('\n', '\r');
            AddEntry(cur);

            // Adding two empty values, if the last value was empty and/or delimiter
            entries.AddRange(new List<string>() { "", "" });
            return entries;
        }
    }
}