using UnityEngine;
namespace Project.MiniGames{
    public sealed class VCNVGameEventManager : BaseGameEventManager<VCNVGameEventManager>{
        [SerializeField] private EventSTO obstacleReachEvent;
        [SerializeField] private EventSTO answerResultEvent;
        public EventSTO AnswerResultEvent => hashEventName[AnswerResultEventName].GameEvent;
        public EventSTO ObstacleReachEvent => hashEventName[ObstacleReachEventName].GameEvent;
        public const string ObstacleReachEventName = "ObstacleEvent";
        public const string AnswerResultEventName = "AnswerResultEvent";

        // protected override void InitHashEventName()
        // {
        //     base.InitHashEventName();
        //     hashEventName.Add("ObstacleReach", ObstacleReachEvent.name);
        //     hashEventName.Add("AnswerResult", AnswerResultEvent.name);
        // }
        // protected override void InitAllListenersAction()
        // {
        //     base.InitAllListenersAction();
        //     ObstacleReachEvent.RegisterListener(this);
        //     AnswerResultEvent.RegisterListener(this);
        // }
    }
}