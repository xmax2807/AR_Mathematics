using Project.Managers;
using Project.QuizSystem;
using Project.Utils;
using UnityEngine;
using UnityEngine.UI;
using Project.Addressable;
using Project.Utils.ExtensionMethods;

namespace Project.MiniGames.FishingGame
{
    [RequireComponent(typeof(Canvas))]
    public class ShapeBoard : MonoBehaviour
    {
        private Canvas canvas;
        [SerializeField] private Image Icon;
        [SerializeField] private StoAddressableRequest request;
        private static Randomizer<ShapePackAsset.ShapePack> Randomizer;

        private async void Awake()
        {
            InitCanvas();
            if (Randomizer == null)
            {
                ScriptableObject[] result = await request.GetResultT();
                if (result.Length == 0 || !result[0].TryCastTo<ShapePackAsset>(out var asset))
                {
                    throw new System.InvalidCastException("Cannot cast ScriptableObject to ShapePackAsset");
                }

                Randomizer = new Randomizer<ShapePackAsset.ShapePack>(asset.Packs);
            }
            UpdateUI();
        }
        private void InitCanvas()
        {
            canvas = this.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }
        public void OnEnable()
        {
            canvas.enabled = true;
        }
        public void OnDisable()
        {
            canvas.enabled = false;
        }
        public void UpdateUI()
        {
            ShapePackAsset.ShapePack pack = Randomizer.Next();
            Icon.sprite = pack.Icon;
        }
        public void ButtonClick()
        {

        }
    }
}