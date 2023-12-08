namespace CsvPortable.Tests;

using Deserialize;
using Interfaces;
using Xunit.Abstractions;

public class StreamTests
{
   private readonly ITestOutputHelper output;

   public StreamTests(ITestOutputHelper output)
   {
      this.output = output;
   }

   [Fact]
   public void TestStreamInputOutput()
   {
      var loader = ICsvPortable.CreateStreamer(output.BuildLogger());

      var stream = File.OpenRead("Files/Person/Person1.csv");
      var persons = loader.FromStream<Person>(stream);
      
      
   }
}