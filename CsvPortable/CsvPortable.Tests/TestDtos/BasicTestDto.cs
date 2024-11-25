using CsvPortable.Attributes;

namespace CsvPortable.Tests.TestDto;

public class BasicTestDto
{
    [CsvProperty()] public string? String { get; set; }
    [CsvProperty(customTransfomer: (a) => { return "d123";})] public char? Char { get; set; }

    [CsvProperty()] public bool? Bool { get; set; }

    [CsvProperty()] public byte? Byte { get; set; }

    [CsvProperty()] public int? Int { get; set; }

    [CsvProperty()] public double? Double { get; set; }
}