using UnityEngine;
using Project.Managers;

public class Obstacle : MonoBehaviour
{
    private PlayerController player;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider octopusCollider){
        Debug.Log("triggered " + octopusCollider.name);
        if(!octopusCollider.TryGetComponent<PlayerController>(out player)) return;
        
        Debug.Log(player.name);
        Time.timeScale = 0;
        TimeCoroutineManager.Instance.WatiForFixedSeconds(1, ()=> Time.timeScale = 1);
        player.SpawnTile();
    }
}
