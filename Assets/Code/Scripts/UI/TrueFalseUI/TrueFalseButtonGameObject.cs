using UnityEngine;
using Project.UI.GameObjectUI;
namespace Project.UI.TrueFalseUI{
    [RequireComponent(typeof(GameObjectButton), typeof(SpriteRenderer))]
    public class TrueFalseButtonGameObject : BaseTrueFalseUI<Color>, ITouchableObject
    {
        private SpriteRenderer spriteRenderer;
        private GameObjectButton button;

        public event System.Action<ITouchableObject> OnButtonTouch;

        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = DefaultUI;

            button = GetComponent<GameObjectButton>();
            button.OnButtonTouch += OnButtonTouch;
        }

        public string UniqueID => button.UniqueID;


        public bool Equals(ITouchableObject other)
        {
            return button.Equals(other);
        }

        public void OnTouch(Touch touch)
        {
            button.OnTouch(touch);
        }

        public override void Reset()
        {
            spriteRenderer.color = DefaultUI;
        }
        public override void ChangeUI(bool isTrue)
        {
            spriteRenderer.color = isTrue ? TrueUI : FalseUI;
        }
    }
}