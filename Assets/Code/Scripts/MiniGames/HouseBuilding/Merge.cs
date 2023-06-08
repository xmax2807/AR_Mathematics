using Project.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.MiniGames.HouseBuilding;

public class Merge : MonoBehaviour
{
    public int ID;
    public Merge PrevMerge;
    //public Merge cube_merge;
    public Merge NextMerge;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collideObject = collision.gameObject;
        if(collideObject.CompareTag("tag"))
        {
            if(!collideObject.TryGetComponent<PlacementObject>(out PlacementObject placementObject))
            {
                Destroy(collideObject);
                return;
            }
//            Debug.Log(placementObject.ID);
            if(placementObject.ID > ID)
            {
                //if () { return; }
                //Debug.Log("Merge" + gameObject.name);
                
                if (NextMerge == null) return;

                Destroy(collideObject);
                gameObject.SetActive(false);
                NextMerge.gameObject.SetActive(true);
                //NextMerge = Instantiate(NextMerge, this.transform.parent);
                //cube_merge = NextMerge;
                NextMerge.PrevMerge = this;
                //cube_merge.ID = ID + 1;
                

                /*fwPrefab = Instantiate(firework, new Vector3 (0, 0 ,-2), fwPrefab.transform.rotation);
                fwPrefab.gameObject.SetActive(true);
                fwPrefab.Play(true);
                Debug.Log("firework is running");
                TimeCoroutineManager.Instance.WaitForSeconds(fwPrefab.main.duration, () => fwPrefab.Stop(true, ParticleSystemStopBehavior.StopEmitting));*/

                OnSuccessfulMerge();
                //TimeCoroutineManager.Instance.WaitForSeconds(2f, OnSuccessfulMerge);
            }
            else
            {
                Debug.Log("Fail");
                placementObject.AddFore(150f);
                TimeCoroutineManager.Instance.WaitForSeconds(1.5f, ()=>Destroy(collideObject));
            }
        }
    }

    private void OnSuccessfulMerge(){
        HouseBuildingEventManager.Instance.RaiseEvent(HouseBuildingEventManager.BlockPlacedEventName, value: NextMerge.ID);
    }
}
