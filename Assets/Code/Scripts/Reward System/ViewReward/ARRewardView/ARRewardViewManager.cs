using UnityEngine;
using Project.ARBehaviours;
using System.Collections.Generic;
using Project.UI.GameObjectUI;
using Project.Utils.ExtensionMethods;
using Project.UI.Panel;
using System;

namespace Project.RewardSystem.ViewReward
{
    [RequireComponent(typeof(PlanePlacer))]
    public class ARRewardViewManager : MonoBehaviour
    {
        [SerializeField] private PlanePlacer m_objectPlanePlacer;
        [SerializeField] private GameObject testObject;
        [SerializeField] private bool isDebug = true;
        [SerializeField] private MenuPanelController uiOptions;
        [SerializeField] private ARRewardUIManager UIManager; 
        private ARModelRewardPackSTO listRewardPrefabs;
        private void Awake()
        {
            m_objectPlanePlacer = GetComponent<PlanePlacer>();
            UIManager = GetComponent<ARRewardUIManager>();
            UIManager.OnItemChooseEvent += OnRewardItemClicked;
            m_objectPlanePlacer.OnSpawnMainPlane += OnObjectPlaced;
        }

        private void Start()
        {
            InteractionEventsBehaviour.Instance.BlockRaycast();
            if (isDebug)
            {
                m_objectPlanePlacer.SetPlacedPrefabAndStart(testObject);
            }
            else
            {
                m_objectPlanePlacer.SetPrefab(testObject);
            }
            listRewardPrefabs = Managers.ResourceManager.Instance.ARModelRewardPack;
            UIManager.SetupUI(listRewardPrefabs);
            m_objectPlanePlacer.TurnOffPlaneDetector();
        }

        private void OnDestroy(){
            listRewardPrefabs.ManualUnloadAsset();
        }

        // private void SetupUI(){
        //     GridPanelViewData data = ScriptableObject.CreateInstance<GridPanelViewData>();

        //     data.Title = "Em hãy chọn 1 mô hình có sẵn";

        //     UnlockableImageButtonData[] imageButtonDatas = new UnlockableImageButtonData[listRewardPrefabs.Count];
        //     for(int i = 0; i < imageButtonDatas.Length; ++i){
        //         int cacheIndex = i;
        //         ARRewardSTO reward = listRewardPrefabs.GetAt(i);
        //         if(reward == null) continue;

        //         UnityEngine.UI.Button.ButtonClickedEvent clickEvent = new();
        //         clickEvent.AddListener(()=>OnRewardItemClicked(cacheIndex));

        //         imageButtonDatas[i] = new UnlockableImageButtonData(){
        //             isUnlocked = acquiredRewardUniqueNames.IsContains((x)=>x == reward.UniqueName),
        //             Image = reward.Avatar,
        //             OnClick = clickEvent,
        //         };
        //     }
        //     data.ButtonNames = imageButtonDatas;
        //     listModelChooser.SetUI(data);
        // }

        private async void OnRewardItemClicked(int index)
        {
            Debug.Log("Clicked at " + index);
            ARRewardSTO rewardSTO = listRewardPrefabs.GetAt(index);
            GameObject model = await rewardSTO.GetModel();
            
            if (isDebug)
            {
                m_objectPlanePlacer.SetPlacedPrefabAndStart(model);
            }
            else
            {
                m_objectPlanePlacer.SetPrefab(model);
            }
            m_objectPlanePlacer.TurnOnPlaneDetector();
        }

        private void OnObjectPlaced(GameObject obj)
        {
            m_objectPlanePlacer.TurnOffPlaneDetector();
            ScaleToDesiredSize(obj);
            
            Transform wrapperTransform = new GameObject(obj.name).transform;
            wrapperTransform.position = obj.transform.position;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.SetParent(wrapperTransform, false);

            //Rotate to face camera
            Vector3 targetDirection = Managers.GameManager.MainGameCam.transform.position - wrapperTransform.position;
            float angle = Vector3.SignedAngle(wrapperTransform.forward, targetDirection, Vector3.up);
            wrapperTransform.Rotate(0, angle, 0);
            //

            ARRewardController controller = wrapperTransform.gameObject.AddComponent<ARRewardController>();
            controller.SetModel(obj);
            controller.SetUIController(uiOptions);

            InteractionEventsBehaviour.Instance.UnblockRaycast();
        }

        // private void OnObjectTouch(GameObjectButton obj)
        // {
        //     foreach (GameObjectButton childObj in listSpawnedObjects)
        //     {
        //         if (obj.Equals(childObj))
        //         {

        //             childObj.Interactable = false;

        //             if (currentUIOptions != null)
        //             {
        //                 Destroy(currentUIOptions.gameObject);
        //             }
        //             // Show ui to replace or remove this object
        //             currentUIOptions = Instantiate(uiOptions, childObj.transform);
        //             currentUIOptions.worldCamera = Managers.GameManager.MainGameCam;
        //             currentUIOptions.enabled = true;
        //             Debug.Log("UI instantiated");
        //         }
        //     }
        // }

        private void ScaleToDesiredSize(GameObject obj, float desiredSize = 0.4f)
        {
            Bounds bounds = obj.GetBoundsFromRenderer();
            Vector3 currentSize = bounds.size;

            BoxCollider box = obj.AddComponent<BoxCollider>();

            float minValue = Mathf.Max(currentSize.x, Mathf.Max(currentSize.y, currentSize.z));
            float ratio = desiredSize / minValue;
            Vector3 localScale = obj.transform.localScale;
            Vector3 scaleRatio = new Vector3(ratio * localScale.x, ratio * localScale.y, ratio * localScale.z);
            obj.transform.localScale = scaleRatio;
            //bounds = obj.GetBoundsFromRenderer();
            box.size = new Vector3(currentSize.x, currentSize.y, currentSize.z);
            box.center = new Vector3(0, currentSize.y / 2, 0);
            //box.center = bounds.center;
        }
    }
}