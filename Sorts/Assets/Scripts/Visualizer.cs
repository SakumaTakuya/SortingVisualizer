using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer : MonoBehaviour
{
	[SerializeField] private Text _showText;
	[SerializeField] private Level _level;
	[SerializeField] private GameObject _levels;

	public void ShowStep(List<SortStep> steps)
	{
		StartCoroutine(Show(steps));
	}

	private IEnumerator Show(IList<SortStep> steps)
	{
		foreach (var step in steps)
		{
			_showText.text += "\nswap(" + step.Left.ToString("00") + "," + step.Right.ToString("00") + ")";
			foreach (var e in step.Process)
			{
				_showText.text += e + ",";
			}
			yield return null;
		}

		for (var i = 0; i < steps.Count; i++)
		{
			var process = steps[i].Process;
			for (var j = 0; j < process.Count; j++)
			{
				Instantiate(_level, _levels.transform).Show((int)process[j],j,i);
				transform.position = Level.Center;
				
			}
			yield return null;
		}
		
	}
}
