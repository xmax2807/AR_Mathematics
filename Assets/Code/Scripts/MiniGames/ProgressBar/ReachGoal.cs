using Project.UI.ProgressBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Project.UI;

public class ReachGoal : MonoBehaviour, IProgressUI
{
    public Slider slider;

    private float newValue;
    private float currentValue;

    private bool IsSliderReachedMax()
    {
        return slider.value == slider.maxValue;
    }

    // public void Awake(){
    // 	m_controller = new SwapableLogicDefault<GameObject>(objectToDeactivate, new SwapableLogic<GameObject>(objectToActivate), IsSliderReachedMax);
    // }
    public void Start()
    {
        slider.onValueChanged.AddListener(OnValueChanged);
        SwitchActive(isMaxReached: false);
        currentValue = slider.value;
        newValue = currentValue;
    }
    public List<GameObject> objectToActivates;
    public List<GameObject> objectToDeactivates;

    void Update()
    {
        if(!Mathf.Approximately(currentValue, newValue)){
            currentValue += Time.deltaTime;
            currentValue = Mathf.Clamp(currentValue, 0, newValue);
            slider.value = currentValue;
        }
    }
    void OnValueChanged(float value)
    {
        bool result = IsSliderReachedMax();
        SwitchActive(result);
    }
	private void SwitchActive(bool isMaxReached){
		foreach (GameObject obj in objectToActivates)
        {
            obj.SetActive(isMaxReached);
        }
        foreach (GameObject obj in objectToDeactivates)
        {
            obj.SetActive(!isMaxReached);
        }
	}

    public void IncreaseProgress(float value)
    {
        value = value / slider.maxValue + slider.minValue;
        SetProgress(currentValue + value);
    }

    public void SetProgress(float value)
    {
        newValue = value / slider.maxValue + slider.minValue;
    }
}
