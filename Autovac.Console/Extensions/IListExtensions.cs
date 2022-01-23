namespace Autovac.Console.Extensions;

internal static class IListExtensions
{
  public static Result[] ToArray<Source, Result>(this IList<Source> collection, Func<Source, Result> selector)
  {
    var array = new Result[collection.Count];
    
    int count = collection.Count;
    for(int i = 0;i < count;++i)
      array[i] = selector(collection[i]);

    return array;
  }
}
