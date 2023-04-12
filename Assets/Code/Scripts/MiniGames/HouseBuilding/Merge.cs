using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merge : MonoBehaviour
{
    private string tag;
    public int ID;
    public Merge cube_merge;
    // Start is called before the first frame update
    void Start()
    {
        tag = "tag";
    }

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
                Destroy(gameObject);

                if (cube_merge == null) return;
                Instantiate(cube_merge.gameObject, new Vector3 (0, 1, 0), cube_merge.transform.rotation);
                cube_merge.ID = ID + 1;
            }
            else
            {
                Debug.Log("Fail");
                placementObject.AddFore(150f);
            }
        }
    }
}
