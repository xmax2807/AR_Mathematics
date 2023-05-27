using UnityEngine;
namespace Project.MiniGames.HouseBuilding{
    public class MainPlane : MonoBehaviour{
        [SerializeField] private Transform buildingParent;
        public Merge First {get;set;}
        private BuildingManager manager;
        private void Awake(){
            if(buildingParent == null){
                var newGameObj = new GameObject("BuildingParent");
                buildingParent = newGameObj.transform;
            }
        }
        public void AddFirstBuilding(Merge first, BuildingManager manager){
            foreach(Transform child in buildingParent){
                Destroy(child.gameObject);
            }

            First = first;
            Merge obj = Instantiate(First, buildingParent);
            obj.transform.localPosition = Vector3.zero;
            obj.Manager = manager;
            this.manager = manager;
        }
    } 
}