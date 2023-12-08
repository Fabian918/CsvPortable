namespace CsvPortable.Tests.TestDto.SplitRow;

public class SplitRowTestDto
{
   public SplitRowTestDto(string row, string delimiter, params string[] expectedItems )
   {
      Row = row ?? throw new ArgumentNullException(nameof(row));
      Delimiter = delimiter ?? throw new ArgumentNullException(nameof(delimiter));
      ExpectedItems = expectedItems ?? throw new ArgumentNullException(nameof(expectedItems));
   }
   public string Row { get; set; }
   public string Delimiter { get; set; }
   public string[] ExpectedItems { get; set; }
}