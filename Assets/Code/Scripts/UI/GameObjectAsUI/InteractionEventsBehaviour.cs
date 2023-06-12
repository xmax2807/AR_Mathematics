using UnityEngine;
using Project.Managers;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Project.UI.GameObjectUI
{
    public class InteractionEventsBehaviour : MonoBehaviour
    {
        [SerializeField] int maxTouchCount = 3;
        [SerializeField] private LayerMask filteredMasks;
        private Camera MainCam => GameManager.MainGameCam;
        private bool m_isBlockTouching = false;

        private static Dictionary<int, ITouchableObject> cacheTouchObjs = new(3);

        private static InteractionEventsBehaviour _instance;
        public static InteractionEventsBehaviour Instance => _instance;

        private void Awake(){
            if(_instance == null){
                _instance = this;
            }
            else if(_instance != this){
                Destroy(this.gameObject);
            }
        }

        public void Update()
        {
            if(m_isBlockTouching == true){
                return;
            }
            
            #if UNITY_EDITOR
            if(Input.GetMouseButtonDown(0)){
                Touch converted = new(){
                    position = Input.mousePosition,
                    phase = TouchPhase.Began,
                    fingerId = 0,
                };
                Debug.Log("Mouse down");
                HandleTouch(converted);
            }
            else if(Input.GetMouseButtonUp(0)){
                Touch converted = new(){
                    position = Input.mousePosition,
                    phase = TouchPhase.Ended,
                    fingerId = 0,
                };
                Debug.Log("Mouse up");
                HandleTouch(converted);
            }
            #else
            int touchCount = Input.touchCount;
            if (touchCount <= 0 || touchCount > maxTouchCount)
            {
                return;
            }
            for(int i = 0; i < touchCount; i++){
                HandleTouch(Input.GetTouch(i));
            }
            #endif
        }

        private void HandleTouch(Touch theTouch)
        {
            Debug.Log(theTouch.phase);
            //OnTouchBegan
            if(theTouch.phase == TouchPhase.Began){
                bool tryRaycast = TryRaycastTouchableObject(theTouch);
                if(tryRaycast == false){
                    return;
                }
            }

            //Handle other interactions
            if(!cacheTouchObjs.TryGetValue(theTouch.fingerId, out ITouchableObject touchableObject)){
                return;
            }

            Debug.Log($"Touching {touchableObject.UniqueID}");
            touchableObject.OnTouch(theTouch);

            if(theTouch.phase == TouchPhase.Ended){
                cacheTouchObjs.Remove(theTouch.fingerId);
            }
        }

        private bool TryRaycastTouchableObject(Touch theTouch){
            Ray touchPositionWorld = MainCam.ScreenPointToRay(theTouch.position);

            bool tryRaycast = Physics.Raycast(touchPositionWorld, out RaycastHit hitObject, 100, filteredMasks);
            if (tryRaycast == false)
            {
                Debug.Log($"Nothing at {touchPositionWorld}");
                return false;
            }

            Collider objectCollider = hitObject.collider;
            Debug.Log($"InteractionEvent: RaycastHit {objectCollider.name}");


            if (objectCollider.GetComponent(typeof(ITouchableObject)) is ITouchableObject touchableObject)
            {
                cacheTouchObjs.Add(theTouch.fingerId, touchableObject);
                touchableObject.OnTouch(theTouch);
                return true;
            }

            Debug.Log($"{objectCollider.name} is not touchable");

            return false;
        }

        public void BlockRaycast() => m_isBlockTouching = true;
        public void UnblockRaycast() => m_isBlockTouching = false;
    }
}