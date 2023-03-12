using Project.Managers;
using Project.QuizSystem;
using Project.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Project.MiniGames.FishingGame{
    [RequireComponent(typeof(Canvas))]
    public class ShapeBoard : MonoBehaviour{
        [System.Serializable]
        public struct ShapePack{
            public Shape.ShapeType type;
            public Sprite icon;
        }

        private Canvas canvas;
        [SerializeField] private Image Icon;
        [SerializeField] private ShapePack[] packs;
        private static Randomizer<ShapePack> Randomizer;

        private void Awake(){
            Randomizer ??= new Randomizer<ShapePack>(packs);

            InitCanvas();
            UpdateUI();
        }
        private void InitCanvas(){
            canvas = this.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }
        public void OnEnable(){
            canvas.enabled = true;
        }
        public void OnDisable(){
            canvas.enabled = false;
        }
        public void UpdateUI(){
            ShapePack pack = Randomizer.Next();
            Icon.sprite = pack.icon;
        }
        public void ButtonClick(){
            
        }
    }
}