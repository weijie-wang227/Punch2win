using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class RoundTimer : MonoBehaviour
{
    private TMP_Text _timerText;
    [SerializeField] private float roundDuration = 60.0f;
    [SerializeField] private float restDuration = 20.0f;
    private float countDown;
    private bool isRunning;
    private bool isRound;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _timerText = GetComponent<TMP_Text>();
    }

    private void OnEnable() {
        EventManager.RoundStart += EventManagerOnRoundStart;
        EventManager.RoundEnd += EventManagerOnRoundEnd;
        EventManager.RestStart += EventManagerOnRestStart;
        EventManager.RestEnd += EventManagerOnRestEnd;
    }

    private void OnDisable() {
        EventManager.RoundStart -= EventManagerOnRoundStart;
        EventManager.RoundEnd -= EventManagerOnRoundEnd;
        EventManager.RestStart -= EventManagerOnRestStart;
        EventManager.RestEnd -= EventManagerOnRestEnd;
    }

    // Update is called once per frame
    private void EventManagerOnRoundStart() {
        countDown = roundDuration;
        isRound = true;
        StartCoroutine(RoundStart());
    }

    private IEnumerator RoundStart() {
        yield return new WaitForSeconds(2.0f);
        isRunning = true;
    }

    private void EventManagerOnRoundEnd() => isRunning = false;
    private void EventManagerOnRestStart() {
        countDown = restDuration;
        isRunning = true;
        isRound = false;
    }

    private void EventManagerOnRestEnd(){
        isRunning = false;
    }

    void Update () {
        TimeSpan timeSpan = TimeSpan.FromSeconds(countDown);
        _timerText.text = timeSpan.ToString(format:@"mm\:ss\:ff");
        if (!isRunning) return;
        if (countDown < 0f) {
            if (isRound){
                EventManager.OnTimerEnd();
                isRunning = false;
            }
            else {EventManager.OnRestEnd();}
        };
        countDown -= Time.deltaTime;
        
    }
}
