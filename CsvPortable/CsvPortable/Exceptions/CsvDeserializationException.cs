namespace CsvPortable.Exceptions;

public class CsvDeserializationException : Exception
{
   public const int DefaultRowIndex = 0;
   public CsvDeserializationException(string message, string? csvLine, int rowIndex = DefaultRowIndex): base(message)
   {
      this.CsvLine = csvLine;
      this.RowIndex = rowIndex;
   }
    
   public string? CsvLine { get; set; }
   
   public int RowIndex { get; set; }
   
}