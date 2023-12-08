using CsvPortable.Exceptions;

namespace CsvPortable.Interfaces;

public interface ICsvStreamer
{
   IEnumerable<T> FromFile<T>(string path) where T : class, new()
   {
      return FromFile<T>(path, null);
   }

   IEnumerable<T> FromFile<T>(string path, Action<CsvDeserializationException>? onErrorRow) where T : class, new();

   /// <summary>
   /// Deserializes csv items of type(T) from stream.
   /// </summary>
   /// <param name="stream">stream for deserialization</param>
   /// <typeparam name="T">type(T) to deserialize</typeparam>
   /// <returns>T or throws <see cref="CsvDeserializationException"/> on deserialization error.</returns>
    IEnumerable<T> FromStream<T>(Stream stream) where T : class, new()
   {
      return FromStream<T>(stream, null).ToList();
   }

   IEnumerable<T> FromStream<T>(Stream stream, Action<CsvDeserializationException>? onErrorRow) where T : class, new();

   Task ToStream<T>(List<T> entries, MemoryStream outputStream);
}