// See https://aka.ms/new-console-template for more information


using CsvPortable.Interfaces;

Console.WriteLine("Hello World CsvPortable!");

Car car = new Car()
{
   Id = 1,
   Name = "Hello World CsvPortable!",
   Engine = new Engine()
   {
      PS = 250,
      Type = Engine.EngineType.Hybrid
   }
};

// Creates the header row of the csv.
var csvHeader = ICsvPortable.ExportDefinition<Car>();
Console.WriteLine(csvHeader);
// Console output: Id;Name;PS;Type

// Creates a csv row with the values of the given object.
var csvValues = ICsvPortable.ExportToCsvLine(car);
Console.WriteLine(csvValues);
// Console output: "1";"Hello World CsvPortable!";"250";"Hybrid"


class Car
{
   public int Id { get; set; }
   public string Name { get; set; } = string.Empty;
   
   public Engine? Engine { get; set; }
}

class Engine
{
   public enum EngineType
   {
      Electric,
      Hybrid,
      Gasoline
   }
   public int PS { get; set; }
   public EngineType Type { get; set; }
}

