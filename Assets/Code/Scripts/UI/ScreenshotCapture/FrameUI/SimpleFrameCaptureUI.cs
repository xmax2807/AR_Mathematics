using System.Collections;
using UnityEngine;
namespace Project.UI.Screenshot
{
    public class SimpleFrameCaptureUI : MonoBehaviour{
        [SerializeField] private RectTransform targetTransform;
        public IFrame GetFrame(){
            return new FixedSizeFrame(targetTransform);
        }

        public IEnumerator ShowUI(System.Action<IFrame> onAdjustedFrame){
            onAdjustedFrame?.Invoke(GetFrame());
            yield break;
        }
    }
}