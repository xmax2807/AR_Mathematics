using System;
using Project.UI.Panel;
using UnityEngine;
using UnityEngine.UI;

namespace Project.MiniGames.TutorialGames
{
    public class TutorialBehaviour : MonoBehaviour, ITutorial
    {
        [SerializeField] private ContextGroup contexts;
        [SerializeField] private BasePanelController menuPanelController;
        [SerializeField] private PracticeTaskUIController practiceTaskUIController;

        [Header("AR Main plane")]
        [SerializeField] private bool isDebug;
        [SerializeField] private GameObject planePrefab;
        [SerializeField] private MainPlaneHolder mainPlaneHolder;
        [SerializeField] private Project.ARBehaviours.PlanePlacer planePlacer;

        [SerializeField] Button backButton;

        private ITutorial _tutorial;
        private Commander _commander;
        private ITutorial Tutorial
        {
            get
            {
                _tutorial ??= SettingUp();
                return _tutorial;
            }
        }

        private ITutorial SettingUp()
        {
            _commander = new Commander(this, menuPanelController, practiceTaskUIController);
            _commander.OnStageEnded += NextStage;
            _commander.OnTutorialEnded += OnTutorialHasEnded;
            ITutorial result = new Tutorial(contexts, _commander);
            return result;
        }

        private void Awake()
        {
            _tutorial = SettingUp();
            _ = menuPanelController.Hide();
            _ = practiceTaskUIController.HideAsync();
        }

        public void OnEnable()
        {
            planePlacer.OnSpawnMainPlane += StartTutorial;
        }
        public void OnDisable()
        {
            planePlacer.OnSpawnMainPlane -= StartTutorial;
        }

        public void Start()
        {
            if (isDebug)
            {
                planePlacer.SetPlacedPrefabAndStart(planePrefab);
                planePlacer.TurnOffPlaneDetector();
                return;
            }
            planePlacer.TurnOffPlaneDetector();
            planePlacer.SetPrefab(planePrefab);
            planePlacer.TurnOnPlaneDetector();
        }

        private void StartTutorial(GameObject mainPlane)
        {
            planePlacer.TurnOffPlaneDetector();
            mainPlaneHolder.SetMainPlaneObj(mainPlane);
            Begin();
        }

        public void Begin()
        {
            Tutorial.Begin();
        }

        public void End()
        {
            Tutorial.End();
        }

        public void NextStage()
        {
            Tutorial.NextStage();
        }

        private void OnTutorialHasEnded(){
            UnityEngine.UI.Button.ButtonClickedEvent ButtonClick = new();
            ButtonClick.AddListener(backButton.onClick.Invoke);
            MenuPanelViewData data = new()
            {
                Title = "Kết thúc lớp học AR",
                Description = "Cảm ơn em đã tham gia buổi học, chúc em học tập thật vui nhé",
                ButtonNames = new ButtonData[1]
                {
                    new ButtonData{
                        Name = "Trở về",
                        OnClick = ButtonClick,
                    }
                }
            };
            menuPanelController.SetUI(data);
            _ = menuPanelController.Show();
            Managers.AudioManager.Instance.Speak(data.Description);
        }
    }
}