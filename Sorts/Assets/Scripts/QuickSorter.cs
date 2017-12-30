using System;
using System.Collections.Generic;

public class QuickSorter : ISorter
{
    public void SortWithEvent(List<IComparable> array, int left, int right, Action<List<IComparable>, int, int> action)
    {
        QuickSortWirhEvent(array,left,right,action);
    }
    
    private static void QuickSortWirhEvent(List<IComparable> array, int left, int right, Action<List<IComparable>, int, int> action)
    {
        while (true)
        {
            if (left >= right) return;

            int i = left, j = right;
            var somewhere = (left + right) / 2;
            var pivot = array[somewhere];
            do
            {
                while (array[i].CompareTo(pivot) > 0) i++;
                while (array[j].CompareTo(pivot) < 0) j--;
                
                if (i > j) continue;
                action(array, i, j);
                Swap(array, i++, j--);
            } while (i <= j);
            QuickSortWirhEvent(array, left, j, action);
            left = i;
        }
    }

    private static void Swap(IList<IComparable> array, int i, int j)
    {
        var t = array[i];
        array[i] = array[j];
        array[j] = t;
    }
}
