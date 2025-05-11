using System.Collections.Generic;
using UnityEngine;

public static class PlayerManager
{
    // Start is called before the first frame update
    public static List<bool> isPunchOut = new List<bool>() {false,false};
    public static List<bool> isPunching = new List<bool>() {false,false};
    public static List<bool> isDown = new List<bool>() {false,false};

    public static List<string> punchType = new List<string> () {"1","2"};
    public static List<int> maxHealth = new List<int> () {100,100};
    public static List<int> health = new List<int> () {100,100};
    public static List<bool> isBlocked = new List<bool>() {false,false};
    public static List<int> knockDownCounter = new List<int> () {0,0};
    public static List<Vector3> position = new List <Vector3> () {new Vector3(),new Vector3 ()};
    public static float distance () {
        return (position[0] - position[1]).magnitude;
    }
    public static int roundCounter = 1;
    public static List <Dictionary <string,int>> skin = new List<Dictionary<string, int>> () {
        new Dictionary<string, int> () {{"gloves",0},{"shorts",0},{"shoes",0}}, 
        new Dictionary<string, int> () {{"gloves",1},{"shorts",1},{"shoes",1}}
    };

    public static List <Dictionary <string, float>> bonus = new List<Dictionary<string,float>> () {
        new Dictionary<string,float> () {{"height",190}, {"damage",0f}, {"stamina",1.0f}},
        new Dictionary<string,float> () {{"height",210}, {"damage",3f}, {"stamina",2.0f},{"aggressiveness",1.0f},{"skill",1.0f}},
    };

    public static List <string> names = new List<string> () {"player","opponent"};
}
