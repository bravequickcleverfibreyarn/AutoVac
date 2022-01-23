namespace AutoVac.VacDynamics.Extensions;

internal static class IReadOnlyCollectionExtensions
{
  public static T[] ToOrdererArray<T>(this IReadOnlyCollection<T> collection, IComparer<T> comparer)
  where T : IComparable<T>
  {
    int count = collection.Count;
    var arr = new T[count];
    int index = -1;
    foreach(T item in collection.OrderBy(x => x, comparer))
      arr[++index] = item;

    return arr;
  }
}
