using UnityEngine;
using Project.Managers;
using System.Linq;

namespace Project.UI.GameObjectUI
{
    public class InteractionEventsBehaviour : MonoBehaviour
    {
        [SerializeField] private LayerMask filteredMasks;
        private Camera MainCam => GameManager.MainGameCam;
        private bool m_isBlockTouching = false;

        public void Update()
        {
            if (m_isBlockTouching == true)
            {
                return;
            }
            HandleTouch();
        }

        private void HandleTouch()
        {
            if (Input.touchCount <= 0) return;

            Touch theTouch = Input.GetTouch(0);
            Ray touchPositionWorld = MainCam.ScreenPointToRay(theTouch.position);

            bool tryRaycast = Physics.Raycast(touchPositionWorld, out RaycastHit hitObject, 100, filteredMasks);
            if (tryRaycast == false)
            {
                return;
            }

            Collider objectCollider = hitObject.collider;
            Debug.Log($"InteractionEvent: RaycastHit {objectCollider.name}");


            if (objectCollider.GetComponent(typeof(ITouchableObject)) is ITouchableObject obj)
            {
                obj.OnTouch(theTouch.phase);
            }
        }

        public void BlockRaycast() => m_isBlockTouching = true;
        public void UnblockRaycast() => m_isBlockTouching = false;
    }
}