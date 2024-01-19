# Export Data from existing Models

Lets say your application has multiple Data Models. You want to export certain fields from the data in csv reports.
By default, the mapper takes all public properties of the model. 

```csharp
 public record PropertySelectionSpecifiedTestDto
{
    public const int DefaultProperty1 = -1;
    public const string DefaultProperty2 = "default";
    public const bool DefaultProperty3 = true;
    public static readonly DateTime DefaultProperty4 = DateTime.Parse("2022.01.01");

    public int Property1 { get; set; } = DefaultProperty1;

    public string Property2 { get; set; } = DefaultProperty2;

    public bool Property3 { get; set; } = DefaultProperty3;

    public DateTime Property4 { get; set; } = DefaultProperty4;
}

// Printing the Header Row
Console.WriteLine(ICsvPortable.ExportDefinition<PropertySelectionSpecifiedTestDto>);

// Would print something like:  "Property1";"Property2";"Property3";"Property4"

// Printing the values
Console.WriteLine(ICsvPortable.ExportToCsvLine(dto));

// Would print something like: -1;"default";true;"2022/01/01 00:00:00"
```


## PropertyMode 
The mapper can be configured to only take specified properties via the `PropertyMode enum`: 

```csharp
public record PropertySelectionSpecifiedTestDto
{
    public const int DefaultProperty1 = -1;
    public const string DefaultProperty2 = "default";
    public const bool DefaultProperty3 = true;
    public static readonly DateTime DefaultProperty4 = DateTime.Parse("2022.01.01");

    [CsvProperty] public int Property1 { get; set; } = DefaultProperty1;

    public string Property2 { get; set; } = DefaultProperty2;

    [CsvProperty] public bool Property3 { get; set; } = DefaultProperty3;

    public DateTime Property4 { get; set; } = DefaultProperty4;
}

// Printing the Header Row
Console.WriteLine(ICsvPortable.ExportDefinition<PropertySelectionSpecifiedTestDto>(new CsvParameter(propertyMode: PropertyMode.Explicit)));
// Would print something like:  "Property1";"Property3"

// Printing the values
PropertySelectionSpecifiedTestDto dto = new  PropertySelectionSpecifiedTestDto();
Console.WriteLine(ICsvPortable.ExportToCsvLine(dto), new CsvParameter(propertyMode: PropertyMode.Explicit));

// Would print something like: -1;true
```

## Custom Configuration 

In future development, multiple configurations on each property will be possible. So the caller can specify wich kind of properties should be mapped.