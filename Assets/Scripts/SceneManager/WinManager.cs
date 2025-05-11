using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    
    [SerializeField] Animator player;
    [SerializeField] Animator opponent;
    List<int> score;
    // Start is called before the first frame update
    void Start()
    {
        if (RoundData.winType == 2) {
            score = RoundData.totalScore();
            Debug.Log(score[0]);
            Debug.Log(score[1]);
            if (score[0]>score[1]) {
                RoundData.winner = 0;
            }
            else if (score[0]>score[1]) {
                RoundData.winner = 1;
            }
            else {
                RoundData.winner = 2;
                RoundData.winType = 3;
            }
        }
        
        if (RoundData.winner == 0) {
            player.SetTrigger("win");
            opponent.SetTrigger("lose");
        }
        else if (RoundData.winner == 1){
            player.SetTrigger("lose");
            opponent.SetTrigger("win");
        }
        else {
            player.SetTrigger("win");
            opponent.SetTrigger("win");
        }
    }

    public void Exit () {
        RoundData.counter = 1;
        RoundData.score = new List<List<int>> ();
        SceneManager.LoadScene("StartScene");
    }
    
}
