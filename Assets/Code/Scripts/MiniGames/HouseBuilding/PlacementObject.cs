using DnsClient.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlacementObject : MonoBehaviour
{
    [SerializeField] Rigidbody Rigidbody;
    private bool IsSelected;
    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
    public bool Selected
    {
        get
        {
            return this.IsSelected;
        }
        set
        {
            IsSelected = value;
        }
    }
    public int ID;

    public void AddFore(float force)
    {
        //Rigidbody.AddForce(new Vector3(Random.Range(-1f, 1f), 1 , Random.Range(-1f, 1f)) * force);
        Rigidbody.AddForceAtPosition(new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)) * force, new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)));
        //Rigidbody.AddExplosionForce(force, this.transform.position, 100);
    }
}
