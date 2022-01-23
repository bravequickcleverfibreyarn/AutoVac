namespace AutoVac.VacDynamics.Extensions;

internal static class IListExtensions
{
  public static Result[] ToArray<Source, Result>(this IList<Source> collection, Func<Source, Result> selector)
  {
    int count = collection.Count;
    var array = new Result[count];

    for(int i = 0;i < count;++i)
      array[i] = selector(collection[i]);

    return array;
  }
}
