using System;
using System.Collections.Generic;
using UnityEngine;
using Project.Utils.ExtensionMethods;
namespace Project.MiniGames{
    public class BaseGameEventManager : SingletonEventManager<BaseGameEventManager>, IEventListener{
        #region ListenerActionClasses
        public class ListenerAction{
            public string ListenerName;
            protected System.Action<object> Action;
            public ListenerAction(string listenerName, System.Action<object> action){
                this.ListenerName = listenerName;
                this.Action = action;
            }
            public virtual void Invoke(object value){
                Action?.Invoke(value);
            }
        }
        public class ListenerAction<T> : ListenerAction{

            private readonly Action<T> RealAction;
            public ListenerAction(string listenerName, Action<T> action) : base(listenerName, (obj)=>action((T)obj))
            {
                RealAction = action;
            }
            public override void Invoke(object value)
            {
                if(value is not T realValue){
                    Action.Invoke(value);
                    return;
                }
                RealAction.Invoke(realValue);
            }
        }
        

        [System.Serializable]
        public struct GameEventStruct{
            public string Name;
            public EventSTO GameEvent;
        }
        #endregion

        
        public static T RealInstance<T>() where T : BaseGameEventManager{
             var result = Instance.CastTo<T>();
            if(result == null){
                Debug.Log("Failed to get game event instance: " + typeof(T).Name);
            }
            return result;
        }

        [SerializeField] protected GameEventStruct[] GameEvents;

        Dictionary<string, List<ListenerAction>> allListenersAction;
        
        protected Dictionary<string, GameEventStruct> hashEventName;

        #region ConstEventName
        public const string StartGameEventName = "GameStartEvent";
        public const string EndGameEventName = "GameEndEvent";
        #endregion

        public string UniqueName => "GameEventManager";
        protected override void Awake(){
            base.Awake();
            Debug.Log("init");
            InitHashEventName();
            InitAllListenersAction();
            RegisterAllAvailableEvent();
        }
        private void OnDestroy(){
            foreach(List<ListenerAction> list in allListenersAction.Values){
                list.Clear();
            }
            foreach(GameEventStruct gameEvent in hashEventName.Values){
                gameEvent.GameEvent.UnregisterListener(this);
            }
        }

        protected virtual void InitHashEventName(){
            hashEventName = new(GameEvents.Length);
            foreach(GameEventStruct gameEvent in GameEvents){
                hashEventName.TryAdd(gameEvent.Name, gameEvent);
            }
        }
        protected virtual void InitAllListenersAction(){
            allListenersAction = new(GameEvents.Length);
            // {
            //     {GameLoadedEvent.name, new List<ListenerAction>(5)},
            //     {GameStartedEvent.name, new List<ListenerAction>(5)},
            //     {GameEndedEvent.name, new List<ListenerAction>(5)},
            // };

            foreach(GameEventStruct gameEvent in GameEvents){
                allListenersAction.TryAdd(gameEvent.GameEvent.name, new List<ListenerAction>(5));
            }
        }
        protected virtual void RegisterAllAvailableEvent(){
            // GameLoadedEvent.RegisterListener(this);
            // GameStartedEvent.RegisterListener(this);
            // GameEndedEvent.RegisterListener(this);

            foreach(GameEventStruct gameEvent in GameEvents){
                gameEvent.GameEvent?.RegisterListener(this);
            }
        }

        public void RaiseEvent<T>(string eventName, T value){
            if(!hashEventName.TryGetValue(eventName, out GameEventStruct result)){
                Debug.Log("No event found with given name: " + eventName);
                return;
            }
            result.GameEvent.Raise<T>(value);
        }
        public void RaiseEvent(string eventName){
            RaiseEvent<bool>(eventName, default);   
        }

        public void RegisterEvent(string eventName,IEventListener listener, System.Action onEventPraised){
            if(onEventPraised == null){
                Debug.Log("Can't register without action callback");
                return;
            }
            
            string realName = FindEventRealName(eventName);
            if(realName == "") return;

            int index = allListenersAction[realName].FindIndex((x)=>x.ListenerName == listener.UniqueName);
            if(index == -1){
                allListenersAction[realName].Add(new ListenerAction(listener.UniqueName, (fake)=>onEventPraised()));
            }
            else{
                allListenersAction[realName][index] = new ListenerAction(listener.UniqueName, (fake)=>onEventPraised());
            }
        }

        public void RegisterEvent<T>(string eventName,IEventListener listener, System.Action<T> onEventPraised){
            if(onEventPraised == null){
                Debug.Log("Can't register without action callback");
                return;
            }

            string realName = FindEventRealName(eventName);
            if(realName == "") return;

            int index = allListenersAction[realName].FindIndex((x)=>x.ListenerName == listener.UniqueName);
            if(index == -1){
                allListenersAction[realName].Add(new ListenerAction<T>(listener.UniqueName, onEventPraised));
            }
            else{
                allListenersAction[realName][index] = new ListenerAction<T>(listener.UniqueName, onEventPraised);
            }
        }
        public void UnregisterListener(string eventName, IEventListener listener){
            string realName = FindEventRealName(eventName);
            if(realName == "") return;

            int index = allListenersAction[realName].FindIndex((x)=>x.ListenerName == listener.UniqueName);
            if(index == -1){
                Debug.Log($"{listener.UniqueName} is not listening to {eventName}");
                return;
            }
            allListenersAction[realName].RemoveAt(index);
        }

        private string FindEventRealName(string givenName){
            if(!hashEventName.TryGetValue(givenName, out GameEventStruct gameEvent)){
                Debug.Log("Can't find event name with given: " + givenName);
                return "";
            }
            return gameEvent.GameEvent.name;
        }

        // public void AskWhichEventRaised(EventSTO source, IEventListener asker){
        //     bool isAvailable = allListenersAction.TryGetValue(source.name, out List<ListenerAction> listenersAction);
            
        //     if(!isAvailable) {
        //         Debug.Log($"No event {source.name} found in this manager");
        //         return;
        //     }

        //     ListenerAction listenerAction = listenersAction.Find((x)=>x.ListenerName == asker.UniqueName);
        //     if(listenerAction.Action == null){
        //         Debug.Log($"No action of {asker} found in event {source.name}");
        //         return;
        //     }
        //     listenerAction.Action?.Invoke();
        // }

        public virtual void OnEventRaised<T>(EventSTO sender, T result)
        {
            bool isAvailable = allListenersAction.TryGetValue(sender.name, out List<ListenerAction> actions);
            if(isAvailable){
                for(int i = actions.Count - 1; i >= 0; --i){
                   //Debug.Log(actions[i].ListenerName);
                    actions[i].Invoke(result);
                }
                return;
            }
            Debug.Log($"{sender.name} doesn't have any actions");
        }
    }
}