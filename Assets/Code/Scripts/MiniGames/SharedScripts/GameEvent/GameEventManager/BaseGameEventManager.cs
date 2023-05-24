using System;
using System.Collections.Generic;
using UnityEngine;
namespace Project.MiniGames{
    public class BaseGameEventManager<TChild> : SingletonEventManager<TChild>, IEventListener where TChild : BaseGameEventManager<TChild>{
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

            private Action<T> realAction;
            public ListenerAction(string listenerName, Action<T> action) : base(listenerName, (obj)=>action((T)obj))
            {
                realAction = action;
            }
            public override void Invoke(object value)
            {
                if(value is not T realValue) return;
                realAction.Invoke(realValue);
            }
        }

        [System.Serializable]
        public struct GameEventStruct{
            public string Name;
            public EventSTO GameEvent;
        }

        [SerializeField] protected GameEventStruct[] GameEvents;

        // [SerializeField] private EventSTO GameLoadedEvent;
        // [SerializeField] private EventSTO GameStartedEvent;
        // [SerializeField] private EventSTO GameEndedEvent;

        Dictionary<string, List<ListenerAction>> allListenersAction;
        

        public string UniqueName => "GameEventManager";
        protected Dictionary<string, GameEventStruct> hashEventName;
        protected override void Awake(){
            base.Awake();
            Debug.Log("init");
            InitHashEventName();
            InitAllListenersAction();
            RegisterAllAvailableEvent();
        }
        // private void OnDestroy(){
        //     foreach(List<ListenerAction> list in allListenersAction.Values){
        //         list.Clear();
        //     }
        // }

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

        public void RegisterEvent(string eventName,IEventListener listener, System.Action onEventPraised){
            if(onEventPraised == null){
                Debug.Log("Can't register without action callback");
                return;
            }
            if(!hashEventName.TryGetValue(eventName, out GameEventStruct gameEvent)){
                Debug.Log("Can't find event name with given: " + eventName);
                return;
            }
            string realName = gameEvent.GameEvent.name;
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
            if(!hashEventName.TryGetValue(eventName, out GameEventStruct gameEvent)){
                Debug.Log("Can't find event name with given: " + eventName);
                return;
            }
            string realName = gameEvent.GameEvent.name;

            int index = allListenersAction[realName].FindIndex((x)=>x.ListenerName == listener.UniqueName);
            if(index == -1){
                allListenersAction[realName].Add(new ListenerAction<T>(listener.UniqueName, onEventPraised));
            }
            else{
                allListenersAction[realName][index] = new ListenerAction<T>(listener.UniqueName, onEventPraised);
            }
            Debug.Log(allListenersAction[realName].Count);
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
                    Debug.Log(actions[i].ListenerName);
                    actions[i].Invoke(result);
                }
                return;
            }
            Debug.Log($"{sender.name} doesn't have any actions");
        }
    }
}