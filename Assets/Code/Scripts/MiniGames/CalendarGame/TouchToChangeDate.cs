using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Project.Utils.ExtensionMethods;
using System;
using Assets.Code.Scripts.Utilities.ExtensionMethods;

public class TouchToChangeDate : MonoBehaviour
{
    //create a public variable to store the date text
    [SerializeField]
    public TMPro.TextMeshProUGUI dateText;
    public TMPro.TextMeshProUGUI daysOfWeek;
    public TMPro.TextMeshProUGUI month;
    public Button button;
	List<string> daysList = new List<string>();
	// Start is called before the first frame update
	void Start()
    {
        //create a list of days of the week
        
        daysList.Add("Chủ nhật");
        daysList.Add("Thứ hai");
        daysList.Add("Thứ ba");
        daysList.Add("Thứ tư");
        daysList.Add("Thứ năm");
        daysList.Add("Thứ sáu");
        daysList.Add("Thứ bảy");

    }

    // Update is called once per frame
    void Update()
    {
        //if the button is clicked and released, choose a random date from the lists
        if (Input.GetMouseButtonDown(0))
        {
            DateTime randomDate = DateTimeExtensionMethods.RandomDayWithinAYear();
            dateText.text = randomDate.Day.ToString();

            int dayOfWeek = (int)randomDate.DayOfWeek;
            daysOfWeek.text = daysList[dayOfWeek];
            month.text = "Tháng " + randomDate.Month.ToString();
		}
    }
}
