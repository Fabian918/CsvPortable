using CsvPortable.Attributes;
using CsvPortable.Interfaces;

namespace CsvPortable.Tests.TestDto.Attributes;

public class IgnoreAttributeTest
{

   [Theory]
   [InlineData("String1", 99)]
   [InlineData("String191838", 1999813)]
   [InlineData("String19183831", 0183841)]
   [InlineData("String17636163", 492941)]
   [InlineData("String10088138", -131441)]
   public void IgnoreProperty(string str, int number)
   {
      BaseClass baseClass = new BaseClass()
      {
         String = str,
         Int = number
      };
      IgnoreClass1 ignoreClass1 = new IgnoreClass1()
      {
         String = str,
         Int = number
      };
      IgnoreClass2 ignoreClass2 = new IgnoreClass2()
      {
         String = str,
         Int = number
      };

      Assert.Contains(str, ICsvPortable.ExportToCsvLine(baseClass));
      Assert.Contains(number.ToString(), ICsvPortable.ExportToCsvLine(baseClass));
      
      Assert.DoesNotContain(str, ICsvPortable.ExportToCsvLine(ignoreClass1) );
      Assert.Contains(number.ToString(), ICsvPortable.ExportToCsvLine(ignoreClass1));
      
      Assert.Contains(str, ICsvPortable.ExportToCsvLine(ignoreClass2) );
      Assert.DoesNotContain(number.ToString(), ICsvPortable.ExportToCsvLine(ignoreClass2));


   }
   
   
   private class BaseClass
   {
      public virtual string String { get; set; } = String.Empty;
      public virtual int Int { get; set; }
   }

   private class IgnoreClass1 : BaseClass
   {
      [CsvIgnore] public override string String { get; set; } = String.Empty;
   }

   private class IgnoreClass2 : BaseClass
   {
      public override string String { get; set; } = string.Empty;

      [CsvIgnore] public override int Int { get; set; }
   }
}