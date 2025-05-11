using System;
using System.Collections;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]private AudioSource ring;
    [SerializeField] private AudioSource player;
    [SerializeField] private AudioSource enemy;
    [SerializeField] private AudioSource ambient;
    [SerializeField] private AudioClip bell;
    [SerializeField] private AudioClip countdown;
    [SerializeField] private AudioClip block;
    [SerializeField] private AudioClip bodyHit;
    [SerializeField] private AudioClip headHit;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        EventManager.RoundStart += EventManagerOnRoundStart;
        EventManager.RoundEnd += EventManagerOnRoundEnd;
        EventManager.Block += EventManagerOnBlock;
        EventManager.TakeHit += EventManagerTakeHit;
        EventManager.KnockOut += EventManagerKnockOut;
        EventManager.KnockDown += EventManagerKnockDown;
        EventManager.Recover += EventManagerRecover;
    }
    void OnDisable() {
        EventManager.RoundStart -= EventManagerOnRoundStart;
        EventManager.RoundEnd -= EventManagerOnRoundEnd;
        EventManager.Block -= EventManagerOnBlock;
        EventManager.TakeHit -= EventManagerTakeHit;
        EventManager.KnockOut -= EventManagerKnockOut;
        EventManager.KnockDown -= EventManagerKnockDown;
        EventManager.Recover -= EventManagerRecover;
    }

    private void EventManagerOnRoundEnd (){
        ring.clip = bell;
        ring.Play();
    }
    private void EventManagerOnRoundStart () {
        StartCoroutine(RoundStart());
    }

    private void EventManagerKnockOut(bool _) {
        ring.clip = bell;
        ring.Play();
        StartCoroutine(CrowdCheer(0.2f));
    }

    private void EventManagerKnockDown(int _, bool _m) {
        
        StartCoroutine(CountUp());
        StartCoroutine(CrowdCheer(0.15f));
    }

    private void EventManagerRecover (int _) {
        ring.Stop();
        StartCoroutine(CrowdCheer(0.03f));
    }
    private IEnumerator CountUp() {
        yield return new WaitForSeconds(1.0f);
        ring.clip = countdown;
        ring.Play();
    }

    private IEnumerator CrowdCheer (float end) {
        float temp = 0f;
        float duration = 1.0f;
        float start = ambient.volume;
        while (temp < duration) {
            ambient.volume = Mathf.SmoothStep(start,end,temp/duration);
            yield return null;
            temp += Time.deltaTime;
        }
        ambient.volume = end;
    }

    private IEnumerator RoundStart() {
        yield return new WaitForSeconds(1.8f);
        ring.clip = bell;
        ring.Play();
    }

    private void EventManagerOnBlock (int id) {
        if (id == 0) {
            enemy.clip = block;
            enemy.Play();
        }
        else {
            player.clip = block;
            player.Play();
        }
    }

    private void EventManagerTakeHit (int id, int _m, Vector3 _, bool isHead) {
        if (id == 0) {
            if (isHead) {
                player.clip = headHit;
            }
            else {
                player.clip = bodyHit;                
            }
            player.Play();
        }
        else {
            if (isHead){
                enemy.clip = headHit;
            }
            else {
                enemy.clip = bodyHit;
            }
            enemy.Play();
        }
    }

}
