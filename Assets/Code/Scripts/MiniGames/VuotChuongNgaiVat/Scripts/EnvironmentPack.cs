using UnityEngine;

[CreateAssetMenu(menuName = "MiniGames/VCNV/EnvironmentPack", fileName ="New Environment Pack")]
public class EnvironmentPack : ScriptableObject{
    [Header("Tile environments")]
    public GameObject[] EnvironmentTiles;
    [Header("Player")]
    public GameObject Player;
}