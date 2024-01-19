# CsvPortable
Simple, open &amp; free  Csv mapper libary for C# .NET Core

## Getting Started 

```csharp
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

```
## Tutorials
[Export existing models](./Documentation/CreateReportsFromExistingModels.md)