namespace CsvPortable.Tests.Deserialize;

public class Person
{
   public int Id { get; set; }

   public string GiveName { get; set; } = string.Empty;

   public string SurName { get; set; } = string.Empty;

   public int Age { get; set; }
   public DateTime Birthday { get; set; }
   
   public Address? LivingAddress { get; set; }
   
   public Address? WorkAddress { get; set; }
}