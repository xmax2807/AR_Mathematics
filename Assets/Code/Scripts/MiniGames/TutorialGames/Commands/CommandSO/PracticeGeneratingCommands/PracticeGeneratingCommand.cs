using System;
using UnityEngine;
namespace Project.MiniGames.TutorialGames{
    public class PracticeGeneratingCommand : CommandSO
    {
        [SerializeField] private ObjectDataDeliver dataDeliver;
        [SerializeField] private ObjectInteractionEventCenter EventCenter;
        [SerializeField] private PracticeTaskDataSO _practiceTaskData;
        public override ITutorialCommand BuildCommand()
        {
            SequenceTutorialCommand sequenceCommand = new SequenceTutorialCommand();
            for(int i = 0; i < _practiceTaskData.Count; ++i){
                //TODO: Create a new command and add it to the sequence
            }

            return sequenceCommand;
        }
        void OnEnable(){
            //Subscribe to event
            EventCenter.Subscribe(dataDeliver, OnTargetTouch);
        }

        private void OnTargetTouch(int index)
        {
            
        }

        void OnDisable(){
            EventCenter.Unsubscribe(dataDeliver, OnTargetTouch);
        }
    }
}