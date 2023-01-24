﻿using System.Collections;
using UnityEngine;

namespace Project.UI.DynamicScrollRect
{
	public class ExampleScroll : MonoBehaviour
	{
		public DynamicScrollRect verticalScroll;
		public DynamicScrollRect horizontalScroll;
		public GameObject referenceObject;

		private ExampleData[] mData;

		private readonly DynamicScroll<ExampleData, ExampleScrollRectItem> mVerticalDynamicScroll = new();
		private readonly DynamicScroll<ExampleData, ExampleScrollRectItem> mHorizontalDynamicScroll = new();

        [System.Obsolete]
        public IEnumerator Start()
		{
			WWW www = new(@"https://jsonplaceholder.typicode.com/comments");
			yield return www;
			mData = JsonHelper.getJsonArray<ExampleData>(www.text);
            // mHorizontalDynamicScroll.spacing = 5f;
            // mHorizontalDynamicScroll.Initiate(horizontalScroll, mData, 0, referenceObject);

            mVerticalDynamicScroll.spacing = 5f;
            mVerticalDynamicScroll.centralizeOnStop = true;
            mVerticalDynamicScroll.Initiate(verticalScroll, mData, 0, referenceObject);
            
            //Debug.Log("Example Horizontal Data : " +  mHorizontalDynamicScroll.RawDataList.Count);
            Debug.Log("Example Vertical Data : " +  mVerticalDynamicScroll.RawDataList.Count);
		}

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) Move(1);
            if (Input.GetKeyDown(KeyCode.Alpha2)) Move(2);
            if (Input.GetKeyDown(KeyCode.Alpha3)) Move(3);
            if (Input.GetKeyDown(KeyCode.Alpha4)) Move(4);
            if (Input.GetKeyDown(KeyCode.Alpha5)) Move(5);
            if (Input.GetKeyDown(KeyCode.Alpha6)) Move(6);
            if (Input.GetKeyDown(KeyCode.Alpha7)) Move(7);
            if (Input.GetKeyDown(KeyCode.Alpha8)) Move(8);
            if (Input.GetKeyDown(KeyCode.Alpha9)) Move(9);

            if (Input.GetKeyDown(KeyCode.Alpha0)) Move(300);
        }

        private void Move(int index)
        {
            mVerticalDynamicScroll.MoveToIndex(index, 2f);
            mHorizontalDynamicScroll.MoveToIndex(index, 2f);
        }
    }
}
