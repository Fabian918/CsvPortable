namespace CsvPortable.Tests.Deserialize;

using Interfaces;

/// <summary>
/// Test class Address.
/// </summary>
public class Address : ICsvPortable
{
  /// <summary>
  /// Gets or sets the street.
  /// </summary>
  public string Street { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the city.
  /// </summary>
  public string City { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the Zipcode.
  /// </summary>
  public int ZipCode { get; set; }

  /// <summary>
  /// Gets or sets country.
  /// </summary>
  public string Country { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets ActiveAddressSince.
  /// </summary>
  public DateTime ActiveAddressSince { get; set; }
}