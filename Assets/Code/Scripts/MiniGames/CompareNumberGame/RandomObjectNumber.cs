using System.Collections.Generic;
using Project.MiniGames;
using Project.Utils;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class RandomObjectNumber : MonoBehaviour
{
    public GameObject[] objects;
    [SerializeField]
    public TMPro.TextMeshPro firstNumber;
    public TMPro.TextMeshPro secondNumber;
    public TMPro.TextMeshPro comparision;

    [SerializeField] private Transform parentFirstObjTransform;
    [SerializeField] private Transform parentSecondObjTransform;

    public GameObject spawnedObject { get; private set; }

    private ItemFactory<GameObject[]> factory;

    // void Start()
    // {
    //     RandomSetup();
    // }

    // private void RandomSetup()
    // {

    //     randomNumbers[0] = Random.Range(50, 500);
    //     randomNumbers[1] = Random.Range(50, 500);
    //     firstObject = objects[Random.Range(0, objects.Length)];
    //     secondObject = objects[Random.Range(0, objects.Length)];
    // }

    public void SetFactory(ItemFactory<GameObject[]> factory){
        this.factory = factory;
    }

    private void DestroyAllChildren(Transform parent)
    {
        StartCoroutine(DestroyChildren(parent));
    }

    private System.Collections.IEnumerator DestroyChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
            yield return null;
        }
    }

    private Vector3 SpawnObject(GameObject obj, int count, Transform parent, Vector3 direction, Vector3 startPosition = default)
    {
        return Project.Managers.SpawnerManager.Instance.SpawnObjectsLimitCol(obj, count, 1, direction, parent, startPosition);
    }

    public void SpawnObjectGroup(GameObject obj, int count, Transform parent, Vector3 direction)
    {
        DestroyAllChildren(parent);
        Transform groupTransform = new GameObject($"{parent.name}Group").transform;
        groupTransform.SetParent(parent, false);
        SpawnObject(obj, count, groupTransform, direction: direction);
        StaticBatchingUtility.Combine(groupTransform.gameObject);
    }

    public void SpawnMultiObjectGroup(int count, Transform parent, Vector3 direction){
        if(factory == null){
            Debug.Log("Factory is null, can't spawn object");
            return;
        }

        DestroyAllChildren(parent);

        int[] objectCounts = factory.AutoFuse(count);

        Transform groupTransform = new GameObject($"{parent.name}Group").transform;
        groupTransform.SetParent(parent, false);
        Vector3 lastPosition = default;

        for(int i = objectCounts.Length - 1; i >= 0 ; --i){
            GameObject[] listObjTierI = factory.GetTier(i);
            if(listObjTierI == null) continue;

            int randomIndex = Random.Range(0, listObjTierI.Length);
            lastPosition = SpawnObject(listObjTierI[randomIndex], objectCounts[i], groupTransform, direction: direction, startPosition: lastPosition);
        }
        StaticBatchingUtility.Combine(groupTransform.gameObject);
    }
}


