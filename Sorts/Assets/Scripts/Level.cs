using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
	private static Vector3 _sumVec = Vector3.zero;
	private static int _count;

	public static Vector3 Center
	{
		get { return _sumVec / _count; }
	}
	
	[SerializeField] private Text _showText;
	[SerializeField] private Image _showImage;

	public void Show(int number, int index, int step)
	{
		_showText.text = number.ToString();
		_showImage.color = new Color(1, number/ 16f,number/ 8f,1);
		transform.position =  new Vector3(index, number/ 2f, step);
		transform.localScale = new Vector3(1, number + 0.1f, 1);
		
		_count++;
		_sumVec += transform.position;
	}
}
