using UnityEngine;
namespace Project.UI.Panel{
    [CreateAssetMenu(fileName ="MenuPanelViewData", menuName ="STO/PanelViewData/MenuPanelViewData")]
    public class MenuPanelViewData : PanelViewData{
        public string Title;
        public string Description;

        public override PanelEnumType Type => PanelEnumType.Menu;
    }
}