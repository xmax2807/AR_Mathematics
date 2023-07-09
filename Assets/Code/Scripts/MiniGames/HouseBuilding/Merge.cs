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

            
            if(placementObject.ID > ID)
            {                
                if (NextMerge == null) return;

                Destroy(collideObject);

                gameObject.SetActive(false);
                NextMerge.gameObject.SetActive(true);
                NextMerge.PrevMerge = this;
                OnCollision();
            }
            else
            {
                Debug.Log("Fail");
                placementObject.AddFore(150f);
                TimeCoroutineManager.Instance.WaitForSeconds(1.5f, ()=>{
                    Destroy(collideObject);
                    OnCollision();
                    HouseBuildingEventManager.Instance.RaiseEvent(HouseBuildingEventManager.ReturnPrevBuildEventName);
                });
            }

        }
    }

    private void OnCollision(){
        HouseBuildingEventManager.Instance.RaiseEvent(HouseBuildingEventManager.BlockPlacedEventName, value: NextMerge.ID);
    }
}
