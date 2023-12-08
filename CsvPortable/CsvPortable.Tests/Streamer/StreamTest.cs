using CsvPortable.Interfaces;
using Xunit.Abstractions;

namespace CsvPortable.Tests.Deserialize;

using System.Text;

public class StreamTest
{
   private readonly ITestOutputHelper output;

   public StreamTest(ITestOutputHelper output)
   {
      this.output = output;
   }

   private const int Count = 20;

   [Theory]
   [InlineData(true)]
   [InlineData(false)]
   public async Task DeserializeSerializeStream(bool loggerNull)
   {
      var streamer = ICsvPortable.CreateStreamer(loggerNull ? null : output.BuildLogger());

      var stream = File.OpenRead("Files/Person/Person1.csv");
      var entries = streamer.FromStream<Person>(stream: stream).ToList();

      Assert.Equal(Count, entries.Count());

      var outputStream = new MemoryStream();

      await streamer.ToStream(entries, outputStream);

      outputStream.Position = 0;
      byte[] buffer = new byte[outputStream.Length];
      await outputStream.ReadAsync(buffer);
      var t = Encoding.UTF8.GetString(buffer);
      outputStream.Position = 0;
      var reEntries = streamer.FromStream<Person>(stream: outputStream);
      

      Assert.Equal(entries.Count, reEntries.Count());
   }
}