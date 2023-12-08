
namespace CsvPortable.Configuration
{
    public class CsvConfiguration
    {
        public int Type { get; }

        public DateTime Date { get; }

        // ReSharper disable once CollectionNeverUpdated.Global
        public static List<CsvConfiguration> Configurations { get; set; } = new List<CsvConfiguration>();

        public CsvConfiguration(int type, string date)
        {
            Type = type;
            Date = DateTime.Parse(date);
        }

        public static bool operator == (CsvConfiguration? conf1, CsvConfiguration? conf2)
        {
            if(conf1 is null && conf2 is null)
            {
                return true;
            }
            if(conf1 is null || conf2 is null)
            {
                return false;
            }
            return 
                conf1.Date == conf2.Date && 
                conf1.Type == conf2.Type;
        }

        public static bool operator !=(CsvConfiguration conf1, CsvConfiguration conf2)
        {
            return !(conf1 == conf2);   
        }

        public static implicit operator CsvConfiguration (int configurationType)
        {
            return  Configurations.First( k => k.Type == configurationType);  
        }
        
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            throw new Exception();
        }

        public override int GetHashCode()
        {
            return 
                this.Date.GetHashCode() * 
                this.Type.GetHashCode();
        }
    }
}