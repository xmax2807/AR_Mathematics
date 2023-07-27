using System.Threading.Tasks;
using UnityEngine;
namespace Project.UI.Panel{
    public class PanelControllerSpawner : BasePanelController
    {
        [SerializeField] private BasePanelController wrappeePrefab;
        private BasePanelController spawnedWrappee;
        public override PanelEnumType Type => wrappeePrefab.Type;

        public override bool CheckType(PanelViewData data)
        {
            return Type == data.Type;
        }

        public override void SetUI(PanelViewData Data)
        {
            if(spawnedWrappee == null){
                spawnedWrappee = Instantiate(wrappeePrefab, this.transform, false);
                spawnedWrappee.gameObject.SetActive(true);
            }
            spawnedWrappee.SetUI(Data);
        }

        public override Task Show()
        {
            if(spawnedWrappee!= null){
                return spawnedWrappee.Show();
            }
            return Task.CompletedTask;
        }

        public override async Task Hide()
        {
            if(spawnedWrappee == null){
                return;
            }
            await spawnedWrappee.Hide();
            Destroy(spawnedWrappee.gameObject);
            spawnedWrappee = null;
        }
    }
}