using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Managers;

public class RandomObjectNumber : MonoBehaviour
{
  
	public List<GameObject> objects;
	[SerializeField]
	public TMPro.TextMeshProUGUI firstNumber;
	public TMPro.TextMeshProUGUI secondNumber;

	[SerializeField] private Transform parentFirstObjTransform;
	[SerializeField] private Transform parentSecondObjTransform;

	private GameObject firstObject;
	private GameObject secondObject;

	int[] randomNumbers = new int[2];
	void Start()
	{
		randomNumbers[0] = Random.Range(0, 100);
		randomNumbers[1] = Random.Range(0, 100);
		firstObject = objects[Random.Range(0, objects.Count)];
		secondObject = objects[Random.Range(0, objects.Count)];

		// spawn
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


