using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerController playerControl;
    private EnemyAI opponentControl;
    [SerializeField] private GameObject recoverBar;
    [SerializeField] private GameObject hitDetect;
    private Transform player;
    private Transform opponent;
    [SerializeField] private Transform model;
    [SerializeField] private GameObject clinch;
    private bool knockedDown = false;
    private List <int> knockDownCounter = new List<int> () {0,0};
    
    // Start is called before the first frame update    
    void Start()
    {
        player = GameObject.Find("Player").transform;
        playerControl = player.GetComponent<PlayerController>();
        opponent = GameObject.Find("Opponent").transform;
        opponentControl = opponent.GetComponent<EnemyAI>();
        EventManager.OnRoundStart();
    }

    void OnEnable()
    {
        EventManager.RoundEnd += EventManagerRoundEnd;
        EventManager.TimerEnd += EventManagerTimerEnd;
        EventManager.RoundStart += EventManagerRoundStart;
        EventManager.TakeHit += EventManagerTakeHit;
        EventManager.KnockDown += EventManagerKnockDown;
        EventManager.Recover += EventManagerRecover;
        EventManager.KnockOut += EventManagerKnockOut;
        EventManager.Clinch += EventManagerClinch;
        EventManager.Break += EventManagerBreak;
        
    }
    void OnDisable () {
        EventManager.RoundEnd -= EventManagerRoundEnd;
        EventManager.TimerEnd -= EventManagerTimerEnd;
        EventManager.RoundStart -= EventManagerRoundStart;
        EventManager.TakeHit -= EventManagerTakeHit;
        EventManager.KnockDown -= EventManagerKnockDown;
        EventManager.Recover -= EventManagerRecover;
        EventManager.KnockOut -= EventManagerKnockOut;
        EventManager.Clinch -= EventManagerClinch;
        EventManager.Break -= EventManagerBreak;
    }

    private void EventManagerRoundStart () {
        RoundData.sigStrikes[0] = RoundData.sigStrikes[1] = 0;

        StartCoroutine(RoundStart());
    }

    private void EventManagerRoundEnd () {
        RoundData.calculateScore(knockDownCounter);
        StartCoroutine(RoundEnd());
    }

    private void EventManagerTimerEnd () {
        if (RoundData.inClinch) {
            EventManager.OnBreak();
        }
        StartCoroutine(TimerEnd());
    }

    private void EventManagerTakeHit (int id, int damage, Vector3 _, bool isHead) {
        int otherId;
        if (id == 0) {
            otherId = 1;
        }
        else {
            otherId = 0;
        }
        if (damage > 3){
            RoundData.sigStrikes[otherId] ++;
        }
        
    }

    private void EventManagerKnockDown (int id,bool _) {
        knockDownCounter[id]++;
        if (knockDownCounter[id] > 2) {
            if (id != 0) {
                RoundData.winner = 0;
                EventManager.OnKnockOut(true);
            }
            else {
                RoundData.winner = 1;
                EventManager.OnKnockOut(true);
            }
            return;
        }
        playerControl.enabled = false;
        opponentControl.enabled = false;
        hitDetect.SetActive(false);
        clinch.SetActive(false);
        knockedDown = true;
        if (id == 0){
            StartCoroutine(KnockDown());
        }    
    }

    private void EventManagerRecover(int id){
        StartCoroutine(Recover(id));
    }

    private void EventManagerKnockOut (bool technical) {
        playerControl.enabled = false;
        opponentControl.enabled = false;
        RoundData.winType = technical ? 1 : 0; 
        recoverBar.SetActive(false);
        StartCoroutine(KnockOut());
    }

    private void EventManagerClinch (int id) {
        playerControl.enabled = false;
        opponentControl.enabled = false;
        RoundData.inClinch = true;
        hitDetect.SetActive(false);
        clinch.SetActive(true);
    }

    private void EventManagerBreak () {
        playerControl.enabled = true;
        opponentControl.enabled = true;
        RoundData.inClinch = false;
        hitDetect.SetActive(true);
        clinch.SetActive(false);
    }

    private IEnumerator RoundStart() {
        playerControl.enabled = false;
        opponentControl.enabled = false;
        hitDetect.SetActive(false);
        RoundData.isRound = true;
        yield return new WaitForSeconds(2.0f);
        playerControl.enabled = true;
        opponentControl.enabled = true;
        hitDetect.SetActive(true);
    }

    private IEnumerator RoundEnd () {
        playerControl.enabled = false;
        opponentControl.enabled = false;
        yield return new WaitForSeconds(2.0f);
        if (RoundData.counter == RoundData.totalRounds) {
            RoundData.winType = 2;
            SceneManager.LoadScene("WinScene");
        }
        else {
            SceneManager.LoadScene("RestScene");
        }
    }

    private IEnumerator TimerEnd () {
        float temp = 0;
        RoundData.isRound = false;
        while (temp < 11f) {
            yield return null;
            if (!knockedDown) {
                EventManager.OnRoundEnd();
                yield break;
            }
            else {
                temp += Time.deltaTime;
            }
        }
        EventManager.OnRoundEnd();
    }

    private IEnumerator Recover (int id) {
        if (id == 0){
            recoverBar.SetActive(false);
        }
        yield return new WaitForSeconds(2.0f);
        playerControl.enabled = true;
        opponentControl.enabled = true;
        hitDetect.SetActive(true);
        knockedDown = false;
    }

    private IEnumerator KnockOut () {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("WinScene");
    }

    private IEnumerator KnockDown () {
        yield return new WaitForSeconds(1.0f);
        recoverBar.SetActive(true);
    }

}
