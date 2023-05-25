using Project.UI.ProgressBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Project.UI;

public class ReachGoal : MonoBehaviour
{
	public Slider slider;
	//ISwapableLogic<GameObject> m_controller;
	public GameObject objectToActivate;
	public GameObject objectToDeactivate;

	private bool IsSliderReachedMax(){
		return slider.value == slider.maxValue;
	}

	// public void Awake(){
	// 	m_controller = new SwapableLogicDefault<GameObject>(objectToDeactivate, new SwapableLogic<GameObject>(objectToActivate), IsSliderReachedMax);
	// }
	public void Start(){
		slider.onValueChanged.AddListener(OnValueChanged);
		objectToActivate.SetActive(false);
		objectToDeactivate.SetActive(true);
	}
	void OnValueChanged(float value){
		bool result = IsSliderReachedMax();
		objectToActivate.SetActive(result);
		objectToDeactivate.SetActive(!result);
	}
	// void Update()
	// {
	// 	if (slider.value == slider.maxValue)
	// 	{
	// 		objectToActivate.SetActive(true);
	// 		objectToDeactivate.SetActive(false);
	// 	}
	// }
}
