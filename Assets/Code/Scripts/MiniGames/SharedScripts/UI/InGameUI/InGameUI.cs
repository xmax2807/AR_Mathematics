using UnityEngine;
using Project.UI;
using System.Collections.Generic;
using System;

namespace Project.MiniGames.UI{
    public class InGameUI : MonoBehaviour{
        [SerializeField] TaskGiver giver;
        [SerializeField] GameObject[] ListProgressUI;
        [SerializeField] private ReachGoal ProgressBar;
        private IProgressUI[] realProgressUIs;
        private void Awake(){
            
            giver.OnTaskChanged += OnTaskChanged;

            List<IProgressUI> listProgressUIs = new(ListProgressUI.Length);

            // for(int i = 0; i < ListProgressUI.Length; ++i){
            //     IProgressUI ui = 
            //     if(){

            //         listProgressUIs.Add(realUI);
            //     }
            // }

            listProgressUIs.Add(ProgressBar);
            realProgressUIs = listProgressUIs.ToArray();
        }

        private void OnTaskChanged(BaseTask task){
            task.OnTaskCompleted += UpdateUI;
        }
        private void UpdateUI()
        {
            foreach(IProgressUI progressUI in realProgressUIs){
                progressUI.SetProgress(((float)giver.Tasks.CurrentProgress) / giver.Tasks.Goal);
            }
        }
    } 
}