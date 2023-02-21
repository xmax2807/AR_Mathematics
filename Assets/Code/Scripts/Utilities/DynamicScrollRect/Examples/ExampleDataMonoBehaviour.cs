using UnityEngine;
using UnityEngine.UI;
using Project.UI;
namespace Project.UI.DynamicScrollRect{
    public class ExampleDataMonoBehaviour : MonoBehaviour, IBaseUI<ExampleData>
    {
        private Image background;
		private Text idText;
		private Text nameEmailText;
		private Text bodyText;
        private Text positionText;

        protected void Awake()
        {
            background = GetComponent<Image>();
            idText = transform.Find("PostId").GetComponent<Text>();
			nameEmailText = transform.Find("NameEmail").GetComponent<Text>();
            bodyText = transform.Find("Body").GetComponent<Text>();
            positionText = transform.Find("Position").GetComponent<Text>();
        }

		public void UpdateUI(ExampleData item)
		{            
            background.gameObject.SetActive(!item.fake);
            idText.gameObject.SetActive(!item.fake);
            nameEmailText.gameObject.SetActive(!item.fake);
            bodyText.gameObject.SetActive(!item.fake);
            positionText.gameObject.SetActive(!item.fake);

            idText.text = item.id.ToString();
			nameEmailText.text = $"{item.name} ({item.email})";
			bodyText.text = item.body;
		}
    }
}