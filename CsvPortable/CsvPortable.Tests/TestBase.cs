using Xunit.Abstractions;

namespace CsvPortable.Tests;

public class TestBase
{
   protected readonly ITestOutputHelper output;

   public TestBase(ITestOutputHelper output)
   {
      this.output = output;
   }
}