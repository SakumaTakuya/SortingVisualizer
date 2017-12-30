using System;
using System.Collections.Generic;

public interface ISorter
{
    //ステップごとの様子をすべて返す
    void SortWithEvent(List<IComparable> array, int left, int right, Action<List<IComparable>, int, int> action);
}
