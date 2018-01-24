using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{	
	[SerializeField] private float _lookSensitivity = 10;
	[SerializeField] private Vector2 _roteSensitivity = new Vector2(15, 15);

	private Quaternion _offset;
	private float _roteX;
	private float _roteY;
	
	// Use this for initialization
	private void Start () 
	{
		_offset = transform.localRotation;
	}

	// Update is called once per frame
	private void Update ()
	{
		Camera.main.transform.Translate(
			0,
			0,
			Input.GetAxis("Mouse ScrollWheel") * _lookSensitivity * Camera.main.transform.position.magnitude * 0.02f
		);		
		
		if(!Input.GetMouseButton(0)) return;
		_roteX += Input.GetAxis("Mouse X") * _roteSensitivity.x;
		_roteY += Input.GetAxis("Mouse Y") * _roteSensitivity.y;
		transform.rotation = Quaternion.Euler(-_roteY, _roteX, 0) * _offset;
	}
}
