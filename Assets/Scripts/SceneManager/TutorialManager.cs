using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorialmanager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private EnemyAI enemy;
    void Start()
    {
        EventManager.OnBasics();
    }

    void OnEnable()
    {
        EventManager.Defense += EventManagerDefense;
        EventManager.TutorialEnd += EventManagerTutorialEnd;
    }

    void Oisable()
    {
        EventManager.Defense -= EventManagerDefense;
        EventManager.TutorialEnd -= EventManagerTutorialEnd;
    }

    private void EventManagerDefense()
    {
        StartCoroutine(Defense());
    }

    private void EventManagerTutorialEnd()
    {
        enemy.enabled = false;
    }

    private IEnumerator Defense()
    {
        yield return new WaitForSeconds(7.0f);
        enemy.enabled = true;
    }

    public void Return()
    {
        SceneManager.LoadScene("StartScene");
    }
}
