using UnityEngine;
using Project.Managers;

public class Obstacle : MonoBehaviour
{
    private OctopusController octopus;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider octopusCollider){
        Debug.Log("triggered " + octopusCollider.name);
        if(!octopusCollider.TryGetComponent<OctopusController>(out octopus)) return;
        
        Debug.Log(octopus.name);
        Time.timeScale = 0;
        TimeCoroutineManager.Instance.WatiForFixedSeconds(1, ()=> Time.timeScale = 1);
        octopus.SpawnTile();
    }
}
