using System.Collections;
using System.Collections.Generic;
using Project.Managers;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] tilePrefabs;
    public float xSpawn = 0;
    public float tileLength = 30;
    public int numberOfTiles = 3;
    public float spawnRate = 5.2f;
    Queue<GameObject> listMap;
    private Vector3 direction;
    public float forwardSpeed = -2;
    void Start()
    {
        direction = Vector3.right * forwardSpeed;
        listMap = new();
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
        }
        //TimeCoroutineManager.Instance.DoLoopAction(SpawnTileRandomly, stopCondition: () => false, spawnRate);

    }
    public void SpawnTileRandomly()
    {
        SpawnTile(Random.Range(0, tilePrefabs.Length));
    }
    private void Update()
    {
        this.transform.position += direction * Time.deltaTime;
    }
    private void SpawnTile(int tileIndex)
    {
        var obj = Instantiate(tilePrefabs[tileIndex], transform);
        obj.transform.localPosition = Vector3.right * xSpawn;
        obj.transform.rotation = tilePrefabs[tileIndex].transform.rotation;
        xSpawn += tileLength;
        listMap.Enqueue(obj);

        if (listMap.Count > 3)
        {
            var destroyObj = listMap.Dequeue();
            Destroy(destroyObj);
        }
    }
    public void IncreaseSpeed(float speed)
    {
        forwardSpeed -= speed;
        direction = Vector3.right * forwardSpeed;
    }
    public void DecreaseSpeed(float speed)
    {
        forwardSpeed += speed;
        direction = Vector3.right * forwardSpeed;
    }
    public void SetSpeed(float speed)
    {
        forwardSpeed = speed;
        direction = Vector3.right * forwardSpeed;
    }
}
