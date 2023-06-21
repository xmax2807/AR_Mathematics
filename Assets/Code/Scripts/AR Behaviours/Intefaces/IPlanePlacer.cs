namespace Project.ARBehaviours{
    public interface IPlanePlacer {
        public void TurnOnPlaneDetector();
        public void TurnOffPlaneDetector();
        public void SetPrefab(UnityEngine.GameObject prefab);
    }
}