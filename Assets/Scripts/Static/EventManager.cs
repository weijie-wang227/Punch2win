using UnityEngine.Events;
using UnityEngine;

public static class EventManager
{
    public static event UnityAction RoundStart;
    public static event UnityAction RoundEnd;
    public static event UnityAction TimerEnd;
    public static event UnityAction RestStart;
    public static event UnityAction RestEnd;
    public static event UnityAction<int> Block;
    public static event UnityAction<int, int, Vector3, bool> TakeHit;
    public static event UnityAction<int, bool> KnockDown;
    public static event UnityAction<int> Recover;
    public static event UnityAction<bool> KnockOut;
    public static event UnityAction<int> Clinch;
    public static event UnityAction Break;
    public static event UnityAction<int> ClinchPunch;
    public static event UnityAction Basics;
    public static event UnityAction Move;
    public static event UnityAction Jab;
    public static event UnityAction Swing;
    public static event UnityAction Defense;
    public static event UnityAction TutorialEnd;


    public static void OnRoundStart() => RoundStart?.Invoke();
    public static void OnRoundEnd() => RoundEnd?.Invoke();
    public static void OnTimerEnd() => TimerEnd?.Invoke();
    public static void OnRestStart() => RestStart?.Invoke();
    public static void OnRestEnd() => RestEnd?.Invoke();
    public static void OnBlock(int value) => Block?.Invoke(value);
    public static void OnTakeHit(int checkID, int damage, Vector3 direction, bool isHead) => TakeHit?.Invoke(checkID, damage, direction, isHead);
    public static void OnKnockDown(int id, bool isBackDown) => KnockDown?.Invoke(id, isBackDown);
    public static void OnRecover(int id) => Recover?.Invoke(id);
    public static void OnKnockOut(bool technical = false) => KnockOut?.Invoke(technical);
    public static void OnClinch(int value) => Clinch?.Invoke(value);
    public static void OnBreak() => Break?.Invoke();
    public static void OnClinchPunch(int id) => ClinchPunch?.Invoke(id);
    public static void OnBasics() => Basics?.Invoke();
    public static void OnMove() => Move?.Invoke();
    public static void OnJab() => Jab?.Invoke();
    public static void OnSwing() => Swing?.Invoke();
    public static void OnDefense() => Defense?.Invoke();
    public static void OnTutorialEnd() => TutorialEnd?.Invoke();
}
