namespace CsvPortable.Extensions
{
    public static class CsvExtensions
    {
        private static List<(string ValueToReplace, string Replacement)> ReplaceAbles
        {
            get =>
                new List<(string ValueToReplace, string Replacement)>()
                {
                    ("\\", ""),
                    ("\"", "'"),
                    ("\r", ""),
                    ("\n", "<br>"),
                    ("|", "-")
                };
        }

        public static string ToCsvConform(this string val)
        {
            foreach(var item in ReplaceAbles)
            {
                val = val.Replace(item.ValueToReplace, item.Replacement);
            }
            return val;
        }

        public static string FromCsvConform(this string val)
        {
            val = val.Replace("\\", "");
            return val;
        }

     
      

    }
}