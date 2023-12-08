// See https://aka.ms/new-console-template for more information

using CsvPortable.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// ------ Preparation - just for the logger, not necessary if no logging is required.
var serviceCollection = new ServiceCollection();
serviceCollection.AddLogging(builder =>
{
   builder.AddConsole();
   builder.SetMinimumLevel(LogLevel.Information);
});

await using var serviceProvider = serviceCollection.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

// ----- Read csv File

// Open File Stream 
await using var fileStream = File.OpenRead("./Addresses.csv");


// Reading items as IEnumerable<T>
int index = 1;
foreach (var address in ICsvPortable.FromStream<Address>(fileStream))
{
   logger.LogInformation(
      "Address '{Index}' - '{Street}, {City}, {Country}, {ZipCode}, {Created}'",
      index,
      address.Street,
      address.City,
      address.Country,
      address.ZipCode,
      address.Created
   );
   index++;
}

// ----- Write csv File

await using var writeStream = File.OpenWrite("./newAddresses.csv"); // Create File


List<Address> newAddresses = new List<Address>()
{
   new Address
   {
      Street = "Street 1",
      City = "City 1",
      Country = "Country 1",
      ZipCode = 12345,
      Created = DateTime.Now
   },
   new Address()
   {
      Street = "Street 2",
      City = "City 2",
      Country = "Country 4",
      ZipCode = 54321,
      Created = new DateTime(2012, 12, 31)
   }
};

// Write items as IEnumerable<T>

await ICsvPortable.ToStream(newAddresses, writeStream);

// Class for the Csv file.
public class Address
{
   public string Street { get; set; } = string.Empty;
   public string City { get; set; } = string.Empty;
   public string Country { get; set; } = string.Empty;
   public int ZipCode { get; set; }
   public DateTime? Created { get; set; }
}