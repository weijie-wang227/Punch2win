using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreenUI : MonoBehaviour
{
    [SerializeField] GameObject exitButton;
    [SerializeField] GameObject dataText;
    [SerializeField] TMP_Text screen;
    [SerializeField] TMP_Text data;
    int winType;
    void Start()
    {
        
        StartCoroutine(RevealWinner());
        
    }

    string message1 = "The winner by KNOCKOUT is ";
    string message2 = "The winner by TECHNICAL KNOCKOUT is ";
    string message3 = "The winner by DECISION is ";
    string message4 = "The outcome of this match is ";
    string message;

    private IEnumerator RevealWinner () {
        yield return null;
        winType = RoundData.winType;
        data.text = "              " + PlayerManager.names[0] + " | " + PlayerManager.names[1];
        int counter = 0;
        foreach (List<int> round in RoundData.score) {
            counter ++;
            data.text += "\n" + "Round " + counter.ToString() + ": " + round[0].ToString() + " | " + round[1].ToString();
        }
        if (winType == 1||winType == 0){
            data.text += "\n" + "Round " + (counter+1 ).ToString() + ": KNOCKOUT";
        }
        
        yield return new WaitForSeconds (1f);
        if (winType == 0){
            message = message1;
        }
        else if (winType == 1){
            message = message2;
        }
        else if (winType == 2){
            message = message3;
        }
        else {
            message = message4;
        }

        float interval = 3.5f / (message.Length + 1);

        for (int i = 0; i < message.Length; i++) {
            screen.text = message.Substring(0,i);
            yield return new WaitForSeconds(interval);
        }

        for (int i = 0; i < 3; i ++) {
            screen.text = screen.text + ".";
            yield return new WaitForSeconds(1.0f);
        }

        screen.fontSize = 40;

        if (winType == 3) {
            screen.text = "DRAW";
        }
        else {
            screen.text = PlayerManager.names[RoundData.winner];
        }

        yield return new WaitForSeconds(2.0f);
        exitButton.SetActive(true);
        dataText.SetActive(true);
    }
}
