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

	private readonly List<SortElement> _elements = new List<SortElement>();
	private ComputeBuffer _elementBuffer;
	
	private static Vector3 _sumVec = Vector3.zero;
	private bool _doRender;
	private int _renderCount;
	
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
		_instanceMaterial.SetBuffer("_LevelBuffer", _elementBuffer);
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

	public void SetElement(int number, int index, int step)
	{
		var pos = new Vector3(index, number / 2f, step);
		_elements.Add(new SortElement(
			pos, 
			number
		));
		_sumVec += pos;
	}

	public void ShowStep(int length)
	{
		if(_elements.Count == 0) return;
		_renderCount = _elements.Count;
		_doRender = true;
		
		DestroyElementBuffer();
		_elementBuffer = new ComputeBuffer(
			_renderCount, 
			Marshal.SizeOf(typeof(SortElement))
		);
		_elementBuffer.SetData(_elements.ToArray());
		_instanceMaterial.SetFloat("_Length", length);
		
		_cameraRig.transform.position = _sumVec / _renderCount / length * 2;
		_arrow.SetActive(true);
		_arrow.transform.position = new Vector3(2.5f,0,transform.position.z);
		
		_elements.Clear();
		_sumVec = Vector3.zero;
	}
	
	public void ShowStep(int begin, int end, int length)
	{
		if(_elements.Count == 0) return;
		_renderCount = end - begin + 1;
		_doRender = true;
		
		DestroyElementBuffer();
		_elementBuffer = new ComputeBuffer(
			_renderCount, 
			Marshal.SizeOf(typeof(SortElement))
		);
		
		var arr = new SortElement[_renderCount];
		Array.Copy(_elements.ToArray(), begin, arr, 0, _renderCount);
		_elementBuffer.SetData(arr);
		_instanceMaterial.SetFloat("_Length", length);
		
		_cameraRig.transform.position = _sumVec / _renderCount / length * 2;
		_arrow.SetActive(true);
		_arrow.transform.position = new Vector3(2.5f,0,transform.position.z);
		
		_elements.Clear();
		_sumVec = Vector3.zero;
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
