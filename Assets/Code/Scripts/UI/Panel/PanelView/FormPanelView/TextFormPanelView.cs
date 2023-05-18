using UnityEngine;
using TMPro;
namespace Project.UI.Panel.Form{
    public class TextFormPanelView : FormPanelView{
        [SerializeField] private TMP_InputField[] textFields;
        public override string[] GetFieldDatas(){
            string[] texts = new string[textFields.Length];
            for(int i = 0; i < texts.Length;++i){
                texts[i] = textFields[i].text;
            }
            return texts;
        }
    }
}