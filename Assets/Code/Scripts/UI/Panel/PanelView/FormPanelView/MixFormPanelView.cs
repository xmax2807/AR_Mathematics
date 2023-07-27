using System.Collections.Generic;

namespace Project.UI.Panel.Form{
    public class MixFormPanelView : FormPanelView
    {
        [UnityEngine.SerializeField] private FormPanelView[] children;
        public override string[] GetFieldDatas()
        {
            List<string> result = new();
            foreach(FormPanelView child in children){
                result.AddRange(child.GetFieldDatas());
            }
            return result.ToArray();
        }
    }
}