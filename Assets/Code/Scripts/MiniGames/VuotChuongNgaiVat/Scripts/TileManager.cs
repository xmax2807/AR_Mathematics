using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] tilePrefabs;
    public float xSpawn = 0;
    public float tileLength = 30;
    public int numberOfTiles = 3;

    void Start()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile(Random.Range(0,tilePrefabs.Length));
        }
    }

    public void SpawnTile(int tileIndex)
    {
        var obj = Instantiate(tilePrefabs[tileIndex], transform);
        obj.transform.position = Vector3.right * xSpawn;
        obj.transform.rotation = tilePrefabs[tileIndex].transform.rotation;
        xSpawn += tileLength;
    }
}
