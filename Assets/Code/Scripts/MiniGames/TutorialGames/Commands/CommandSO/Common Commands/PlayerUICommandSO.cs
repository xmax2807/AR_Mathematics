using System;
using System.Collections;
using Project.UI.Panel;
using UnityEngine;

namespace Project.MiniGames.TutorialGames{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Commands/PlayerUICommandSO", fileName = "PlayerUICommand")]
    public class PlayerUICommandSO : CommandSO{
        public enum CommandType{
            Restart, Continue
        }

        [System.Serializable]
        public struct ButtonCommand{
            public string ButtonName;
            public CommandType CommandType;
        }

        [SerializeField] private string Title;
        [SerializeField] private string Description;
        [SerializeField] private ButtonCommand[] buttonCommands;

        private bool IsPlayerClicked = false;
        private MenuPanelViewData cacheData;

        public override ITutorialCommand BuildCommand()
        {
            return new DelegateActionCommand(HandleOnUIShow);
        }

        private MenuPanelViewData BuildUI(ICommander commander){
            MenuPanelViewData data = ScriptableObject.CreateInstance<MenuPanelViewData>();
            
            data.Title = Title;
            data.Description = Description;

            data.ButtonNames = new ButtonData[buttonCommands.Length];
            for(int i = 0; i < buttonCommands.Length; ++i){
                var clickEvent = CreateClickEventFromCommandType(buttonCommands[i].CommandType, commander);
                data.ButtonNames[i] = new ButtonData(){
                    Name = buttonCommands[i].ButtonName,
                    OnClick = clickEvent
                };
            }

            return data;
        }

        private IEnumerator HandleOnUIShow(ICommander commander)
        {
            BasePanelController controller = commander.UIController;
            if(cacheData == null){
                cacheData = BuildUI(commander);
                controller.SetUI(cacheData);
            }
            IsPlayerClicked = false;
            yield return new TaskAwaitInstruction(controller.Show());
            yield return new WaitUntil(() => IsPlayerClicked == true);
        }

        private UnityEngine.UI.Button.ButtonClickedEvent CreateClickEventFromCommandType(CommandType commandType, ICommander commander){
            UnityEngine.UI.Button.ButtonClickedEvent clickEvent = new();
            
            switch(commandType){
                case CommandType.Restart:
                    clickEvent.AddListener(commander.ReturnToCheckpoint);
                    break;
                default:
                    break;
            }

            clickEvent.AddListener(commander.UIController.HideImmediately);
            clickEvent.AddListener(WaitForInput);
            return clickEvent;
        }

        private void WaitForInput()
        {
            IsPlayerClicked = true;
        }
    }
}