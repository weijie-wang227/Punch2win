using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataText : MonoBehaviour
{
    // Start is called before the first frame update
    private TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = "Round " + RoundData.counter.ToString() + 
            "\n Significant Stikes:\n" + 
            "  Player: " + RoundData.sigStrikes[0].ToString() + 
            " Opponent: " + RoundData.sigStrikes[1].ToString() + 
            "\n  Judge Score: " + RoundData.score[RoundData.counter-1][0].ToString() + " : " + RoundData.score[RoundData.counter-1][1].ToString();
    }



}
