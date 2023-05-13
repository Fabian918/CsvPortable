using CsvPortable.Dtos;
using CsvPortable.Interfaces;
using CsvPortable.Tests.TestDto;

namespace CsvPortable.Tests;

public class BasicDeSerializationTests
{
    [Theory]
    [InlineData(null, null, null, null, null, null)]
    [InlineData("testString", null, null, null, null, null)]
    [InlineData("testString123", 's', null, null, null, null)]
    [InlineData("77812", '7', true, null, null, null)]
    [InlineData("", '=', false, (byte)0x56, null, null)]
    [InlineData("9())((()", '/', true, (byte)0x16, -98, null)]
    [InlineData("hallo", 'ß', true, (byte)0x99, 12, null)]
    [InlineData("TestClass 1289", '6', true, (byte)0x5, 12, 98.12)]
    [InlineData(";;;;", ';', true, (byte)0xAC, 18287731, 2990210)]
    [InlineData("", ';', true, (byte)0xB1, 12, -200)]
    
    public void Test1(string? stringS, char? charS, bool? boolS, byte? byteS, int? intS, double? doubleS)
    {
        BasicTestDto basicTestDto = new BasicTestDto()
        {
            String = stringS,
            Char = charS,
            Bool = boolS,
            Byte = byteS,
            Int = intS,
            Double = doubleS
        };

        var testUserAfterCsv =
            ICsvPortable.FromCsvRow<BasicTestDto>(ICsvPortable.ExportToCsvLine(basicTestDto, Dtos.CsvParameter.Default),
                CsvParameter.Default)!;

     
        // We cant differ here, if the string was null or empty, because the csv will be empty in both cases.
        Assert.Equal(stringS ?? string.Empty, testUserAfterCsv.String);
        Assert.Equal(charS, testUserAfterCsv.Char);
        Assert.Equal(boolS, testUserAfterCsv.Bool);
        Assert.Equal(byteS, testUserAfterCsv.Byte);
        Assert.Equal(intS, testUserAfterCsv.Int);
        Assert.Equal(doubleS, testUserAfterCsv.Double);
        
    }
}