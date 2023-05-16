using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Panel{
    [System.Serializable]
    public class ButtonData{
        public string Name;
        public Button.ButtonClickedEvent OnClick;
        public string Description;
    }

    public class ImageButtonData : ButtonData{
        public Sprite Image;

        public virtual Sprite GetImage()=>Image;
    }

    public class UnlockableImageButtonData : ImageButtonData{
        public Sprite LockedImage;
        public bool isUnlocked;

        public override Sprite GetImage()
        {
            if(isUnlocked) return base.GetImage();
            return LockedImage;
        }
    }
}