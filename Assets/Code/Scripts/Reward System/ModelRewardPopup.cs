using UnityEngine;

namespace Project.RewardSystem
{
    [RequireComponent(typeof(Canvas))]
    public class ModelRewardPopup : MonoBehaviour
    {
        [Header("Test fields")]
        public GameObject Test;
        public bool isTesting = false;

        [Header("Requires")]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Camera ModelViewerPrefab;
        [SerializeField] private TMPro.TextMeshProUGUI DescriptionTextUI;
        [SerializeField] private Gameframe.GUI.PanelSystem.AnimatedPanelView view;


        private Canvas canvas;
        private RemoteRewardSTO remoteReward;
        private GameObject model;
        private GameObject spawnObject;
        private Camera modelViewer;
        private Camera mainCam;
        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            mainCam = Camera.main;
        }

        void Start()
        {

            if (isTesting)
            {
                model = Test;
                modelViewer = Instantiate(ModelViewerPrefab);
                spawnObject = Instantiate(model, modelViewer.transform);
                spawnObject.AddComponent<RotatingFingerSwipe>();
                ScaleModelFitCanvas(spawnObject);

                DescriptionTextUI.text = "Testing";
                view?.ShowAsync();
            }
            else
            {
                view?.HideAsync();
            }
        }

        public async void PopModelReward(RemoteRewardSTO modelReward)
        {
            Debug.Log("Got model");
            remoteReward = modelReward;
            model = await remoteReward.GetModel();
            modelViewer = Instantiate(ModelViewerPrefab);
            spawnObject = Instantiate(model, modelViewer.transform);
            spawnObject.AddComponent<RotatingFingerSwipe>();
            //MoveCamToFit();
            // ScaleModelFitCanvas(spawnObject);
            ScaleAlgo2(spawnObject);

            DescriptionTextUI.text = modelReward.Description;
        }
        public async void Hide()
        {
            await view?.HideAsync();
            remoteReward?.UnloadAsset();
            Destroy(spawnObject);
            Destroy(modelViewer);
        }

        public void Show(){
            view?.ShowAsync();
        }


        private void ScaleModelFitCanvas(GameObject model)
        {
            // Get the Canvas size
            // RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();

            // float canvasWidth = canvasRectTransform.rect.width;
            // float canvasHeight = canvasRectTransform.rect.height;

            float canvasWidth = modelViewer.targetTexture.width;
            float canvasHeight = modelViewer.targetTexture.height;

            // Get the model bounds
            Renderer renderer = model.GetComponentInChildren<Renderer>();
            Bounds modelBounds = renderer.bounds;
            Vector3 min = modelBounds.min;
            Vector3 max = modelBounds.max;

            // Convert the model bounds to screen space
            Vector3 minScreen = modelViewer.WorldToScreenPoint(min);
            Vector3 maxScreen = modelViewer.WorldToScreenPoint(max);
            float modelWidth = maxScreen.x - minScreen.x;
            float modelHeight = maxScreen.y - minScreen.y;

            // Calculate the scale factor
            float widthScale = canvasWidth / modelWidth;
            float heightScale = canvasHeight / modelHeight;
            float scaleFactor = Mathf.Min(widthScale, heightScale);

            Debug.Log($"{modelWidth} - {modelHeight}");
            // Change the model transform
            model.transform.localScale *= Mathf.Abs(scaleFactor);
            model.transform.position -= renderer.bounds.center - modelViewer.transform.position;
            model.transform.localPosition += new Vector3(0, 0, modelViewer.fieldOfView / 2);
            //model.transform.rotation = Quaternion.identity;
        }
        void ScaleAlgo2(GameObject model)
        {
            Renderer renderer = model.GetComponentInChildren<Renderer>();
            Bounds modelBounds = renderer.bounds;

            model.transform.localPosition += new Vector3(0, 0, modelViewer.fieldOfView);

            float distance = Vector3.Distance(modelViewer.transform.position, model.transform.position);
            float cameraHeight = 2.0f * distance * Mathf.Tan(0.5f * modelViewer.fieldOfView * Mathf.Deg2Rad);
            
            // Get the size of the model's bounding box
            Vector3 objectSizes = modelBounds.max - modelBounds.min;
            float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);

            // Calculate the desired scale based on the size of the camera's viewport
            float desiredScale = cameraHeight / objectSize;

            // Scale the model to fit within the camera's viewport
            model.transform.localScale *= Mathf.Abs(desiredScale);
            model.transform.position -= renderer.bounds.center - modelViewer.transform.position;
            model.transform.localPosition += new Vector3(0, 0, modelViewer.fieldOfView);
        }
    }
}