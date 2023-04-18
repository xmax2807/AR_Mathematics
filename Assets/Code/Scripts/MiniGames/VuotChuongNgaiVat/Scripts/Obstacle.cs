using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Collider eventTrigger;
    private OctopusController octopus;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider octopusCollider){
        if(!octopusCollider.TryGetComponent<OctopusController>(out octopus)) return;
        
        
    }
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
