namespace CsvPortable.Interfaces;

public interface ICsvStreamer<T>
{
  public Task StreamAsync(IEnumerable<T> data, CancellationToken cancellationToken = default);
}