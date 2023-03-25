using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchToChangeDate : MonoBehaviour
{
    //create a public variable to store the date text
    [SerializeField]
    public TextMesh dateText;
    public TextMesh daysOfWeek;
    public Button button;
	List<int> dateList = new List<int>();
	List<string> daysList = new List<string>();
	// Start is called before the first frame update
	void Start()
    {
        //create a list of date from 1 to 31
        for (int i = 1; i <= 31; i++)
        {
			dateList.Add(i);
		}
        //create a list of days of the week
        
        daysList.Add("Thứ hai");
        daysList.Add("Thứ ba");
        daysList.Add("Thứ tư");
        daysList.Add("Thứ năm");
        daysList.Add("Thứ sáu");
        daysList.Add("Thứ bảy");
        daysList.Add("Chủ nhật");

    }

    // Update is called once per frame
    void Update()
    {
        //if the button is clicked and released, choose a random date from the lists
        if (Input.GetMouseButtonDown(0))
        {
			int randomDate = Random.Range(0, dateList.Count);
			int randomDay = Random.Range(0, daysList.Count);
			dateText.text = dateList[randomDate].ToString();
			daysOfWeek.text = daysList[randomDay].ToString();
		}
    }
}
