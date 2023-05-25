using UnityEngine;

namespace Project.RewardSystem
{
    [RequireComponent(typeof(Canvas))]
    public class ModelRewardPopup : MonoBehaviour
    {
        [SerializeField] private RectTransform spawnPoint;
        private Canvas canvas;
        private RemoteRewardSTO remoteReward;
        private GameObject model;
        public GameObject Test;
        public bool isTesting = false;
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
                var Obj= Instantiate(model, spawnPoint);
                ScaleModelFitCanvas(Obj);
            }
        }

        public async void PopModelReward(RemoteRewardSTO modelReward)
        {
            remoteReward = modelReward;
            model = await remoteReward.GetModel();
            // ScaleModelFitCanvas(Obj);
        }


        private void ScaleModelFitCanvas(GameObject model)
        {
            // Get the Canvas size
            RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
            float canvasWidth = canvasRectTransform.rect.width;
            float canvasHeight = canvasRectTransform.rect.height;

            // Get the model bounds
            Renderer renderer = model.GetComponentInChildren<Renderer>();
            Bounds modelBounds = renderer.bounds;
            Vector3 min = modelBounds.min;
            Vector3 max = modelBounds.max;

            // Convert the model bounds to screen space
            Vector3 minScreen = mainCam.WorldToScreenPoint(min);
            Vector3 maxScreen = mainCam.WorldToScreenPoint(max);
            float modelWidth = maxScreen.x - minScreen.x;
            float modelHeight = maxScreen.y - minScreen.y;

            Debug.Log($"{modelHeight}-{modelWidth}");

            // Calculate the scale factor
            float widthScale = canvasWidth / modelWidth;
            float heightScale = canvasHeight / modelHeight;
            float scaleFactor = Mathf.Min(widthScale, heightScale);

            // Scale the model
            model.transform.localScale *= scaleFactor;
            model.transform.position -= renderer.bounds.center - spawnPoint.position;
            model.transform.rotation = Quaternion.identity;
        }

        private void ScaleAlgo2(GameObject WorldObject)
        {
            //this is your object that you want to have the UI element hovering over

            //this is the ui element
            //RectTransform UI_Element;

            //first you need the RectTransform component of your canvas
            RectTransform CanvasRect = canvas.GetComponent<RectTransform>();

            //then you calculate the position of the UI element
            //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

            Vector2 ViewportPosition = mainCam.WorldToViewportPoint(WorldObject.transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

            //now you can set the position of the ui element
            spawnPoint.anchoredPosition = WorldObject_ScreenPosition;
        }
    }
}