using UnityEngine;
using Project.Utils.ExtensionMethods;
using System;

public class Calendar : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI dateText;
    [SerializeField] private TMPro.TextMeshProUGUI daysOfWeek;
    [SerializeField] private TMPro.TextMeshProUGUI month;

    readonly static string[] daysList = new string[7]{
        "Chủ nhật",
        "Thứ ba",
        "Thứ năm",
        "Thứ hai",
        "Thứ bảy",
        "Thứ tư",
        "Thứ sáu",
    };
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
