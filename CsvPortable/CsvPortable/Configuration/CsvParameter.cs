
using CsvPortable.Interfaces;

namespace CsvPortable.Configuration
{
    public class CsvParameter
    {
        public static CsvParameter Default {get => (null, ";");}
        public CsvParameter(CsvConfiguration? configuration)
        {
            Configuration = configuration;
        }

        public CsvParameter(CsvConfiguration? configuration, CsvDelimiter delimiter) : this(configuration)
        {
            Delimiter = delimiter ?? throw new ArgumentNullException(nameof(delimiter));
        }

        public CsvParameter(CsvConfiguration? configuration, CsvDelimiter delimiter, bool closeEnd) : this(configuration, delimiter)
        {
            CloseEnd = closeEnd;
        }

        public CsvParameter(CsvConfiguration? configuration, CsvDelimiter delimiter, bool closeEnd, List<(Type, CsvConfiguration)> specifiedConfigurations) : this(configuration, delimiter, closeEnd)
        {
            SpecifiedConfigurations = specifiedConfigurations ?? throw new ArgumentNullException(nameof(specifiedConfigurations));
        }
        
        public CsvParameter(CsvConfiguration? configuration, CsvDelimiter delimiter, bool closeEnd, params (Type, CsvConfiguration)[] specifiedConfigurations) : this(configuration, delimiter, closeEnd)
        {
            SpecifiedConfigurations = specifiedConfigurations.ToList();
        }
        


        /// <summary>
        /// Gets or Sets Configuration for the Type.
        /// </summary>
        public CsvConfiguration? Configuration { get; set; } = null;

        /// <summary>
        /// Gets or Sets Delimiter --> default ";"
        /// </summary>
        public CsvDelimiter Delimiter { get; set; } = ";";

        /// <summary>
        /// Gets or Sets CloseEnd (The CSVLine gets closed(\r\n)) --> default "true"
        /// </summary>
        public bool CloseEnd { get; set; } = true;


        public CsvParameter? ParameterToUse(Type type, bool closeEnd)
        {
            bool MatchType((Type Type, CsvConfiguration Configuration) t)
            {
                return t.Type == type;
            }
            
            var configToUse = this.SpecifiedConfigurations.Exists(MatchType)
                ? this.SpecifiedConfigurations.FirstOrDefault(MatchType).Configuration
                : this.Configuration;
            
            return new CsvParameter(configToUse, this.Delimiter, closeEnd, this.SpecifiedConfigurations);
        }
        /// <summary>
        /// Gets or Sets Specified Configurations.
        /// For Class X use that Specification.
        /// </summary>
        public List<(Type Type, CsvConfiguration Configuration)> SpecifiedConfigurations { get; set; } = new List<(Type Type, CsvConfiguration Configuration)>();

        public static implicit operator CsvParameter((CsvConfiguration? Configuration, CsvDelimiter Delimiter) tupel)
        {
            return new CsvParameter(tupel.Configuration, tupel.Delimiter);
        }

        public static implicit operator CsvParameter((CsvConfiguration? Configuration, CsvDelimiter Delimiter, bool CloseEnd) tupel)
        {
            return new CsvParameter(tupel.Configuration, tupel.Delimiter, tupel.CloseEnd);
        }

        public static implicit operator CsvParameter((CsvConfiguration Configuration, CsvDelimiter Delimiter, bool CloseEnd, List<(Type, CsvConfiguration)> SpecifiedConfigurations) tupel)
        {
            return new CsvParameter(tupel.Configuration, tupel.Delimiter, tupel.CloseEnd, tupel.SpecifiedConfigurations);
        }



    }
}