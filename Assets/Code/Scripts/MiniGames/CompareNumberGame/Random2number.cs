using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Random2number : MonoBehaviour
{
	
	[SerializeField]
	public TMPro.TextMeshProUGUI firstNumber;
	public TMPro.TextMeshProUGUI secondNumber;
	public Button button;
	int[] randomNumbers = new int[2];

	void Start()
    {
		randomNumbers[0] = Random.Range(0, 100);
		randomNumbers[1] = Random.Range(0, 100);
	}

	// Update is called once per frame
	void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			Start();
			firstNumber.text = randomNumbers[0].ToString();
			secondNumber.text = randomNumbers[1].ToString();
		}
    }
}
