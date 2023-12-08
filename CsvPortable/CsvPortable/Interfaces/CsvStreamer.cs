using CsvPortable.Exceptions;
using Microsoft.Extensions.Logging;

namespace CsvPortable.Interfaces;

using System.Text;

public class CsvStreamer : ICsvStreamer
{
   private ILogger? Logger { get; init; }

   public CsvStreamer(ILogger? logger)
   {
      this.Logger = logger;
   }

   public IEnumerable<T> FromFile<T>(string path, Action<CsvDeserializationException>? onErrorRow) where T : class, new()
   {
      if (!File.Exists(path))
      {
         Logger?.LogError("File '{FilePath}' not found", path);
         throw new FileNotFoundException(nameof(path));
      }

      return FromStream<T>(File.OpenRead(path), onErrorRow);
   }

   public IEnumerable<T> FromStream<T>(Stream stream, Action<CsvDeserializationException>? onErrorRow) where T : class, new()
   {
      string currentRow = string.Empty;
      bool isInQuotes = false;
      
      bool IsStreamEnd() => stream.Position >= stream.Length;
      bool IsRowComplete() => (currentRow.EndsWith(ICsvPortable.CsvRowDelimiter[^1]) && !isInQuotes) || IsStreamEnd();
  
      bool firstRow = true;
      
      while (stream.Position <= stream.Length)
      {
         if (IsRowComplete())
         {
            if (firstRow)
            {
               firstRow = false;
               currentRow = string.Empty;
               continue;
            }

            T? item = null;
            try
            {
               item = ICsvPortable.FromCsvRow<T>(currentRow);
            }
            catch (CsvDeserializationException e)
            {
               e.CsvLine = currentRow;
               if (onErrorRow is not null)
               {
                  onErrorRow(e);
               }
               else
               {
                  throw;
               }
            }

            if (item is not null)
            {
              yield return item;
            }

            currentRow = string.Empty;
            if (IsStreamEnd())
            {
               break;
            }
         }
         else
         {
            currentRow += (char)stream.ReadByte();

            if (currentRow[^1] == 0x22)
            {
               isInQuotes = !isInQuotes;
            }
         }
      }
   }

   public async Task ToStream<T>(List<T> entries, MemoryStream outputStream)
   {
      await outputStream.WriteAsync(Encoding.UTF8.GetBytes(ICsvPortable.ExportDefinition<T>()));
      foreach (var entry in entries)
      {
         await outputStream.WriteAsync(Encoding.UTF8.GetBytes(ICsvPortable.ExportToCsvLine(entry)));
      }
      await outputStream.FlushAsync();
   }
}