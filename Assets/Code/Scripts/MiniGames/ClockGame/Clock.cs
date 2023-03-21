using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Managers;

public class Clock : MonoBehaviour
{
    [SerializeField]
    Transform hoursHand;
    [SerializeField]
    Transform minutesHand;
    [SerializeField]
    Transform secondsHand;
    // Start is called before the first frame update
    void Start(){
        //init time
        System.DateTime time = System.DateTime.Now;
        SetHour(time);
        SetMinute(time);
        SetSecond(time);
        TimeCoroutineManager.Instance.DoLoopAction(()=>SetSecond(System.DateTime.Now), ()=>false, 1);
    }
    public void SetHour(System.DateTime value)
    {
        //set the rotation of the hours hand
        float hoursRotation = -(value.Hour / 12f) * 360f;
        hoursHand.localRotation = Quaternion.Euler(new Vector3(hoursRotation - 90, 0, 0));
	}
    public void SetMinute(System.DateTime value){
        //set the rotation of the minutes hand
        float minutesRotation = -(value.Minute / 60f) * 360f;
        minutesHand.localRotation = Quaternion.Euler(new Vector3(minutesRotation - 90,0, 0));
        
        if(minutesRotation == 0) SetHour(value);
    }
    public void SetSecond(System.DateTime value){
        float secondsRotation = -(value.Second / 60f) * 360f;
        secondsHand.localRotation = Quaternion.Euler(new Vector3(secondsRotation - 90, 0, 0));
        
        if(secondsRotation == 0){
            SetMinute(value);
        }
    }
    public void IncreaseSecond(){
        secondsHand.localRotation = Quaternion.Euler(new Vector3(360f/60f - 90, 0, 0));
    }

    //Update is called once per frame
    // void Update()
    // {
    //     SetSecond(System.DateTime.Now);

    //     // //create datetime
    //     // System.DateTime time = System.DateTime.Now;
    //     // //set the rotation of the hours hand
    //     // // float hoursRotation = -(hourVal / 12f) * 360f;
    //     // // hoursHand.localRotation = Quaternion.Euler(new Vector3(hoursRotation - 90, 0, 0));
    //     // //set the rotation of the minutes hand
    //     // float minutesRotation = -(time.Minute / 60f) * 360f;
    //     // minutesHand.localRotation = Quaternion.Euler(new Vector3(minutesRotation - 90,0, 0));
    //     // //set the rotation of the seconds hand
    //     // float secondsRotation = -(time.Second / 60f) * 360f;
    //     // secondsHand.localRotation = Quaternion.Euler(new Vector3(secondsRotation - 90, 0, 0));

    //     // //print(time.Hour + ":" + time.Minute + ":" + time.Second);
    //     // // print(hoursRotation + ":" + minutesRotation + ":" + secondsRotation);
    // }
}
