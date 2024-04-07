namespace CsvPortable.Tests;

using System.Globalization;
using Attributes;
using Configuration;
using Interfaces;

public class PropertySelectionTest
{
   [Theory]
   [InlineData(null)]
   [InlineData(PropertyMode.Explicit)]
   [InlineData(PropertyMode.All)]
   public void PropertySelectionModeInParameter(PropertyMode? mode)
   {
      void AssertParameter(CsvParameter parameter, PropertyMode? expectedMode)
      {
         Assert.Equal(expectedMode, parameter.PropertyMode);
      }

      AssertParameter(new CsvParameter(propertyMode: mode), mode ?? CsvParameter.DefaultPropertyMode);
   }


   [Theory]
   [InlineData(PropertyMode.All)]
   [InlineData(PropertyMode.Explicit)]
   public void ImportExportDataPropertyMode(PropertyMode mode)
   {
      var objectToExport = new PropertySelectionSpecifiedTestDto()
      {
         Property1 = 100,
         Property2 = "SomethingElse",
         Property3 = true,
         Property4 = DateTime.Parse("1999.12.12")
      };

      CsvParameter parameter = new CsvParameter(propertyMode: mode);


      void AssertExportedString(string definition, string expectedString, bool shouldBeContained = true)
      {
         if (shouldBeContained)
         {
            Assert.Contains(expectedString, definition);
         }
         else
         {
            Assert.DoesNotContain(expectedString, definition);
         }
      }

      // Assert Definition.
      // Property1 and Property3 should be always in the definition.
      // Property2 and Property4 should be in the definition only if mode is All.

      AssertExportedString(
         ICsvPortable.ExportDefinition<PropertySelectionSpecifiedTestDto>(parameter),
         nameof(PropertySelectionSpecifiedTestDto.Property1));


      AssertExportedString(
         ICsvPortable.ExportDefinition<PropertySelectionSpecifiedTestDto>(parameter),
         nameof(PropertySelectionSpecifiedTestDto.Property2),
         mode == PropertyMode.All);

      AssertExportedString(
         ICsvPortable.ExportDefinition<PropertySelectionSpecifiedTestDto>(parameter),
         nameof(PropertySelectionSpecifiedTestDto.Property3));

      AssertExportedString(
         ICsvPortable.ExportDefinition<PropertySelectionSpecifiedTestDto>(parameter),
         nameof(PropertySelectionSpecifiedTestDto.Property4),
         mode == PropertyMode.All);

      // Assert Exported csv string.
      // Property1 and Property3 values should be always exported.
      // Property2 and Property4 should be exported only if mode is All.

      AssertExportedString(
         ICsvPortable.ExportToCsvLine(objectToExport, parameter),
         objectToExport.Property1.ToString());


      AssertExportedString(
         ICsvPortable.ExportToCsvLine(objectToExport, parameter),
         objectToExport.Property2,
         mode == PropertyMode.All);

      AssertExportedString(
         ICsvPortable.ExportToCsvLine(objectToExport, parameter),
         objectToExport.Property3.ToString());

      AssertExportedString(
         ICsvPortable.ExportToCsvLine(objectToExport, parameter),
         objectToExport.Property4.ToString(CultureInfo.CurrentCulture),
         mode == PropertyMode.All);

      // Assert imported values
      // Property1 and Property3 values should be always be imported again.
      // Property2 and Property4 should be imported only if mode is All, if not the values should be the defaults.
      Assert.Equal(objectToExport.Property1, ICsvPortable.FromCsvRow<PropertySelectionSpecifiedTestDto>(
         ICsvPortable.ExportToCsvLine(objectToExport, parameter), parameter).Property1);

      Assert.Equal(mode == PropertyMode.All ? objectToExport.Property2 : PropertySelectionSpecifiedTestDto.DefaultProperty2, ICsvPortable.FromCsvRow<PropertySelectionSpecifiedTestDto>(
         ICsvPortable.ExportToCsvLine(objectToExport, parameter), parameter).Property2);

      Assert.Equal(objectToExport.Property3, ICsvPortable.FromCsvRow<PropertySelectionSpecifiedTestDto>(
         ICsvPortable.ExportToCsvLine(objectToExport, parameter), parameter).Property3);

      Assert.Equal(mode == PropertyMode.All ? objectToExport.Property4 : PropertySelectionSpecifiedTestDto.DefaultProperty4, ICsvPortable.FromCsvRow<PropertySelectionSpecifiedTestDto>(
         ICsvPortable.ExportToCsvLine(objectToExport, parameter), parameter).Property4);
   }

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
}