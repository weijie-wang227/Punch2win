using System.Collections;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    private TMP_Text displayText;
    private Coroutine countRoutine;
    void Start () {
        displayText = GetComponent<TMP_Text>();
    }
    
    void OnEnable () {
        EventManager.RoundStart += EventManagerRoundStart;
        EventManager.RoundEnd += EventManagerRoundEnd;
        EventManager.KnockDown += EventManagerKnockDown;
        EventManager.Recover += EventManagerRecover;
        EventManager.KnockOut += EventManagerKnockOut;
        EventManager.Clinch += EventManagerClinch;
    }

    void OnDisable () {
        EventManager.RoundStart -= EventManagerRoundStart;
        EventManager.RoundEnd -= EventManagerRoundEnd;
        EventManager.KnockDown -= EventManagerKnockDown;
        EventManager.Recover -= EventManagerRecover;
        EventManager.KnockOut -= EventManagerKnockOut;
        EventManager.Clinch -= EventManagerClinch;
    }

    void EventManagerRoundStart () {
        StartCoroutine(Display("Round " + RoundData.counter.ToString(),1.0f));
        StartCoroutine(Display("Fight",2.0f));
    }
    void EventManagerRoundEnd () {
        StartCoroutine(Display("Round End"));
    }
    void EventManagerKnockDown (int id, bool _m) {
        StartCoroutine(Display("KNOCKDOWN"));
        countRoutine = StartCoroutine(CountUp(id));
    }

    void EventManagerRecover (int _) {
        StopCoroutine(countRoutine);
        if (RoundData.isRound){
            StartCoroutine(Display("Fight",1.0f));
        }
    }

    void EventManagerKnockOut (bool technical) {
        if (technical) {
            StartCoroutine(Display("Technical KNOCKOUT"));
        }
        else{
            StartCoroutine(Display("KNOCKOUT"));
        }
        if (countRoutine != null) {
            StopCoroutine(countRoutine);
        }
    }
    
    void EventManagerClinch (int id) {
        StartCoroutine(Clinch());
    }

    private IEnumerator CountUp (int id) {
        yield return new WaitForSeconds(2.0f);
        for (int i = 1; i < 11; i++) {
            StartCoroutine(Display(i.ToString()));
            yield return new WaitForSeconds(1.0f);
        }
        if (id != 0){
            RoundData.winner = 0;
        }
        else {
            RoundData.winner = 1;
        }
        EventManager.OnKnockOut(false);
    }
    private IEnumerator Clinch () {
        yield return new WaitForSeconds(10.0f);
        Display("Break");
        yield return new WaitForSeconds(1.0f);
        EventManager.OnBreak();
    }

    private IEnumerator Display (string text, float delay = 0f) {
        yield return new WaitForSeconds(delay);
        displayText.text = text;
        Color newColor = displayText.color;
        newColor.a = 1;
        displayText.color = newColor;
        yield return new WaitForSeconds(0.2f);
        float tempTime = 0f;
        while (tempTime < 0.7f) {
            newColor.a = Mathf.SmoothStep(1f, 0.2f,tempTime/0.8f);
            displayText.color = newColor;
            yield return null;
            tempTime += Time.deltaTime;
        }
        newColor.a = 0f;
        displayText.color = newColor;
    }

    
}
