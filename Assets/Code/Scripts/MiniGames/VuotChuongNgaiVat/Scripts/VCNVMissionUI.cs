using Project.MiniGames;
using UnityEngine;
public class VCNVMissionUI : MonoBehaviour
{
    [SerializeField] private Canvas missionCanvas;
    [SerializeField] private TaskGiver giver;
    [SerializeField] private EventSTO eventSTO;

    #region GUI
    [SerializeField] private TMPro.TextMeshProUGUI questionTitle;

    public string Name => name;
    #endregion

    public void OnEventRaised(EventSTO sender,object value)
    {
        // pop ui
    }
}