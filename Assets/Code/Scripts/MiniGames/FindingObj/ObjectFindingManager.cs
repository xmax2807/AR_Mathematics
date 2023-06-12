using System;
using System.Collections;
using System.Collections.Generic;
using Project.Managers;
using UnityEngine;
namespace Project.MiniGames.ObjectFinding
{
    public class ObjectFindingManager : MonoBehaviour, IEventListener
    {
        [SerializeField] private bool IsDebugging;
        [SerializeField] private ObjectCenter objectCenter;
        private ObjectCenter objectCenterObj;
        [SerializeField] private PlaceOnPlaneHouse mainGamePlacer;
        [SerializeField] private SelecObjectFromCameraFO objectRaycaster;
        [SerializeField] private TaskGiver taskGiver;
        [SerializeField] private Animator answerGIFAnimator;
        private PlacementObject[] placementObjectPrefabs;
        private bool questionIsReady;

        private Camera mainCam;

        public string UniqueName => "ObjectFindingManager";

        private void Awake()
        {
            mainCam = Camera.main;
            if (taskGiver != null)
            {
                taskGiver.OnInitialized += OnTaskGiverReady;

            }
            mainGamePlacer.OnSpawnMainPlane += OnSpawnMainPlane;
        }

        private void OnSpawnMainPlane(GameObject obj)
        {
            if (!obj.TryGetComponent<ObjectCenter>(out objectCenterObj))
            {
                objectCenterObj = obj.AddComponent<ObjectCenter>();
            }

            // Replace obj in front of camera
            Vector3 objPos = obj.transform.position;
            objPos = new Vector3(objPos.x, mainCam.transform.position.y, objPos.z);
            obj.transform.position = objPos;

            Vector3 direction = (obj.transform.position - mainCam.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            obj.transform.rotation = Quaternion.Euler(lookRotation.eulerAngles.x, lookRotation.eulerAngles.y, obj.transform.rotation.eulerAngles.z);

            //Vector3 camPosition = mainCam.transform.localPosition;
            //obj.transform.LookAt(mainCam.transform.position);

            objectCenterObj.Setup(manager: this, giver: this.taskGiver);
            objectRaycaster.SetPlacements(placementObjectPrefabs, objectCenterObj.transform);
        }
        private void OnEnable()
        {
            ARGameEventManager.Instance.RegisterEvent(BaseGameEventManager.StartGameEventName, this, StartGame);
            ARGameEventManager.Instance.RegisterEvent(BaseGameEventManager.EndGameEventName, this, EndGame);
            ObjectFindingEventManager.Instance.RegisterEvent<PlacementObject>(ObjectFindingEventManager.ObjectTouchEventName, this, OnPlacementTouch);
        }

        private void EndGame()
        {
            objectRaycaster.BlockRaycast();
        }

        private void OnPlacementTouch(PlacementObject obj)
        {
            if (!questionIsReady)
            {
                return;
            }

            objectRaycaster.BlockRaycast();
            //Animator anim = obj.GetComponentInChildren<Animator>(true);
            Debug.Log(obj.ID);
            bool IsCorrect = taskGiver.CurrentTask.IsCorrect(obj.ID);
            // if (IsCorrect == true)
            // {
            //     taskGiver.Tasks.UpdateProgress(1);
            // }
            // else
            // {
            //     Debug.Log("Fail");
            // }

            StartCoroutine(VideoStart(answerGIFAnimator, IsCorrect,
                postCallback: () =>
                {
                    objectRaycaster.UnblockRaycast();
                    UpdateProgress(IsCorrect);
                })
            );
        }

        private void UpdateProgress(bool isCorrect)
        {

            if (isCorrect == true)
            {
                taskGiver.Tasks.UpdateProgress(1);
            }
            else
            {
                Debug.Log("Fail");
            }
        }

        private void StartGame()
        {
            if (!questionIsReady)
            {
                TimeCoroutineManager.Instance.WaitUntil(() => questionIsReady, StartGame);
                return;
            }

        }

        private void OnTaskGiverReady()
        {
            // if (taskGiver.CurrentTask is not HouseBuildingTask houseBuildingTask)
            // {
            //     Debug.Log("Game does not have any task to play");
            //     return;
            // }// 2 4 8 9
            questionIsReady = true;
        }

        public void GetModelFromRemote(GameObject[] objs)
        {
            List<PlacementObject> items = new(objs.Length);
            foreach (GameObject obj in objs)
            {
                if (obj.TryGetComponent<PlacementObject>(out PlacementObject result))
                {
                    items.Add(result);
                }
            }

            placementObjectPrefabs = items.ToArray();

            TimeCoroutineManager.Instance.WaitUntil(() => questionIsReady, () =>
            {
                if (IsDebugging)
                {
                    mainGamePlacer.SetPlacedPrefabAndStart(objectCenter.gameObject);
                }
                else
                {
                    mainGamePlacer.SetPlacedPrefab(objectCenter.gameObject);
                }
            });
        }

        public void OnEventRaised<T>(EventSTO sender, T result)
        {
            throw new System.NotImplementedException();
        }

        IEnumerator VideoStart(Animator anim, bool RightAns, Action postCallback)
        {
            anim.gameObject.SetActive(true);
            anim.SetBool("isCorrect", RightAns);
            // if (RightAns == true)
            // {
            //     anim.Play("RightAnswerAnimation");
            // }
            // else
            // {
            //     anim.Play("WrongAnswerAnimation");
            // }
            //anim.transform.LookAt(mainCam.transform);
            var animInfo = anim.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(animInfo.length);

            anim.gameObject.SetActive(false);
            postCallback?.Invoke();
        }
    }
}