using System.Collections.Generic;

public static class PunchData
{
    public static Dictionary<string,float> duration = new Dictionary<string, float>()
    {
            {"1",0.33f},
            {"2",0.5f},
            {"3",0.583f},
            {"4",0.583f},
            {"5",0.583f},
            {"6",0.583f},
    };
    public static Dictionary<string, int> stamina = new Dictionary<string, int>()
    {
        {"1",10},
        {"2",15},
        {"3",20},
        {"4",20},
        {"5",20},
        {"6",20},
    };
    public static Dictionary<string, int> damage = new Dictionary<string, int>()
    {
        {"1",1},
        {"2",2},
        {"3",3},
        {"4",3},
        {"5",4},
        {"6",4},
    };

    public static Dictionary<string,List<float>> timeSplit = new Dictionary<string, List<float>>()
    {
            {"1",new List<float> {0f,0.167f,0.166f}},
            {"2",new List<float> {0f,0.25f,0.25f}},
            {"3",new List<float> {0.167f,0.208f,0.208f}},
            {"4",new List<float> {0.021f,0.312f,0.25f}},
            {"5",new List<float> {0.25f,0.125f,0.208f}},
            {"6",new List<float> {0.167f,0.166f,0.25f}},
    };
    
}
