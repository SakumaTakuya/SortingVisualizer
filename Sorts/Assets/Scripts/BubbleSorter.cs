using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSorter : ISorter {

	public string Name
	{
		get { return "Bubble Sort"; }
	}

	public Order Order { get; set; }
	
	public void SortWithEvent(List<IComparable> array, int left, int right, Action<List<IComparable>, int, int> action)
	{
		BubbleSortWirhEvent(array,action);
		action(array, -1, -1);
	}
	
	private void BubbleSortWirhEvent(List<IComparable> array, Action<List<IComparable>, int, int> action)
	{
		for (var i = 0; i < array.Count; i++)
		{
			for (var j = array.Count - 1; j > i; j--)
			{
				switch (Order)
				{
					case Order.Ascending:
						if (array[j].CompareTo(array[j - 1]) >= 0) continue;
						break;
					case Order.Descending:
						if (array[j].CompareTo(array[j - 1]) <= 0) continue;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				action(array, i - 1, i);
				Swap(array, j - 1, j);			
			}
			
		}
	}

	private static void Swap(IList<IComparable> array, int i, int j)
	{
		var t = array[i];
		array[i] = array[j];
		array[j] = t;
	}
}
