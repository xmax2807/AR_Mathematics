using System.Collections;
using System.Collections.Generic;
using Project.MiniGames;
using UnityEngine;

public class TileManager : MonoBehaviour, IEventListener
{
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
        SetDirection(0);
        //SetupEnvironment();
        //TimeCoroutineManager.Instance.DoLoopAction(SpawnTileRandomly, stopCondition: () => false, spawnRate);
    }

    private void SetDirection(float speed){
        direction = Vector3.right * speed;
    }
    private void SetDirection() => SetDirection(forwardSpeed);
    private void SetupEnvironment(){
        //SetDirection(forwardSpeed);
        listMap = new();
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTileRandomly();
        }
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
        // if(obj.TryGetComponent(out PlaneController controller)){
        //     controller.SetTriggerEvent(this.eventSTO);
        // }
        xSpawn += tileLength;
        listMap.Enqueue(obj);
        

        // numberOftile = 3
        // listMap = 4
        canSpawn = listMap.Count <= 9;
        if (!canSpawn)
        {
            for(int i = 0; i < numberOfTiles; ++i){
                var destroyObj = listMap.Dequeue();
                Destroy(destroyObj);
            }
            canSpawn = true;
        }
    }

    public void SetTileGroup(GameObject[] group) {
        tilePrefabs = new GameObject[group.Length];
        for(int i = 0; i < group.Length; ++i){
            tilePrefabs[i] = group[i];
        }
        //tilePrefabs = group;
        SetupEnvironment();
    }

    private bool canSpawn = true;
    public void OnObstacleReach()
    {
        if(canSpawn){
            SpawnTileRandomly();
        }
        SetDirection(0);
        //Project.Managers.TimeCoroutineManager.Instance.WaitForSeconds(1, ()=>SetDirection(forwardSpeed));
    }
    public string UniqueName => "TileManager";
    public void OnEnable(){
        BaseGameEventManager.Instance.RegisterEvent(BaseGameEventManager.StartGameEventName, this, OnGameStarted);
        VCNVGameEventManager.Instance.RegisterEvent<bool>(VCNVGameEventManager.AnswerResultEventName, this, OnAnswerQuestion);
        VCNVGameEventManager.Instance.RegisterEvent(VCNVGameEventManager.ObstacleReachEventName, this, OnObstacleReach);
        BaseGameEventManager.Instance.RegisterEvent(BaseGameEventManager.EndGameEventName, this, OnGameEnded);
    }

    private void OnGameStarted()
    {
        SetDirection(forwardSpeed);
    }

    public void OnEventRaised<T>(EventSTO sender, T result)
    {
        // Debug.Log(result);
        // if(result is not bool realResult) return;
        // if(realResult == true){
        //     SetDirection(forwardSpeed * 1.5f);
        //     Project.Managers.TimeCoroutineManager.Instance.WaitForSeconds(1, SetDirection);
        // }
        // else{
        //     //Game over
        //     SetDirection(0);
        // }
    }
    private void OnGameEnded(){
        Debug.Log("TIlemanager, game ended");
        SetDirection(0);
        enabled = false;
    }

    private void OnAnswerQuestion(bool result){
        if(result == true){
            SetDirection(forwardSpeed * 1.75f);
            Project.Managers.TimeCoroutineManager.Instance.WaitForSeconds(1, SetDirection);
        }
        else{
            //Game over
            SetDirection(0);
        }
    }
}
