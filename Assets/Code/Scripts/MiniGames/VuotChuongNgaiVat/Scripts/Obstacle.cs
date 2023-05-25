using UnityEngine;
using Project.Managers;
using Project.MiniGames;

public class Obstacle : MonoBehaviour
{
    private PlayerController player;

    private void RaiseColliderTriggerEvent()
    {
        //TimeCoroutineManager.Instance.PauseGame(1);
  //      Debug.Log("Triggered event ");
        BaseGameEventManager.RealInstance<VCNVGameEventManager>().ObstacleReachEvent?.Raise();
    }

    // Start is called before the first frame update
    void OnTriggerEnter(Collider octopusCollider){
//        Debug.Log("triggered " + octopusCollider.name);
        if(!octopusCollider.TryGetComponent<PlayerController>(out player)) return;

        RaiseColliderTriggerEvent();
    }

}
