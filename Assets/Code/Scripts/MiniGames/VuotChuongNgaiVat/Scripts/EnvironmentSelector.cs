using System.Collections.Generic;
using Project.Managers;
using UnityEngine;
public class EnvironmentSelector : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    
    // public void Start(){
    //     int index = Random.Range(0,packs.Length);
    //     pack = packs[0];

    //     GameObject player = Instantiate(pack.Player);
    //     var controller = player.GetComponent<OctopusController>();
    //     controller.SetTileManager(tileManager);
    //     tileManager.SetTileGroup(pack.EnvironmentTiles);
    // }
    private void GetEnvironment(EnvironmentPack[] packs)
    {
        if(packs == null || packs.Length == 0) {
            Debug.Log("No environment is available");
            return;
        }

        UserManager.CurrentUnit currentUnit = UserManager.Instance.CurrentUnitProgress; 
        int currentChap = currentUnit.chapter;

        EnvironmentPack pack;
        int max = packs.Length - 1;
        switch (currentChap)
        {
            case 3: pack = packs[0]; break;
            case 4: pack = packs[Mathf.Min(1, max)]; break;
            case 5: pack = packs[Mathf.Min(2, max)]; break;
            default: pack = packs[Mathf.Min(2, max)]; break;
        }

        GameObject player = Instantiate(pack.Player);
        player.transform.position += new Vector3(0,-5,8);
        player.AddComponent<UnityEngine.XR.ARFoundation.ARAnchor>();
        var controller = player.GetComponent<PlayerController>();
        controller.SetTileManager(tileManager);
        tileManager.SetTileGroup(pack.EnvironmentTiles);
    }
    public void GetEnvironment(ScriptableObject[] packs){
        if(packs == null){
            //TODO Pop game can't be played
            return;
        }
        List<EnvironmentPack> realPacks = new(packs.Length);
        
        foreach(ScriptableObject pack in packs){
            if(pack is EnvironmentPack realPack){
                realPacks.Add(realPack);
            }
        }

        GetEnvironment(realPacks.ToArray());
    }
}