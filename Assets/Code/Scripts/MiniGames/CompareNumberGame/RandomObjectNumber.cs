using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Managers;
using UnityEngine.Events;

public class RandomObjectNumber : MonoBehaviour
{
	UnityEvent placementUpdate;
	public List<GameObject> objects;
	[SerializeField]
	public TMPro.TextMeshProUGUI firstNumber;
	public TMPro.TextMeshProUGUI secondNumber;

	[SerializeField] private Transform parentFirstObjTransform;
	[SerializeField] private Transform parentSecondObjTransform;

	private GameObject firstObject;
	private GameObject secondObject;

	public Vector3 rotateObject;

	int[] randomNumbers = new int[2];
	void Start()
	{
		randomNumbers[0] = Random.Range(0, 100);
		randomNumbers[1] = Random.Range(0, 100);
		firstObject = objects[Random.Range(0, objects.Count)];
		secondObject = objects[Random.Range(0, objects.Count)];

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Start();
			firstNumber.text = randomNumbers[0].ToString();
			secondNumber.text = randomNumbers[1].ToString();
			firstObject.SetActive(true);
			secondObject.SetActive(true);
		}

		if (firstObject != null)
		{
			for (int i = 0; i < randomNumbers[0]; i++)
			{
				Instantiate(firstObject, parentFirstObjTransform);
				//firstObject.transform.LookAt(ARCamera.transform);
				var rotation = firstObject.transform.rotation;
				firstObject.transform.Rotate(rotation.x + rotateObject.x, rotation.y + rotateObject.y, rotation.z + rotateObject.z);
			}
		}
		if (secondObject != null)
		{
			for (int i = 0; i < randomNumbers[1]; i++)
			{
				Instantiate(secondObject, parentSecondObjTransform);
				//firstObject.transform.LookAt(ARCamera.transform);
				var rotation = secondObject.transform.rotation;
				secondObject.transform.Rotate(rotation.x + rotateObject.x, rotation.y + rotateObject.y, rotation.z + rotateObject.z);
			}
		}
		firstObject.SetActive(false);
		secondObject.SetActive(false);
		placementUpdate.Invoke();
	}
}


