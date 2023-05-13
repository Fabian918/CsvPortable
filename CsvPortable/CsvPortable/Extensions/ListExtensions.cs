namespace CsvPortable.Extensions;

public static class ListExtensions
{
    public static T Cut<T>(this List<T> list, int index)
    {
        var item = list[index];
        list.RemoveAt(index);
        return item;
    }
}