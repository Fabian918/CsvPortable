namespace CsvPortable.Interfaces;

public class CsvDelimiter
{
   public CsvDelimiter(string value)
   {
      if (value is null || value.Length == 0)
      {
         throw new ArgumentException("Delimiter must not be null or empty", nameof(value));
      }

      if (value.Contains("\""))
      {
         throw new ArgumentException("Delimiter must not contain \"", nameof(value));  
      }
         
      Value = value;
   }
   
   public string Value { get; init; }
   
   public static implicit operator string(CsvDelimiter delimiter) => delimiter.Value;
   public static implicit operator CsvDelimiter(string delimiter) => new(delimiter);   
  
}