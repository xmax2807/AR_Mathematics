using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.DynamicScrollRect
{
	public class ExampleScrollRectItem : ScrollRectItem<ExampleData>
	{
		public override string Name => "ExampleObject";

        private Image background;
		private Text idText;
		private Text nameEmailText;
		private Text bodyText;
        private Text positionText;

        public void Awake()
        {
            background = GetComponent<Image>();
            idText = transform.Find("PostId").GetComponent<Text>();
			nameEmailText = transform.Find("NameEmail").GetComponent<Text>();
            bodyText = transform.Find("Body").GetComponent<Text>();
            positionText = transform.Find("Position").GetComponent<Text>();
        }

		public override void UpdateScrollItem(ExampleData item, int index)
		{
			base.UpdateScrollItem(item, index);
            
            background.gameObject.SetActive(!item.fake);
            idText.gameObject.SetActive(!item.fake);
            nameEmailText.gameObject.SetActive(!item.fake);
            bodyText.gameObject.SetActive(!item.fake);
            positionText.gameObject.SetActive(!item.fake);

            idText.text = item.id.ToString();
			nameEmailText.text = $"{item.name} ({item.email})";
			bodyText.text = item.body;
		}

        public override void SetPositionInViewport(Vector2 position, Vector2 distanceFromCenter)
        {
            base.SetPositionInViewport(position, distanceFromCenter);
            positionText.text = position.ToString();

            if (IsCentralized)
                background.color = Color.white;
            else
                background.color = Color.Lerp(Color.green, Color.red, Mathf.Clamp01(Mathf.Max(distanceFromCenter.x, distanceFromCenter.y) / 1000f));
        }
    }
}