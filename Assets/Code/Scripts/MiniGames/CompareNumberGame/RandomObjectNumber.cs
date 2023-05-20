using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Managers;
using UnityEngine.Events;
using UnityEngine.UIElements;
using System;
using Random = UnityEngine.Random;

public class RandomObjectNumber : MonoBehaviour
{
	UnityEvent placementUpdate;
	public List<GameObject> objects;
	[SerializeField]
	public TMPro.TextMeshProUGUI firstNumber;
	public TMPro.TextMeshProUGUI secondNumber;
	public TMPro.TextMeshProUGUI comparision;

	public int RandomNumberRange;


	[SerializeField] private Transform parentFirstObjTransform;
	[SerializeField] private Transform parentSecondObjTransform;

	private GameObject firstObject;
	private GameObject secondObject;

	public Vector3 rotateObject;

	int[] randomNumbers = new int[2];

	public GameObject spawnedObject { get; private set; }

	void Start()
	{
		randomNumbers[0] = Random.Range(0, 20);
		randomNumbers[1] = Random.Range(0, 20);
		firstObject = objects[Random.Range(0, objects.Count)];
		secondObject = objects[Random.Range(0, objects.Count)];
	}

	//void Awake()
	//{
	//	if (Input.GetMouseButtonDown(0))
	//	{
	//		Start();

	//		if (firstNumber.text != randomNumbers[0].ToString() & secondNumber.text != randomNumbers[1].ToString())
	//		{
	//			firstNumber.text = randomNumbers[0].ToString();
	//			secondNumber.text = randomNumbers[1].ToString();
	//		}

	//		if (firstObject != null)
	//		{
	//			for (int i = 0; i < randomNumbers[0]; i++)
	//			{
	//				float y = i / 10;
	//				float x = i % 10;
	//				Instantiate(firstObject, parentFirstObjTransform);
	//				var position = firstObject.transform.position;
	//				position.x = position.x + x;
	//				position.y = position.y + y;
	//				firstObject.transform.position = position;
	//				var rotation = firstObject.transform.rotation;
	//				firstObject.transform.Rotate(rotation.x + rotateObject.x, rotation.y + rotateObject.y, rotation.z + rotateObject.z);
	//				Debug.Log("First spawned: " + i + " " + firstObject.transform.position + " " + firstObject.transform.rotation);
	//			}
	//		}
	//		if (secondObject != null)
	//		{
	//			for (int i = 0; i < randomNumbers[1]; i++)
	//			{
	//				float x = i / 10;
	//				float y = i % 10;
	//				Instantiate(secondObject, parentSecondObjTransform);
	//				var position = secondObject.transform.position;
	//				position.x = position.x + x;
	//				position.y = position.y + y;
	//				secondObject.transform.position = position;
	//				var rotation = secondObject.transform.rotation;
	//				secondObject.transform.Rotate(rotation.x + rotateObject.x, rotation.y + rotateObject.y, rotation.z + rotateObject.z);
	//				Debug.Log("Second spawned: " + i + " " + secondObject.transform.position + " " + secondObject.transform.rotation);
	//			}
	//		}
	//	}
	//	if (Input.GetMouseButton(0))
	//	{
	//		if (randomNumbers[0] > randomNumbers[1])
	//		{
	//			comparision.text = ">";
	//		}
	//		else if (randomNumbers[0] < randomNumbers[1])
	//		{
	//			comparision.text = "<";
	//		}
	//		else
	//		{
	//			comparision.text = "=";
	//		}
	//	}
	//}
	//Update is called once per frame

void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Start();

			if (firstNumber.text != randomNumbers[0].ToString() & secondNumber.text != randomNumbers[1].ToString())
			{
				firstNumber.text = randomNumbers[0].ToString();
				secondNumber.text = randomNumbers[1].ToString();
			}

			if (firstObject != null)
			{
				for (int i = 0; i < randomNumbers[0]; i++)
				{
					float y = i / 10;
					float z = i % 10;
					Instantiate(firstObject, parentFirstObjTransform);
					var position = firstObject.transform.position;
					position.z = z + parentFirstObjTransform.position.z;
					position.y = y + 1;
					firstObject.transform.position = position;
					var rotation = firstObject.transform.rotation;
					//firstObject.transform.Rotate(rotateObject.x, rotateObject.y, rotateObject.z);
					Debug.Log("First spawned: " + i + " " + firstObject.transform.position + " " + firstObject.transform.rotation);
				}
			}
			if (secondObject != null)
			{
				for (int i = 0; i < randomNumbers[1]; i++)
				{
					float y = i / 10;
					float z = i % 10;
					Instantiate(secondObject, parentSecondObjTransform);
					var position = secondObject.transform.position;
					position.z = z + parentSecondObjTransform.position.z;
					position.y = y + 1;
					secondObject.transform.position = position;
					var rotation = secondObject.transform.rotation;
					//secondObject.transform.Rotate(rotateObject.x, rotateObject.y, rotateObject.z);
					Debug.Log("Second spawned: " + i + " " + secondObject.transform.position + " " + secondObject.transform.rotation);
				}
			}
			Destroy(firstObject, 5);
			Destroy(secondObject, 5);
		}
		if (Input.GetMouseButton(0))
		{
			if (randomNumbers[0] > randomNumbers[1])
			{
				comparision.text = ">";
			}
			else if (randomNumbers[0] < randomNumbers[1])
			{
				comparision.text = "<";
			}
			else
			{
				comparision.text = "=";
			}
		}
		
		placementUpdate.Invoke();

	}
}


