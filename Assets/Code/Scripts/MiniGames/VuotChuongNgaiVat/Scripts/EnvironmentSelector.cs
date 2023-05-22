using Project.Managers;
using UnityEngine;
public class EnvironmentSelector : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private EnvironmentPack[] packs;
    private EnvironmentPack pack;
    public void Start(){
        int index = Random.Range(0,packs.Length);
        pack = packs[0];

        GameObject player = Instantiate(pack.Player);
        var controller = player.GetComponent<OctopusController>();
        controller.SetTileManager(tileManager);
        tileManager.SetTileGroup(pack.EnvironmentTiles);
    }
    public void GetEnvironment(EnvironmentPack[] packs)
    {
        int currentChap = UserManager.Instance.CurrentUnitProgress.chapter;

        switch (currentChap)
        {
            case 3: pack = packs[0]; break;
            case 4: pack = packs[1]; break;
            case 5: pack = packs[2]; break;
        }

        GameObject player = Instantiate(pack.Player);
        var controller = player.GetComponent<OctopusController>();
        controller.SetTileManager(tileManager);
        tileManager.SetTileGroup(pack.EnvironmentTiles);
    }
}