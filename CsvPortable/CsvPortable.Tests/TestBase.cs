
namespace CsvPortable.Tests;

using Xunit.Abstractions;

/// <summary>
/// Testbase class.
/// </summary>
public class TestBase
{
  /// <summary>
  /// Gets the output.
  /// </summary>
  private readonly ITestOutputHelper output;

  /// <summary>
  /// Initializes a new instance of the <see cref="TestBase"/> class.
  /// </summary>
  /// <param name="output">output.</param>
  public TestBase(ITestOutputHelper output)
  {
    this.output = output;
  }
}