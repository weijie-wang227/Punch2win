using UnityEngine;
using TMPro;

public class WeighIn : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text playerHW;
    [SerializeField] TMP_Text playerRecord;
    [SerializeField] TMP_Text opponentName;
    [SerializeField] TMP_Text opponentHW;
    [SerializeField] TMP_Text opponentRecord;
    [SerializeField] TMP_Text roundText;
    private int rounds = 3;

    public void Confirm()
    {
        if (PlayerManager.names[0] == "") {
            PlayerManager.names[0] = "player";
        }
        playerName.text = PlayerManager.names[0];
        playerHW.text = Random.Range(81,91).ToString() + "kg/" + ((int) PlayerManager.bonus[0]["height"]).ToString() + "cm";
        playerRecord.text = "0-0-0";

        opponentName.text = PlayerManager.names[1];
        opponentHW.text = Random.Range(81,91).ToString() + "kg/" + ((int)PlayerManager.bonus[1]["height"]).ToString() + "cm";
        opponentRecord.text = "0-0-0";

    }

    public void IncreaseRounds () {
        rounds = Mathf.Min(rounds+1,10);
        RoundData.totalRounds = rounds;
        roundText.text = "Rounds:\n" + rounds.ToString();
    }
    public void DecreaseRounds () {
        rounds = Mathf.Max(rounds-1,3);
        RoundData.totalRounds = rounds;
        roundText.text = "Rounds:\n" + rounds.ToString();
    }

}
