using CsvPortable.Interfaces;

namespace CsvPortable.Configuration
{
   public class CsvParameter
   {
      public static readonly CsvConfiguration? DefaultConfiguration = null;
      public static readonly CsvDelimiter DefaultDelimiter = ";";
      public static readonly bool DefaultCloseEnd = true;
      public static readonly PropertyMode DefaultPropertyMode = PropertyMode.All;
      public static readonly List<(Type, CsvConfiguration)> DefaultSpecifiedConfigurations = new List<(Type, CsvConfiguration)>();

      public static CsvParameter Default
      {
         get => (DefaultConfiguration, DefaultDelimiter);
      }

      public CsvParameter(CsvConfiguration? configuration = null, CsvDelimiter? delimiter = null, bool? closeEnd = null, PropertyMode? propertyMode = null, List<(Type Type, CsvConfiguration Configuration)>? specifiedConfigurations = null)
      {
         this.Configuration = configuration ?? DefaultConfiguration;
         this.Delimiter = delimiter ?? DefaultDelimiter;
         this.CloseEnd = closeEnd ?? DefaultCloseEnd;
         this.PropertyMode = propertyMode ?? DefaultPropertyMode;
         this.SpecifiedConfigurations = specifiedConfigurations ?? DefaultSpecifiedConfigurations;
      }


      /// <summary>
      /// Gets or Sets Configuration for the Type.
      /// </summary>
      public CsvConfiguration? Configuration { get; set; }

      /// <summary>
      /// Gets or Sets Delimiter --> default ";"
      /// </summary>
      public CsvDelimiter Delimiter { get; set; }

      /// <summary>
      /// Gets or Sets CloseEnd (The CSVLine gets closed(\r\n)) --> default "true"
      /// </summary>
      public bool CloseEnd { get; set; }

      public PropertyMode PropertyMode { get; set; }


      public CsvParameter ParameterToUse(Type type, bool closeEnd)
      {
         bool MatchType((Type Type, CsvConfiguration Configuration) t)
         {
            return t.Type == type;
         }

         var configToUse = this.SpecifiedConfigurations.Exists(MatchType)
            ? this.SpecifiedConfigurations.FirstOrDefault(MatchType).Configuration
            : this.Configuration;

         return new CsvParameter(configToUse, this.Delimiter, closeEnd, this.PropertyMode, this.SpecifiedConfigurations);
      }

      /// <summary>
      /// Gets or Sets Specified Configurations.
      /// For Class X use that Specification.
      /// </summary>
      public List<(Type Type, CsvConfiguration Configuration)> SpecifiedConfigurations { get; set; }

      public static implicit operator CsvParameter((CsvConfiguration? Configuration, CsvDelimiter Delimiter) tupel)
      {
         return new CsvParameter(tupel.Configuration, tupel.Delimiter);
      }

      public static implicit operator CsvParameter((CsvConfiguration? Configuration, CsvDelimiter Delimiter, bool CloseEnd) tupel)
      {
         return new CsvParameter(tupel.Configuration, tupel.Delimiter, tupel.CloseEnd);
      }
   }
}