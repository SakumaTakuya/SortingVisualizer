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
	private readonly List<SortStep> _steps = new List<SortStep>();

	public ISorter Sorter
	{
		get { return _sorter; }
		set { _sorter = value; }
	}

/*	public List<SortStep> Steps
	{
		get { return _steps; }
	}*/

	private void Start () 
	{			
		_targets = new List<IComparable>();
		for (var i = 0; i < _length; i++)
		{
			_targets.Add(Random.Range(0, _length));
		}
		
		Sorter = new QuickSorter();
		Sorter.SortWithEvent(_targets, 0, _length - 1, (a, l, r) =>
			{
				var list = new List<IComparable>(a);
				_steps.Add(new SortStep(list,l,r));
			}
		);
		
		_visualizer.ShowStep(_steps);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
