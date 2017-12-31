using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
	[SerializeField] private Dropdown _sortDropdown;
	[SerializeField] private SortManager _sortManager;
	[SerializeField] private InputField _lengthInput;
	[SerializeField] private Dropdown _arrayDropdown;

	private ISorter[] _sorters;
	private Action[] _alignments;

	// Use this for initialization
	private void Start ()
	{
		_sorters = new ISorter[]
		{
			new QuickSorter(),
			new BubbleSorter()
		};

		_sortDropdown.options.Clear();
		foreach (var s in _sorters)
		{
			_sortDropdown.options.Add(new Dropdown.OptionData(s.Name));
		}
		
		_alignments = new Action[]
		{
			_sortManager.SetAscendingArray,
			_sortManager.SetDescendingArray,
			_sortManager.SetRandomArray,
			
		};
		_arrayDropdown.options = new List<Dropdown.OptionData>
		{
			new Dropdown.OptionData("Ascending"),
			new Dropdown.OptionData("Descending"),
			new Dropdown.OptionData("Random")
		};
		
		
		SetSorter();
		SetLength();
		SetAlignment();
	}
	
	public void SetAlignment()
	{
		var id = _arrayDropdown.value;
		_alignments[id]();
	}

	public void SetSorter()
	{
		var id = _sortDropdown.value;
		_sortManager.Sorter = _sorters[id];
	}

	public void SetLength()
	{
		int num;
		if(!int.TryParse(_lengthInput.text,out num)) return;
		_sortManager.Length = num;
	}
	
	public void ExecuteSort()
	{
		SetSorter();
		SetLength();
		SetAlignment();
		_sortManager.Execute();
	}
}
