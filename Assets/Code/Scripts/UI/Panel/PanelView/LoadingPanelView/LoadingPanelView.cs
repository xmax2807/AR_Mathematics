using Gameframe.GUI.PanelSystem;
using UnityEngine;
using UnityEngine.UI;
using Project.UI.ProgressBar;
using System.Threading.Tasks;
using System.Threading;

namespace Project.UI.Panel{
    public class LoadingPanelView : AnimatedPanelView{
        [SerializeField] protected TMPro.TextMeshProUGUI loadingText;

        public void SetupUI(string loadingTitle){
            loadingText.text = loadingTitle;
        }
    }
}