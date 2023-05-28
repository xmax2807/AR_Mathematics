using Project.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.MiniGames.HouseBuilding;

public class Merge : MonoBehaviour
{
    public int ID;
    public Merge cube_merge;
    public ParticleSystem firework;
    public BuildingManager Manager {get;set;}

    private void Awake(){
        firework ??= GetComponentInChildren<ParticleSystem>(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("tag"))
        {
            if(!collision.gameObject.TryGetComponent<PlacementObject>(out PlacementObject placementObject))
            {
                return;
            }
//            Debug.Log(placementObject.ID);
            if(placementObject.ID > ID)
            {
                //if () { return; }
                //Debug.Log("Merge" + gameObject.name);
                Merge nextMerge = Manager?.GiveNextMerge(placementObject.ID);
                
                if (nextMerge == null) return;

                collision.gameObject.SetActive(false);

                nextMerge = Instantiate(nextMerge, new Vector3 (0, 1, 0), nextMerge.transform.rotation, this.transform.parent);
                nextMerge.Manager = this.Manager;
                cube_merge = nextMerge;
                //cube_merge.ID = ID + 1;
                

                /*fwPrefab = Instantiate(firework, new Vector3 (0, 0 ,-2), fwPrefab.transform.rotation);
                fwPrefab.gameObject.SetActive(true);
                fwPrefab.Play(true);
                Debug.Log("firework is running");
                TimeCoroutineManager.Instance.WaitForSeconds(fwPrefab.main.duration, () => fwPrefab.Stop(true, ParticleSystemStopBehavior.StopEmitting));*/
                if(firework == null){
                    firework = this.GetComponentInChildren<ParticleSystem>(true);
                }

                if(firework == null){
                    Debug.Log("Firework is null");
                    gameObject.SetActive(false);
                    HouseBuildingEventManager.Instance.RaiseEvent(HouseBuildingEventManager.BlockPlacedEventName, value: cube_merge.ID);
                    return;
                }

                firework.Play(true);
                TimeCoroutineManager.Instance.WaitForSeconds(firework.main.duration, OnSuccessfulMerge);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Fail");
                placementObject.AddFore(150f);
            }
        }
    }

    private void OnSuccessfulMerge(){
        firework.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        HouseBuildingEventManager.Instance.RaiseEvent(HouseBuildingEventManager.BlockPlacedEventName, value: cube_merge.ID);
    }
}
