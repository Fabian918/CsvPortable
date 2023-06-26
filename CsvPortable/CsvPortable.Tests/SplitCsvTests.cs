using CsvPortable.Interfaces;
using CsvPortable.Tests.TestDto.SplitRow;
using Newtonsoft.Json;

namespace CsvPortable.Tests.TestDto;

public class SplitCsvTests
{
   const string TestDataPrefix = "./Files/SplitCsvRow";

   private class BasicTestData : TheoryData<SplitRowTestDto>
   {
      public BasicTestData()
      {
         foreach (var splitRowTestDto in JsonConvert.DeserializeObject<SplitRowTestDto[]>(
                     File.ReadAllText(Path.Combine(TestDataPrefix, "SplitCsvRowBasic.json"))))
         {
            Add(splitRowTestDto);
         }
      }
   }

   private class EncapsulationTestData : TheoryData<SplitRowTestDto>
   {
      public EncapsulationTestData()
      {
         foreach (var splitRowTestDto in JsonConvert.DeserializeObject<SplitRowTestDto[]>(
                     File.ReadAllText(Path.Combine(TestDataPrefix, "SplitCsvRowEncapsulation.json"))))
         {
            Add(splitRowTestDto);
         }
      }
   }


   // Basic testing
   [Theory]
   [ClassData(typeof(BasicTestData))]
   [ClassData(typeof(EncapsulationTestData))]
   public void TestSplitCsvRow(SplitRowTestDto testData)
   {
      var splitItems = ICsvPortable.SplitCsv(testData.Row, testData.Delimiter);
      Assert.Equal(testData.ExpectedItems.Length, splitItems.Count);
      for (int i = 0; i < testData.ExpectedItems.Length; i++)
      {
         Assert.Equal(testData.ExpectedItems[i], splitItems[i]);
      }
   }
}