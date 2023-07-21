using System.Collections;
using UnityEngine;
using Project.UI.GameObjectUI;
using Project.Utils.ExtensionMethods;

namespace Project.MiniGames.TutorialGames
{
    public class InteractionAttachingCommand : ITutorialCommand
    {
        private GameObject m_target;
        private GameObjectButton m_goButton;

        public event System.Action<GameObjectButton> OnTargetTouch;

        public InteractionAttachingCommand(GameObject target)
        {
            m_target = target;
        }
        public IEnumerator Execute(ICommander commander)
        {
            m_goButton = m_target.AddComponent<GameObjectButton>();
            
            if(m_target.GetComponent<BoxCollider>() == null){
                BoxCollider collder = m_target.AddComponent<BoxCollider>();
                collder.size = m_target.GetSizeFromRenderer();
                collder.isTrigger = true;
            }

            m_goButton.OnButtonTouch += this.OnTargetTouch;
            yield break;
        }
    }
}