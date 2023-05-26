using Project.UI.ProgressBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReachGoal : MonoBehaviour
{
	public Slider slider;
	public List<GameObject> objectToActivate;
	public List<GameObject> objectToDeactivate;

	void Update()
	{
		foreach (GameObject obj in objectToActivate)
		{
			obj.SetActive(false);
		}
		foreach (GameObject obj in objectToDeactivate)
		{
			obj.SetActive(true);
		}

		if (slider.value == slider.maxValue)
		{
			foreach (GameObject obj in objectToActivate)
			{
				obj.SetActive(true);
			}
			foreach (GameObject obj in objectToDeactivate)
			{
				obj.SetActive(false);
			}
		}
	}
}
