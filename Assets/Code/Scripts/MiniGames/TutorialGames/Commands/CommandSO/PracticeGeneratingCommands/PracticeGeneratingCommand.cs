using System;
using System.Collections;
using Project.UI.GameObjectUI;
using UnityEngine;
namespace Project.MiniGames.TutorialGames{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Commands/PracticeGeneratingCommand", fileName = "PracticeGeneratingCommand")]
    public class PracticeGeneratingCommand : CommandSO
    {
        [SerializeField] private ObjectDataDeliver dataDeliver;
        [SerializeField] private ObjectInteractionEventCenter EventCenter;
        [SerializeField] private PracticeTaskDataSO _practiceTaskData;

        [Header("Mid commands")]
        [SerializeField] private CommandSO betweenTaskCommand;
        [SerializeField] private PlayerAnswerResultTemplateSO playerAnsweredCorrectTemplate;
        [SerializeField] private PlayerAnswerResultTemplateSO playerAnsweredFailTemplate;

        public event Action<int> OnTargetTouchEvent;

        private PracticeTaskGenerator generator;
        private bool IsPlayerAnswered = false;
        public override ITutorialCommand BuildCommand()
        {
            EnsureSubscribeToEventCenter();

            generator ??= new PracticeTaskGenerator(dataDeliver.Names, _practiceTaskData);
            generator.GeneratePracticeTask();

            ITutorialCommand correctAnswerCommand = playerAnsweredCorrectTemplate?.BuildCommand();
            ITutorialCommand failAnswerCommand = playerAnsweredFailTemplate?.BuildCommand();

            SequenceTutorialCommand sequenceCommand = new();
            for(int i = 0; i < _practiceTaskData.Count; ++i){
                //TODO: Create a new command and add it to the sequence
                PracticeTask task = generator[i];

                SpeakCommand speakCommand = new(task.GetTaskQuestion());
                PlayerPracticeAnswerCommand playerAnswerCommand = new(HandlePlayerAnswer, task, correctAnswerCommand, failAnswerCommand);

                SequenceTutorialCommand subCommand = new();
                subCommand.AddNextCommand(speakCommand);
                subCommand.AddNextCommand(playerAnswerCommand);

                if(i < _practiceTaskData.Count - 1){

                    subCommand.AddNextCommand(betweenTaskCommand?.BuildCommand());
                }

                sequenceCommand.AddNextCommand(subCommand);
            }

            return sequenceCommand;
        }

        IEnumerator HandlePlayerAnswer(PlayerPracticeAnswerCommand command)
        {
            InteractionEventsBehaviour.Instance.UnblockRaycast();
            int playerChooseIndex = -1;
            IsPlayerAnswered = false;

            void clickEvent(int index) => playerChooseIndex = index;
            OnTargetTouchEvent += clickEvent;
            
            yield return new WaitUntil(() => playerChooseIndex != -1);
            InteractionEventsBehaviour.Instance.BlockRaycast();
            
            OnTargetTouchEvent -= clickEvent;
            command.SetAnswer(playerChooseIndex);
        }

        private void EnsureSubscribeToEventCenter(){
            EventCenter.Unsubscribe(dataDeliver, OnTargetTouch);
            EventCenter.Subscribe(dataDeliver, OnTargetTouch);
        }

        void OnEnable(){
            //Subscribe to event
            EventCenter.Subscribe(dataDeliver, OnTargetTouch);
            Debug.Log("Subscribe to event");
        }

        private void OnTargetTouch(int index)
        {
            OnTargetTouchEvent?.Invoke(index);
            IsPlayerAnswered = true;
            Debug.Log("Invoked Event");
        }

        void OnDisable(){
            EventCenter.Unsubscribe(dataDeliver, OnTargetTouch);
            Debug.Log("Unsubscribe from event");
        }
    }
}