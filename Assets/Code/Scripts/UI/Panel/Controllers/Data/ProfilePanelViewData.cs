using UnityEngine;
namespace Project.UI.Panel{
    [CreateAssetMenu(fileName ="ProfilePanelViewData", menuName ="STO/PanelViewData/ProfilePanelViewData")]
    public class ProfilePanelViewData : PanelViewData{
        public string Title;
        public string Description;

        public override PanelEnumType Type => PanelEnumType.Profile;
    }
}