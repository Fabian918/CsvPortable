// <copyright file="StreamTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CsvPortable.Tests.Deserialize;

using System.Text;
using CsvPortable.Interfaces;
using Xunit;
using Xunit.Abstractions;

/// <summary>
/// Defines the <see cref="StreamTest" />.
/// </summary>
public class StreamTest
{
  private const int Count = 20;

  private readonly ITestOutputHelper output;

  /// <summary>
  /// Initializes a new instance of the <see cref="StreamTest"/> class.
  /// </summary>
  /// <param name="output">output.</param>
  public StreamTest(ITestOutputHelper output)
  {
    this.output = output;
  }

  /// <summary>
  /// Test deserialization and serialization of a stream.
  /// </summary>
  /// <returns>async t.</returns>
  [Fact]
  public async Task DeserializeSerializeStream()
  {
    var stream = File.OpenRead("Files/Person/Person1.csv");
    var entries = ICsvPortable.FromStream<Person>(stream: stream).ToList();

    Assert.Equal(Count, entries.Count());

    var outputStream = new MemoryStream();

    await ICsvPortable.ToStream(entries, outputStream);

    outputStream.Position = 0;
    byte[] buffer = new byte[outputStream.Length];
    _ = await outputStream.ReadAsync(buffer);
    var t = Encoding.UTF8.GetString(buffer);
    outputStream.Position = 0;
    var reEntries = ICsvPortable.FromStream<Person>(stream: outputStream);


    Assert.Equal(entries.Count, reEntries.Count());
  }
}