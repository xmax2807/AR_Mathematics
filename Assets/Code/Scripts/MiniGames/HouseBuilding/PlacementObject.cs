using DnsClient.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.MiniGames.HouseBuilding;
using Project.MiniGames;

public class PlacementObject : MonoBehaviour, IEventListener
{
    [SerializeField] Rigidbody Rigidbody;
    private bool IsSelected;
    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        HouseBuildingEventManager.Instance.RegisterEvent(HouseBuildingEventManager.ResetBuildEventName, this, OnBuildReset);
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

    public string UniqueName => "PlacementObject" + ID;

    public int ID;

    public void AddFore(float force)
    {
        //Rigidbody.AddForce(new Vector3(Random.Range(-1f, 1f), 1 , Random.Range(-1f, 1f)) * force);
        Rigidbody.AddForceAtPosition(new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 0f)) * force, new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)));
        //Rigidbody.AddExplosionForce(force, this.transform.position, 100);
    }

    public void OnEventRaised<T>(EventSTO sender, T result)
    {
        throw new System.NotImplementedException();
    }
    private void OnBuildReset() => gameObject.SetActive(true);
}
