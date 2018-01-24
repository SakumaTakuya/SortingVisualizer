using System;
using System.Collections.Generic;

public class QuickSorter : ISorter
{
    public string Name
    {
        get { return "Quick Sort"; }
    }

    public Order Order { get; set; }

    public void SortWithEvent(List<IComparable> array, int left, int right, Action<List<IComparable>, int, int> action)
    {
        QuickSortWirhEvent(array,left,right,action);
        action(array, -1, -1);
    }

    private void QuickSortWirhEvent(List<IComparable> array, int left, int right,
        Action<List<IComparable>, int, int> action)
    {
        if (left >= right) return;

        int i = left, j = right;
        var somewhere = (left + right) / 2;
        var pivot = array[somewhere];
        do
        {
            switch (Order)
            {
                case Order.Ascending:
                    while (array[i].CompareTo(pivot) < 0) i++;
                    while (array[j].CompareTo(pivot) > 0) j--;
                    break;
                case Order.Descending:
                    while (array[i].CompareTo(pivot) > 0) i++;
                    while (array[j].CompareTo(pivot) < 0) j--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (i > j) continue;
            action(array, i, j);
            Swap(array, i++, j--);  
        } while (i <= j);
        QuickSortWirhEvent(array, left, j, action);
        QuickSortWirhEvent(array, i, right, action);
        
    }

    private static void Swap(IList<IComparable> array, int i, int j)
    {
        var t = array[i];
        array[i] = array[j];
        array[j] = t;
    }
}
