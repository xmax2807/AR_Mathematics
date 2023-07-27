using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
/// <summary>
/// The reference is from: https://youtu.be/IRocHHdzww4
/// </summary>
public class ScrollingCredit : MonoBehaviour
{
    [Header("Text components")]
    [SerializeField] private TMPro.TextMeshProUGUI presenter;
    [SerializeField] private RectTransform textTransform;
    [SerializeField] private TextAsset textFile;
    [SerializeField] private float startDelay = 0.5f;

    [Header("Animation Variables")]
    [SerializeField] private float lineHeight;
    [SerializeField] private float yDistance;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private int maxLinesOnScreen;


    private bool isStarted = false;
    private Vector2 startPos;
    private float currentY;
    private int linesDisplayed;

    private string[][] dataInLines;
    private StringBuilder stringBuilder;
    

    private void Awake(){
        stringBuilder = new StringBuilder();
        SplitData();
    }

    private void SplitData(){
        string[] lines = textFile.text.Split("\r\n", System.StringSplitOptions.RemoveEmptyEntries);
        dataInLines = new string[lines.Length][];
        for(int i = 0; i < lines.Length; ++i){
            dataInLines[i] = lines[i].Split(',', System.StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public void Start(){
        startPos = textTransform.anchoredPosition;
        presenter.text = "";

        textTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxLinesOnScreen * lineHeight);

        StartCoroutine(Wait());
    }

    private void Update(){
        if(!isStarted){
            return;
        }
        currentY += Time.deltaTime * scrollSpeed;

        while(currentY >= yDistance){
            if(linesDisplayed > maxLinesOnScreen + 2 && presenter.alignment != TextAlignmentOptions.Top){
                presenter.alignment = TextAlignmentOptions.Top;
            }
            
            LinesToText();

            currentY -= yDistance;
            ++linesDisplayed;

            if(linesDisplayed > dataInLines.Length){
                //Stop animation
                this.enabled = false;
                isStarted = false;
            }
        }

        textTransform.anchoredPosition = startPos + new Vector2(0, currentY);
    }

    private IEnumerator Wait(){
        yield return startDelay;
        isStarted = true;
    }

    private void LinesToText(){

        //Clear chars in stringBuilder
        stringBuilder.Clear();

        // The index will be at the first line, until it's off screen
        int rowIndex = Mathf.Max(0, linesDisplayed - maxLinesOnScreen);

        //Allows fill-in, full screen and fill-out
        int rowCount = Mathf.Min(linesDisplayed, maxLinesOnScreen, dataInLines.Length - linesDisplayed);

        for(int i =0; i < rowCount; ++i){
            for(int j = 0; j < dataInLines[rowIndex].Length; ++j){
                if(j > 0){
                    stringBuilder.Append('-');
                }

                stringBuilder.Append(dataInLines[rowIndex][j]);
            }
            ++rowIndex;

            if(i < rowCount - 1){
                stringBuilder.Append('\n');
            }
        }

        presenter.text = stringBuilder.ToString();
    }
}
