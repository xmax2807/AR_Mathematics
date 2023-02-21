using UnityEngine;

namespace Project.Utils
{
    public class DebugLog : MonoBehaviour
    {
        //#if !UNITY_EDITOR
        static string myLog = "";
        private string output;
        private string stack;
        [SerializeField] private float size = 200;
        GUIStyle style;

        void OnEnable()
        {
            style = new GUIStyle();
            style.fontSize = 56;
            Application.logMessageReceived += Log;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= Log;
            myLog = "";
        }

        public void Log(string logString, string stackTrace, LogType type)
        {
            output = logString;
            stack = stackTrace;
            myLog = output + $" - {Time.time} - {stack}" + "\n" + myLog;
            if (myLog.Length > 5000)
            {
                myLog = myLog.Substring(0, 4800);
            }
        }

        void OnGUI()
        {
            //if (!Application.isEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
            {
                myLog = GUI.TextArea(new Rect(Screen.width - size, Screen.height - size, size, size), myLog);
            }
        }
        //#endif
    }
}