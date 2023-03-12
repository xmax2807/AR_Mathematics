using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    GameObject hoursHand;
    [SerializeField]
    GameObject minutesHand;
    [SerializeField]
    GameObject secondsHand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //create datetime
        System.DateTime time = System.DateTime.Now;
        Debug.Log(time.Hour);
        //set the rotation of the hours hand
        float hoursRotation = -(time.Hour / 12f) * 360f;
        hoursHand.transform.localRotation = Quaternion.Euler(new Vector3(hoursRotation - 90, 0, 0));
        //set the rotation of the minutes hand
        float minutesRotation = -(time.Minute / 60f) * 360f;
        minutesHand.transform.localRotation = Quaternion.Euler(new Vector3(minutesRotation - 90,0, 0));
        //set the rotation of the seconds hand
        float secondsRotation = -(time.Second / 60f) * 360f;
        secondsHand.transform.localRotation = Quaternion.Euler(new Vector3(secondsRotation - 90, 0, 0));

        //print(time.Hour + ":" + time.Minute + ":" + time.Second);
        print(hoursRotation + ":" + minutesRotation + ":" + secondsRotation);
    }
}
