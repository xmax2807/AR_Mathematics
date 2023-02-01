using System.Collections;
using UnityEngine;
using Project.Utils.ExtensionMethods;

namespace Project.UI.DynamicScrollRect
{
	public class ExampleScroll : MonoBehaviour
	{
		[SerializeField] private DynamicScrollRect verticalScroll;
		[SerializeField] private ExampleScrollRectItem referenceObject;
        [SerializeField] private GameObject unitItem;
		private ExampleData[][] mData;
        [SerializeField] private RectTransform contentScrollview;
        [SerializeField] private bool disableScrollView = true;
        [SerializeField] private int populateNumber = 1000;

		private DynamicScroll<ExampleData[], ExampleScrollRectItem> mVerticalDynamicScroll;

        [System.Obsolete]
        public void Start()
		{
			// WWW www = new(@"https://jsonplaceholder.typicode.com/comments");
			// yield return www;
			// mData = JsonHelper.getJsonArray<ExampleData>(www.text);
            Project.Managers.TimeCoroutineManager.Instance.WaitFor(new WaitForEndOfFrame(),FetchData);


            // if(!disableScrollView){

            // for(int i = 0; i < populateNumber; i++){
            //     var obj = Instantiate(unitItem);
            //     //obj.AddComponent<ExampleScrollRectItem>().UpdateScrollItem(mData[i],i);
            //     obj.transform.SetParent(contentScrollview);
            //     //reset the item's scale -- this can get munged with UI prefabs
            //     obj.transform.localScale = Vector2.one;
            // }
            
            // }
		}

        private void FetchData(){
            int col = 10;
            int row = populateNumber / col + 1;
             mData = new ExampleData[row][];
            for(int i = 0; i < mData.Length; i++){
                var realCol = Mathf.Min(populateNumber - i* col, col);  
                mData[i] = new ExampleData[realCol];
                for(int j = 0; j < realCol; j++){
                    mData[i][j] = new ExampleData(i*col + j,false);
                }
            }

            mVerticalDynamicScroll = new()
            {
                spacing = 5f,
                centralizeOnStop = true,
                listPool = new(0, referenceObject, verticalScroll.content)
                {
                    createMoreIfNeeded = true
                }
            };
            mVerticalDynamicScroll.listPool.OnObjectCreationCallBack += (ExampleScrollRectItem item, int index)=>{
                item.maxItemInRow = mData[index].Length;
                item.IsHorizontal = verticalScroll.vertical;
            };
            mVerticalDynamicScroll.Initiate(verticalScroll, mData, 0, referenceObject);
        }

        // public void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.Alpha1)) Move(1);
        //     if (Input.GetKeyDown(KeyCode.Alpha2)) Move(2);
        //     if (Input.GetKeyDown(KeyCode.Alpha3)) Move(3);
        //     if (Input.GetKeyDown(KeyCode.Alpha4)) Move(4);
        //     if (Input.GetKeyDown(KeyCode.Alpha5)) Move(5);
        //     if (Input.GetKeyDown(KeyCode.Alpha6)) Move(6);
        //     if (Input.GetKeyDown(KeyCode.Alpha7)) Move(7);
        //     if (Input.GetKeyDown(KeyCode.Alpha8)) Move(8);
        //     if (Input.GetKeyDown(KeyCode.Alpha9)) Move(9);

        //     if (Input.GetKeyDown(KeyCode.Alpha0)) Move(300);
        // }

        // private void Move(int index)
        // {
        //     mVerticalDynamicScroll.MoveToIndex(index, 2f);
        //     mHorizontalDynamicScroll.MoveToIndex(index, 2f);
        // }
    }
}
