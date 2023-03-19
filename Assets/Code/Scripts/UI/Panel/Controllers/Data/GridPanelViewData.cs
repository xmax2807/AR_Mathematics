using UnityEngine;
namespace Project.UI.Panel{
    [CreateAssetMenu(fileName ="GridViewPanelData", menuName ="STO/PanelViewData/GridViewPanelData")]
    public class GridPanelViewData : PanelViewData{
        public string Title;
        public string Description;

        public override PanelEnumType Type => PanelEnumType.Grid;
    }
}