using Project.UI.ProgressBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Project.UI;

public class ReachGoal : MonoBehaviour
{
    public Slider slider;

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
    }
    public List<GameObject> objectToActivates;
    public List<GameObject> objectToDeactivates;

    // void Update()
    // {
    //     foreach (GameObject obj in objectToActivates)
    //     {
    //         obj.SetActive(false);
    //     }
    //     foreach (GameObject obj in objectToDeactivates)
    //     {
    //         obj.SetActive(true);
    //     }

    //     if (slider.value == slider.maxValue)
    //     {
    //         foreach (GameObject obj in objectToActivates)
    //         {
    //             obj.SetActive(true);
    //         }
    //         foreach (GameObject obj in objectToDeactivates)
    //         {
    //             obj.SetActive(false);
    //         }
    //     }
    // }
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
}
