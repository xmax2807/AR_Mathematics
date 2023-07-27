using System.Collections;
using System.Collections.Generic;
using Project.MiniGames;
using UnityEngine;

public class PlaneController : MonoBehaviour
{   
    [SerializeField] Obstacle[] availableObstacles;

    private void Awake(){
        // if(availableObstacles == null || availableObstacles.Length == 0){
        //     availableObstacles = GetComponentsInChildren<Obstacle>();
        // }
    }

    // public void SetTriggerEvent(EventSTO eventSTO){
    //     foreach(Obstacle obstacle in availableObstacles){
    //         obstacle.SetTriggerEvent(eventSTO);
    //     }
    // }
}
