using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SortManager : MonoBehaviour 
{
	[SerializeField] private Visualizer _visualizer;
	[SerializeField] private int _length = 256;

	private List<IComparable> _targets;
	private ISorter _sorter = new QuickSorter();
	
	public ISorter Sorter
	{
		get { return _sorter; }
		set { _sorter = value; }
	}

	public int Length
	{
		get { return _length; }
		set{ _length = value; }
	}
	
	public void SetAscendingArray()
	{
		_targets = new List<IComparable>();
		for (var i = 0; i < _length; i++)
		{
			_targets.Add(i);
		}
		_sorter.Order = Order.Descending;
	}
	
	public void SetDescendingArray()
	{
		_targets = new List<IComparable>();
		for (var i = _length; i > 0; i--)
		{
			_targets.Add(i);
		}
		_sorter.Order = Order.Ascending;
	}
	
	public void SetRandomArray()
	{
		_targets = new List<IComparable>();
		for (var i = 0; i < _length; i++)
		{
			_targets.Add(Random.Range(0, _length));
		}
		_sorter.Order = Order.Descending;
	}

	public void Execute()
	{		
		var step = 0;
		Sorter.SortWithEvent(_targets, 0, _length - 1, (arr, l, r) =>
			{	
				var index = 0;
				foreach (var c in arr)
				{
					_visualizer.SetElement(index++,step,(int)c);
				}
				step++;
			}
		);
		
		_visualizer.ShowStep(_length);
	}
}
