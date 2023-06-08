using UnityEngine;

namespace Project.Utils
{
    public class CameraValueLogger : MonoBehaviour
    {
        private static Camera currentCam => Managers.GameManager.MainGameCam;
        private static Transform currentCamTrans => currentCam.transform;
        GUIStyle style;
        public void OnEnable()
        {

            style = new GUIStyle
            {
                fontSize = 24
                
            };
            style.normal.textColor = Color.white;
        }

        void OnGUI()
        {
            GUI.Label(new Rect(50, 60, 300, 80), $"Current Cam Position: {currentCamTrans.position}", style);
            GUI.Label(new Rect(50, 30, 300, 80), $"Current Cam LocalPosition: {currentCamTrans.localPosition}", style);
        }
    }
}