using Project.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merge : MonoBehaviour
{
    public int ID;
    public Merge cube_merge;
    //public ParticleSystem firework;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("tag"))
        {
            if(!collision.gameObject.TryGetComponent<PlacementObject>(out PlacementObject placementObject))
            {
                return;
            }
            Debug.Log(placementObject.ID);
            if(placementObject.ID - ID == 1)
            {
                //if () { return; }
                Debug.Log("Merge" + gameObject.name);
                
                Destroy(collision.gameObject);
                //collision.gameObject.SetActive(false);
                Destroy(gameObject);
                //gameObject.SetActive(false);

                if (cube_merge == null) return;
                Instantiate(cube_merge.gameObject, new Vector3 (0, 1, 0), cube_merge.transform.rotation);
                cube_merge.ID = ID + 1;


                /*fwPrefab = Instantiate(firework, new Vector3 (0, 0 ,-2), fwPrefab.transform.rotation);
                fwPrefab.gameObject.SetActive(true);
                fwPrefab.Play(true);
                Debug.Log("firework is running");
                TimeCoroutineManager.Instance.WaitForSeconds(fwPrefab.main.duration, () => fwPrefab.Stop(true, ParticleSystemStopBehavior.StopEmitting));*/
                
            }
            else
            {
                Debug.Log("Fail");
                placementObject.AddFore(150f);
            }
        }
    }
}
