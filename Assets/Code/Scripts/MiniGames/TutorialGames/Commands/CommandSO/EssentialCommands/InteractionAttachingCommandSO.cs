using System;
using Project.UI.GameObjectUI;
using UnityEngine;

namespace Project.MiniGames.TutorialGames
{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Commands/InteractionAttachingCommand", fileName = "InteractionAttachingCommand")]
    public class InteractionAttachingCommandSO : CommandSO{
        [SerializeField] private ObjectDataDeliver dataDeliver;
        [SerializeField] private ObjectInteractionEventCenter EventCenter;
        public override ITutorialCommand BuildCommand(){
            InteractionAttachingCommand[] listCommands = new InteractionAttachingCommand[dataDeliver.Objects.Length];
            for(int i = 0; i < dataDeliver.Objects.Length; ++i){
                listCommands[i] = new InteractionAttachingCommand(dataDeliver.Objects[i]);
                listCommands[i].OnTargetTouch += (obj) => this.HandleTouch(obj, i);
            }
            return new CompositeCommand(listCommands);
        }

        private void HandleTouch(GameObjectButton button, int index)
        {
            Debug.Log(button.UniqueID);
            EventCenter.InvokeEvent(dataDeliver, index);
        }
    }
}