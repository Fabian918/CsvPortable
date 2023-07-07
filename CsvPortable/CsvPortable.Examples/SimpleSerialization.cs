using CsvPortable.Configuration;
using CsvPortable.Interfaces;

namespace CsvPortable.Examples;

public class SimpleSerialization
{
   private class Employee
   {
      public int Id { get; set; } = 0;
      public string Name { get; set; } = string.Empty;
      public DateTime BirthDate { get; set; } = default;
   }
   
   [Fact]
   public void EmployeeCsv()
   {
      Employee employee = new Employee()
      {
         Id = 1,
         Name = "John Doe",
         BirthDate = DateTime.Parse("01.01.2000")
      };
      
      string csvLine = ICsvPortable.ExportToCsvLine(employee);
      Employee employeeDeserialized = ICsvPortable.FromCsvRow<Employee>(csvLine, CsvParameter.Default)!;
      
      
      Assert.Equal(employee.Id, employeeDeserialized.Id);
      Assert.Equal(employee.Name, employeeDeserialized.Name);
      Assert.Equal(employee.BirthDate, employeeDeserialized.BirthDate);

   }
}