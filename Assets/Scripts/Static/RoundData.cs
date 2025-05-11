using System.Collections.Generic;
using UnityEngine;

public static class RoundData
{
    public static int counter = 1;
    public static int totalRounds = 3;
    public static List<int> sigStrikes = new List<int> () {0,0};
    public static List<List<int>> score = new List<List<int>> ();
    public static int winner;
    public static int winType = 2;
    public static bool isRound;
    public static bool inClinch;
    public static void calculateScore (List<int> knockDown) {        
        int diffStrike = sigStrikes[0] - sigStrikes[1];
        int diffDown = knockDown[0] - knockDown[1];
        int diff;
        
        if (diffStrike > 4){
            diff = 2;
        }
        else if (diffStrike > 1){
            diff = 1;
        }
        else if (diffStrike > -2){
            diff = 0;
        }
        else if (diffStrike > -5){
            diff = -1;
        }
        else {
            diff = -2;
        }
        diff = Mathf.Min(Mathf.Max(diff - diffDown,-2),2);
        
        switch (diff) {
            case 2: score.Add(new List <int> () {10,8});break;
            case 1: score.Add(new List <int> () {10,9});break;
            case 0: score.Add(new List <int> () {10,10});break;
            case -1: score.Add(new List <int> () {9,10});break;
            case -2: score.Add(new List <int> () {8,10});break;
        }
    }
    public static List <int> totalScore () {
        List <int> temp = new List <int> () {0,0};
        foreach (List<int> round in score){
            temp[0] += round[0];
            temp[1] += round[1];
        }
        return temp;
    }
}
