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

    private Coroutine tickCoroutine;
    void Start(){
        // //init time
        // System.DateTime time = System.DateTime.Now;
        // SetHour(time);
        // SetMinute(time);
        // SetSecond(time);
        // tickCoroutine = TimeCoroutineManager.Instance.DoLoopAction(IncreaseSecond, ()=>false, 1);
        // TimeCoroutineManager.Instance.DoLoopAction(()=>{
        //     SetHour(Random.Range(1,13));
        // }, ()=>false, 5);
    }
    void onDestroy(){
        TimeCoroutineManager.Instance.StopCoroutine(tickCoroutine);
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
    public void SetSecond(int value){
        float secondsRotation = -(value / 60f) * 360f;
        secondsHand.localRotation = Quaternion.Euler(new Vector3(secondsRotation - 90, 0, 0));
    }
    public void SetMinute(int value){
        float minutesRotation = -(value / 60f) * 360f;
        Quaternion destination = Quaternion.Euler(new Vector3(minutesRotation - 90, 0, 0));
        minutesHand.localRotation = destination;
    }
     public void SetHour(int value){
        float hoursRotation = -(value / 12f) * 360f;
        Quaternion destination = Quaternion.Euler(new Vector3(hoursRotation - 90, 0, 0));
        StartCoroutine(Rotate(destination, hoursHand, duration));
    }
    public void RandomHour(){
        SetTime(UnityEngine.Random.Range(1,13));
    }

    private int seconds;
    public void SetTime(int hour, float animationDuration = 2f){
        duration = animationDuration;
        SetHour(hour);
        SetMinute(0);
        //SetSecond(0);
        if(tickCoroutine != null){
            TimeCoroutineManager.Instance.StopCoroutine(tickCoroutine);
        }
        seconds = 0;
        tickCoroutine = TimeCoroutineManager.Instance.DoLoopAction(IncreaseSecond, ()=>false, 1f);
    }
    public void IncreaseSecond(){
        seconds = (seconds + 1) % 60;
        float newXRotation = -(seconds / 60f) * 360f;
        secondsHand.localRotation = Quaternion.Euler(new Vector3(newXRotation, 0, 0));
    }

    public Quaternion destinationRotation;
    private float duration = 2f;
    IEnumerator Rotate(Quaternion destinationRotation, Transform hand, float duration)
    {        
        float elapsed = 0f;
        Quaternion startRotation = hand.localRotation;
        while (elapsed < duration)
        {
            hand.localRotation = Quaternion.Lerp(startRotation, destinationRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        hand.localRotation = destinationRotation;
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
