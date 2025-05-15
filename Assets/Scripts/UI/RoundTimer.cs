using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class RoundTimer : MonoBehaviour
{
    private TMP_Text _timerText;
    [SerializeField] private float roundDuration = 60.0f;
    [SerializeField] private float restDuration = 20.0f;
    private float countDown;
    private bool isRunning;
    private Action action;


    // Start is called before the first frame update
    void Start()
    {
        _timerText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        EventManager.RoundStart += EventManagerRoundStart;
        EventManager.RoundEnd += EventManagerRoundEnd;
        EventManager.TimerEnd += EventManagerTimerEnd;
        EventManager.RestStart += EventManagerRestStart;
        EventManager.RestEnd += EventManagerRestEnd;
        EventManager.Defense += EventManagerDefense;
    }

    private void OnDisable()
    {
        EventManager.RoundStart -= EventManagerRoundStart;
        EventManager.RoundEnd -= EventManagerRoundEnd;
        EventManager.TimerEnd -= EventManagerTimerEnd;
        EventManager.RestStart -= EventManagerRestStart;
        EventManager.RestEnd -= EventManagerRestEnd;
        EventManager.Defense -= EventManagerDefense;
    }



    // Update is called once per frame
    private void EventManagerRoundStart()
    {
        countDown = roundDuration;
        action = EventManager.OnTimerEnd;
        StartCoroutine(RoundStart());
    }

    private IEnumerator RoundStart()
    {
        yield return new WaitForSeconds(2.0f);
        isRunning = true;
    }

    private void EventManagerRoundEnd() => isRunning = false;
    private void EventManagerTimerEnd() => isRunning = false;
    private void EventManagerRestStart()
    {
        countDown = restDuration;
        isRunning = true;
        action = EventManager.OnRestEnd;
    }

    private void EventManagerRestEnd()
    {
        isRunning = false;
    }


    private void EventManagerDefense()
    {
        Invoke("Defense", 7f);
    }

    private void Defense()
    {
        countDown = 20f;
        isRunning = true;
        action = () => { };
    }

    void Update()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(countDown);
        _timerText.text = timeSpan.ToString(format: @"mm\:ss\:ff");
        if (!isRunning) return;
        if (countDown < 0f)
        {
            isRunning = false;
            action();
        }
        ;
        countDown -= Time.deltaTime;

    }
}
