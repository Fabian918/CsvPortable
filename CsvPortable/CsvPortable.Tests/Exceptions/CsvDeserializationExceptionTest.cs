using CsvPortable.Exceptions;
using CsvPortable.Interfaces;
using Xunit.Abstractions;

namespace CsvPortable.Tests.Exceptions;

public class CsvDeserializationExceptionTest : TestBase
{
   public CsvDeserializationExceptionTest(ITestOutputHelper output) : base(output)
   {
   }

   [Theory]
   [InlineData("", null, null)]
   [InlineData("This is a error Message", null, null)]
   [InlineData("This is a error Message", "This is a faulty CSV Row", null)]
   [InlineData("This is a error Message", "This is a faulty CSV Row", 1)]
   [InlineData("This is a another error Message", "This is a another faulty CSV Row", 4)]
   public void CsvExceptionTest(string message, string? csvLine, int? rowIndex)
   {
      CsvDeserializationException Instance() => rowIndex is not null ? new CsvDeserializationException(message, csvLine, rowIndex.Value) : new CsvDeserializationException(message, csvLine);

      Assert.Equal(message, Instance().Message);
      Assert.Equal(csvLine, Instance().CsvLine);
      Assert.Equal(rowIndex ?? CsvDeserializationException.DefaultRowIndex, Instance().RowIndex);

      var instance = Instance();
      // Test change of Row
      const string newCsvLine = "New CSV Line";
      instance.CsvLine = newCsvLine;
      Assert.NotEqual(csvLine, instance.CsvLine);
      Assert.Equal(newCsvLine, instance.CsvLine);

      // Test change of RowIndex


      var rowIndexNew = new Random().Next(100, 1000);
      instance.RowIndex = rowIndexNew;
      Assert.NotEqual(rowIndex, instance.RowIndex);
      Assert.Equal(rowIndexNew, instance.RowIndex);
   }

   private class ExceptionTestDto
   {
      public DateTime Time { get; set; }
   }

   public struct ExceptionTestDtoStruct
   {
      public ExceptionTestDtoStruct(string? row, bool faulty)
      {
         Row = row;
         Faulty = faulty;
      }

      public string? Row { get; set; }
      public bool Faulty { get; set; }
   }
   public static IEnumerable<object[]> CsvExceptionTestCases()
   {
      return new List<object[]>
      {
         new object[] { null!, true} ,
         new object[] {"", true},
         new object[] {" ", true},
         new object[]{"--", true},
         new object[]{"199999.19.12", true},
         new object[]{"31.31.2002", true},
         new object[]{"2003.03.12 25.16.12", true},
         new object[]{"2003.04.08 12.05.34", true},
         new object[]{"2003.04.08 12.05.34", true},
      };
   }


   [Theory]
   [MemberData(nameof(CsvExceptionTestCases))]
   public void TestTryFromRow(string row, bool faulty)
   {
      var ob = ICsvPortable.TryFromCsvRow<ExceptionTestDto>(row);
      if (faulty)
      {
         Assert.Null(ob);
      }
      else
      {
         Assert.NotNull(ob);
      }
   }
   [Theory]
   [MemberData(nameof(CsvExceptionTestCases))]
   public void TestFromRow(string row, bool faulty)
   {
      ExceptionTestDto Instance() =>  ICsvPortable.FromCsvRow<ExceptionTestDto>(row);
      if (faulty)
      {
         Assert.Throws<CsvDeserializationException>(Instance);
      }
      else
      {
         Assert.NotNull(Instance());
      }
   }
   
   [Theory]
   [MemberData(nameof(CsvExceptionTestCases))]
   public void TestFromRowErrorAction(string row, bool faulty)
   {
      ExceptionTestDto Instance() =>  ICsvPortable.FromCsvRow<ExceptionTestDto>(row);
      if (faulty)
      {
         Assert.Throws<CsvDeserializationException>(Instance);
      }
      else
      {
         Assert.NotNull(Instance());
      }
   }
}