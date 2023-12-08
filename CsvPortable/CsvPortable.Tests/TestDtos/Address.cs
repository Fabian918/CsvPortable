namespace CsvPortable.Tests.Deserialize;

using Interfaces;

public class Address : ICsvPortable
{
   public string Street { get; set; } = string.Empty;
   public string City { get; set; } = string.Empty;
   public int ZipCode { get; set; }
   public string Country { get; set; } = string.Empty;
   public DateTime ActiveAddressSince { get; set; }
}