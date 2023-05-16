using UnityEngine;
using Project.Utils.ExtensionMethods;
using System;

public class Calendar : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI dateText;
    [SerializeField] private TMPro.TextMeshProUGUI daysOfWeek;
    [SerializeField] private TMPro.TextMeshProUGUI month;

    string[] daysList;
    // Start is called before the first frame update
    void Start()
    {
        //create a list of days of the week
        daysList = new string[7];
        daysList[0] = "Chủ nhật";
        daysList[1] = "Thứ hai";
        daysList[2] = "Thứ ba";
        daysList[3] = "Thứ tư";
        daysList[4] = "Thứ năm";
        daysList[5] = "Thứ sáu";
        daysList[6] = "Thứ bảy";

    }

    // Update is called once per frame
    // void Update()
    // {
    //     //if the button is clicked and released, choose a random date from the lists
    //     if (Input.GetMouseButtonDown(0))
    //     {

    // 	}
    // }

    public void ChangeDateRandomly()
    {
        DateTime randomDate = DateTimeExtensionMethods.RandomDayWithinAYear();
        SetDate(randomDate);
    }

    public void SetDate(DateTime dateTime){
        dateText.text = dateTime.Day.ToString();

        int dayOfWeek = (int)dateTime.DayOfWeek;
        daysOfWeek.text = daysList[dayOfWeek];
        month.text = "Tháng " + dateTime.Month.ToString();
    }
}
