using UnityEngine;
using Project.Utils.ExtensionMethods;
using System.Collections.Generic;

namespace Project.UI.DynamicScrollRect
{
	public class ExampleScrollRectItem : ScrollRectComplexItem<ExampleData>
	{
		public override string Name => "ExampleObject";
        [SerializeField] private ExampleDataMonoBehaviour UnitItemSample;
        ExampleDataMonoBehaviour[] list;
        public override void InitScrollRectItem(){
            FlexibleGridLayout gridLayout = this.gameObject.AddComponent<FlexibleGridLayout>();
            gridLayout.SetMaxColumn(10);
            //gridLayout.AutoCellWidth = false;
            list = new ExampleDataMonoBehaviour[maxItemInRow];
            for(int i = 0; i < maxItemInRow; i++){
               list[i] = Instantiate(UnitItemSample, this.transform);
            }
            Rect.StretchWidth(RectTransformExtensionMethods.RectPositionType.Top);
        }
		protected override void UpdateUnitItem(ExampleData item, int index)
		{
            list[index].UpdateUI(item);
		}
    }
}