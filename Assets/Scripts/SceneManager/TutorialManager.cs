using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorialmanager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void onEnable()
    {
        EventManager.EventManagerBasics += EventManagerBasics;
        EventManager.EventManagerMove += EventManagerMove;
        EventManager.EventManagerJab += EventManagerJab;
        EventManager.EventManagerSwing += EventManagerSwing;
        EventManager.EventManagerRecover += EventManagerRecover;
    }

    void OnDisable()
    {
        EventManager.EventManagerBasics += EventManagerBasics;
        EventManager.EventManagerMove += EventManagerMove;
        EventManager.EventManagerJab += EventManagerJab;
        EventManager.EventManagerSwing += EventManagerSwing;
        EventManager.EventManagerRecover += EventManagerRecover;
    }

    private void EventManagerBasics() { }

    private void EventManagerMove() { }

    private void EventManagerJab() { }

    private void EventManagerSwing() { }

    private void EventManagerRecover() { }
}
