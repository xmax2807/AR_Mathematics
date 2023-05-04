using Project.Managers;
using UnityEngine;

namespace Project.UI.Panel{
    public class RequestWrapperPanelController : BasePanelController<RequestPanelViewData>
    {
        [SerializeField] private Canvas loadingCanvas;
        [SerializeField] private Canvas noDataFoundCanvas;
        [SerializeField] private BasePanelController wrappedController;
        public override PanelEnumType Type => PanelEnumType.Request;
        protected override void OnEnable()
        {
            base.OnEnable();
            loadingCanvas.enabled = false;
            noDataFoundCanvas.enabled = false;
        }
        public override async void SetUI(RequestPanelViewData Data)
        {
            loadingCanvas.enabled = true;
            bool result = await Data.RequestData();
            loadingCanvas.enabled = false;
            
            if(result == false) {
                noDataFoundCanvas.enabled = true;
                return;
            }
            
            wrappedController.SetUI(Data.RealPanelViewData);
            await wrappedController.Show();
        }

        public override bool CheckType(PanelViewData data)
        {
            if(data is not RequestPanelViewData reqData){
                return false;
            }
            return reqData.Type == this.Type && reqData.RealPanelViewData.Type == wrappedController.Type; 
        }
    }
}