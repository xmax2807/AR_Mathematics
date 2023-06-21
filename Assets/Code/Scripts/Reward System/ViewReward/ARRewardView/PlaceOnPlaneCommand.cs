using UnityEngine;
using Project.ARBehaviours;
using Project.Pattern.Command;
namespace Project.RewardSystem.ViewReward{
    public class ActivatePlaneDetectionCommand : IUndoableCommand
    {
        private IPlanePlacer m_planePlacer;
        public ActivatePlaneDetectionCommand(IPlanePlacer placer, GameObject prefab){
            this.m_planePlacer = placer;
            placer.SetPrefab(prefab);
        }
        public void Execute()
        {
            m_planePlacer.TurnOnPlaneDetector();
        }

        public void Undo()
        {
            m_planePlacer.TurnOffPlaneDetector();
        }
    }
}