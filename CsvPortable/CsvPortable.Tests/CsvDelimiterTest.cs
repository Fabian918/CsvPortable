using System.Runtime.InteropServices;
using CsvPortable.Interfaces;

namespace CsvPortable.Tests;

public class CsvDelimiterTest
{

   [Theory]
   [InlineData(";")]
   [InlineData(";;")]
   [InlineData("&")]
   [InlineData("/")]
   [InlineData("//")]
   [InlineData("^")]
   [InlineData("|")]
   [InlineData("||")]
   [InlineData("{")]
   [InlineData("\"", true)]
   [InlineData("\"&", true)]
   [InlineData("\"//", true)]
   [InlineData("{\"", true)]
   [InlineData("\"8173kxk", true)]
   public void CsvDelimiter(string delimiter, bool shouldThrow = false)
   {
      
      CsvDelimiter TestAction() => delimiter;
      if (shouldThrow)
      {
         Assert.Throws<ArgumentException>(TestAction);
      }
      else 
      {
         TestAction();
      }
   }
}