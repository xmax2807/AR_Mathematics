using Project.MiniGames;
using UnityEngine;
public class VCNVMissionUI : MonoBehaviour, IEventListener
{
    [SerializeField] private Canvas missionCanvas;
    [SerializeField] private TaskGiver giver;
    [SerializeField] private EventSTO eventSTO;
    public EventSTO GetEventSTO() => eventSTO;

    #region GUI
    [SerializeField] private TMPro.TextMeshProUGUI questionTitle;

    public string Name => name;
    #endregion

    public void OnEventRaised()
    {
        // pop ui
    }
}