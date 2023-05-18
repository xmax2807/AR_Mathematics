using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class RandomQuestion : MonoBehaviour
{
    List<string> posits = new List<string>();
    public string posit;
    private TMP_Text aimText;
    public int numForPos;
    // Start is called before the first frame update
    void Start()
    {
        posits.Add("trên");
        posits.Add("dưới");
        posits.Add("trái");
        posits.Add("phải");
        posits.Add("trước");
        posits.Add("sau");

        aimText = GetComponent<TMP_Text>();
        Debug.Log("Question");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            posit = posits[Random.Range(0,posits.Count)];
            aimText.text = "Chọn vật có vị trí "+posit+" so với khối lập phương trắng ?";
            Debug.Log("Random question");
        }
    }

    public void PositionToNumber()
    {
        if(posit == "trên")
        {
            numForPos = 1;
        }
        else if(posit == "dưới")
        {
            numForPos = 2;
        }    
        else if(posit == "trái")
        {
            numForPos = 3;
        }    
        else if(posit == "phải")
        {
            numForPos = 4;
        }
        else if (posit == "trước")
        {
            numForPos = 5;
        }
        else if (posit == "sau")
        {
            numForPos = 6;
        }
    }
}
