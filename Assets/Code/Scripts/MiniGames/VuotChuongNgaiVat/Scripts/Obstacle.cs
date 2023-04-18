using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Managers;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Collider eventTrigger;
    private OctopusController octopus;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider octopusCollider){
        if(!octopusCollider.TryGetComponent<OctopusController>(out octopus)) return;
        
        Debug.Log(octopus.name);
        Time.timeScale = 0;
        TimeCoroutineManager.Instance.WatiForFixedSeconds(1, ()=> Time.timeScale = 1);
    }
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
