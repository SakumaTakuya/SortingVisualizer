using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
	[SerializeField] private GameObject _arrow;
	[SerializeField] private CameraController _cameraRig;
	
	//Graphics.DrawMeshInstancedIndirect のための準備
	[SerializeField] private Mesh _instanceMesh;
	[SerializeField] private Material _instanceMaterial;

	private ComputeBuffer _argsBuffer;
	private readonly uint[] _args = {0, 0, 0, 0, 0};

	private readonly List<List<SortElement>> _elements = new List<List<SortElement>>();
	private ComputeBuffer _elementBuffer;

	private bool _doRender;
	private int _renderCount;

	[SerializeField] private int _begin=0;
	[SerializeField]private int _end=1;

	public int Begin //表示する最初のステップ、0以上End以下
	{
		get { return _begin; }
		set { _begin = value < 0 ? 0 : value < _end ? value : _end - 1; }
	} 

	public int End //最後のステップ、要素の配列以上にもならないし、Begin以下にもならない
	{
		get { return _end; }
		set { _end = _elements != null ? value <= _elements.Count ? value > _begin ? value : _begin + 1 : _elements.Count : _end; }
	}

	public int Middle
	{
		get { return (_begin + _end) / 2; }
	}

	private void Start () 
	{
		if(!SystemInfo.supportsInstancing) gameObject.SetActive(false);
		
		_argsBuffer = new ComputeBuffer(
			1, 
			_args.Length * sizeof(uint), 
			ComputeBufferType.IndirectArguments
		);
	}

	private void Update()
	{
		if(!_doRender) return;
		RenderInstancedMesh();
	}
	
	private void RenderInstancedMesh()
	{
		var displayStepNum = End - Begin + 1;

		if (displayStepNum > _elements.Count)
		{
			displayStepNum = _elements.Count;
			Begin = 0;
			End = _elements.Count;
		}
		
		var length = _elements[0].Count;
		_renderCount = displayStepNum * length;
		
		var renderArray = new SortElement[_renderCount];
		for (var i = Begin; i < End; i++)
		{
			for (var j = 0; j < length; j++)
			{
				renderArray[(i - Begin) * length + j] = _elements[i][j];
			}
		}
		
		DestroyElementBuffer();
		_elementBuffer = new ComputeBuffer(
			_renderCount, 
			Marshal.SizeOf(typeof(SortElement))
		);
		_elementBuffer.SetData(renderArray);
		
		_instanceMaterial.SetBuffer("_LevelBuffer", _elementBuffer);
		_instanceMaterial.SetInt("_MidStep", displayStepNum / 2 + Begin);
		
		//ここまでの処理がネックな場合は関数分けして、表示区域が移動した時のみ実行するように仕様を変更する
		
		var numIndices = _instanceMesh ? _instanceMesh.GetIndexCount(0) : 0;
		_args[0] = numIndices;
		_args[1] = (uint) _renderCount;
		_argsBuffer.SetData(_args);
		
		Graphics.DrawMeshInstancedIndirect(
			_instanceMesh,
			0,
			_instanceMaterial,
			new Bounds(Vector3.zero, new Vector3(1000,1000,1000)), 
			_argsBuffer
		);
	}

	public void SetElement(int index, int step, int number)
	{
		if (_elements.Count <= step) _elements.Add(new List<SortElement>());	

		_elements[step].Add(new SortElement(
			index,
			step,
			number
		));
	}

	public void ShowStep(int length)
	{
		if(_elements.Count == 0) return;
		_doRender = true;

		_instanceMaterial.SetFloat("_Length", length);
		
		_arrow.SetActive(true);
		Begin = 0;
		End = Mathf.Min(_elements.Count, 255);
	}

	public void HideStep()
	{
		_doRender = false;
		Begin = 0;
		End = 0;
		_elements.Clear();
		_arrow.SetActive(false);

	}
	
	private void DestroyElementBuffer()
	{
		if (_elementBuffer == null) return;
		_elementBuffer.Release();
		_elementBuffer = null;
	}

	private void DestroyArgsBuffer()
	{
		if (_argsBuffer == null) return;
		_argsBuffer.Release();
		_argsBuffer = null;
	}

	private void OnDestroy()
	{
		DestroyElementBuffer();
		DestroyArgsBuffer();
	}
}
