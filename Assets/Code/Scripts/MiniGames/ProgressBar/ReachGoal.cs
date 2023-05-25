using Project.UI.ProgressBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReachGoal : MonoBehaviour
{
	public Slider slider;
	public GameObject objectToActivate;
	public GameObject objectToDeactivate;

	void Update()
	{
		objectToActivate.SetActive(false);
		objectToDeactivate.SetActive(true);

		if (slider.value == slider.maxValue)
		{
			objectToActivate.SetActive(true);
			objectToDeactivate.SetActive(false);
		}
	}
}
