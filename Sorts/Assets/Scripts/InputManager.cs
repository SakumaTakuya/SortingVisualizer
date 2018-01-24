using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
	[SerializeField] private GameObject _executionUI;
	[SerializeField] private GameObject _hidingUI;

	[SerializeField] private SortManager _sortManager;
	[SerializeField] private Visualizer _visualizer;

	[SerializeField] private Dropdown _sortDropdown;
	[SerializeField] private InputField _lengthInput;
	[SerializeField] private Dropdown _arrayDropdown;
	[SerializeField] private InputField _sizeInput;

	[SerializeField] private int _fastForward = 128;

	private ISorter[] _sorters;
	private Action[] _alignments;
	private int _size;

	// Use this for initialization
	private void Start()
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

	private void Update()
	{
		if (Input.GetKey(KeyCode.A))
		{
			var end = _visualizer.End;
			_visualizer.End++;
			_visualizer.Begin = _visualizer.End > end ? _visualizer.Begin + 1 : _visualizer.Begin;
		}
		if (Input.GetKey(KeyCode.S))
		{
			var begin = _visualizer.Begin;
			_visualizer.Begin--;
			_visualizer.End = _visualizer.Begin < begin ? _visualizer.End - 1 : _visualizer.End;
		}
		
		if (Input.GetKey(KeyCode.Z))
		{
			_visualizer.End += _fastForward;
			_visualizer.Begin = _visualizer.End - _size;
		}
		if (Input.GetKey(KeyCode.X))
		{
			_visualizer.Begin -= _fastForward;
			_visualizer.End = _visualizer.Begin + _size;
		}

		if (Input.GetKey(KeyCode.Q))
		{
			ChangeSize(_size + 1);
		}
		if (Input.GetKey(KeyCode.W))
		{
			ChangeSize(_size - 1);
		}
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
		if (!int.TryParse(_lengthInput.text, out num)) return;
		_sortManager.Length = num;
	}

	public void ExecuteSort()
	{
		SetSorter();
		SetLength();
		SetAlignment();
		_sortManager.Execute();
		_executionUI.SetActive(false);
		_hidingUI.SetActive(true);
		SetSize();
	}

	public void HideResult()
	{
		_visualizer.HideStep();
		_executionUI.SetActive(true);
		_hidingUI.SetActive(false);
	}

	public void SetSize()
	{
		if (!int.TryParse(_sizeInput.text, out _size)) return;
		ChangeSize(_size);
	}

	public void ChangeSize(int size)
	{
		var mid = _visualizer.Middle;
		_visualizer.Begin = Mathf.CeilToInt(mid - size / 2f);
		_visualizer.End = Mathf.CeilToInt(mid + size / 2f);

		_size = _visualizer.End - _visualizer.Begin;
		_sizeInput.text = _size.ToString();
		
	}
}
