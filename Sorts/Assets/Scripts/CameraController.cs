using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
	[SerializeField] private float _lookSensitivity = 10;
	[SerializeField] private Vector2 _roteSensitivity = new Vector2(15, 15);
	[SerializeField] private Vector2 _min = new Vector2(-360, -60);
	[SerializeField] private Vector2 _max = new Vector2(360, 60);
	//[SerializeField] private OVRCameraRig _camerarig;

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
		Camera.main.transform.Translate(0,0,Input.GetAxis("Mouse ScrollWheel") * _lookSensitivity);
		if(!Input.GetMouseButton(0)) return;
		
		_roteX += Input.GetAxis("Mouse X") * _roteSensitivity.x;
		_roteY += Input.GetAxis("Mouse Y") * _roteSensitivity.y;
		//_roteX = ClampAngle(_roteX, _min.x, _max.x);
		//_roteY = ClampAngle(_roteY, _min.y, _max.y);
		transform.rotation = Quaternion.Euler(-_roteY, _roteX, 0) * _offset;
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360) angle += 360;
		if (angle > 360) angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}

}
