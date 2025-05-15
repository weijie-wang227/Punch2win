using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject oppHealthBar;
    [SerializeField] private GameObject oppStaminaBar;
    [SerializeField] private GameObject playerHealthBar;
    [SerializeField] private GameObject playerStaminaBar;
    [SerializeField] private GameObject playerName;
    [SerializeField] private GameObject enemyName;
    [SerializeField] private GameObject timerText;
    [SerializeField] private GameObject scenes;
    [SerializeField] private Image middle;
    [SerializeField] private Image left;
    [SerializeField] private Image right;
    [SerializeField] private Image bottom;
    [SerializeField] private GameObject hitIcons;
    private int id = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        EventManager.Basics += EventManagerBasics;
        EventManager.Move += EventManagerMove;
        EventManager.Jab += EventManagerJab;
        EventManager.Swing += EventManagerSwing;
        EventManager.Defense += EventManagerDefense;
        EventManager.TutorialEnd += EventManagerTutorialEnd;
    }

    void OnDisable()
    {
        EventManager.Basics -= EventManagerBasics;
        EventManager.Move -= EventManagerMove;
        EventManager.Jab -= EventManagerJab;
        EventManager.Swing -= EventManagerSwing;
        EventManager.Defense -= EventManagerDefense;
        EventManager.TutorialEnd -= EventManagerTutorialEnd;
    }

    private void EventManagerBasics()
    {
        StartCoroutine(Basics());
    }

    private void EventManagerMove()
    {
        StartCoroutine(Move());
    }

    private void EventManagerJab()
    {
        hitIcons.SetActive(true);
        middle.color = new Color(0f, 1f, 0, 0.2f);
        middle.raycastTarget = true;
        left.color = new Color(1f, 0f, 0f, 0.2f);
        left.raycastTarget = false;
        right.color = new Color(1f, 0f, 0f, 0.2f);
        right.raycastTarget = false;
        bottom.color = new Color(1f, 0f, 0f, 0.2f);
        bottom.raycastTarget = false;
        StartCoroutine(Jab());
    }

    private void EventManagerSwing()
    {
        middle.color = new Color(1f, 0f, 0f, 0.2f);
        middle.raycastTarget = false;
        left.color = new Color(0f, 1f, 0, 0.2f);
        left.raycastTarget = true;
        right.color = new Color(0f, 1f, 0, 0.2f);
        right.raycastTarget = true;
        bottom.color = new Color(0f, 1f, 0, 0.2f);
        bottom.raycastTarget = true;
        StartCoroutine(Swing());
    }

    private void EventManagerDefense()
    {
        middle.color = new Color(0, 0, 0, 0);
        left.color = new Color(0, 0, 0, 0);
        right.color = new Color(0, 0, 0, 0);
        bottom.color = new Color(0, 0, 0, 0);
        left.raycastTarget = false;
        right.raycastTarget = false;
        bottom.raycastTarget = false;
        hitIcons.SetActive(false);
        StartCoroutine(Defense());
    }

    private void EventManagerTutorialEnd()
    {
        scenes.transform.GetChild(12).gameObject.SetActive(true);
    }

    private IEnumerator Basics()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                switch (id)
                {
                    case 0:
                        timerText.SetActive(true);
                        break;
                    case 1:
                        playerName.SetActive(true);
                        playerHealthBar.SetActive(true);
                        break;
                    case 2:
                        playerStaminaBar.SetActive(true);
                        break;
                    case 3:
                        enemyName.SetActive(true);
                        oppHealthBar.SetActive(true);
                        oppStaminaBar.SetActive(true);
                        break;
                }

                scenes.transform.GetChild(id).gameObject.SetActive(false);
                id += 1;
                if (id < 5)
                {
                    scenes.transform.GetChild(id).gameObject.SetActive(true);
                }
                else
                {
                    EventManager.OnMove();
                    yield break;
                }
            }
            yield return null;
        }
    }

    private IEnumerator Move()
    {
        scenes.transform.GetChild(5).gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        scenes.transform.GetChild(5).gameObject.SetActive(false);
        scenes.transform.GetChild(6).gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        scenes.transform.GetChild(6).gameObject.SetActive(false);
        while (PlayerManager.distance() > 120)
        {
            yield return null;
        }
        EventManager.OnJab();
    }

    private IEnumerator Jab()
    {
        scenes.transform.GetChild(7).gameObject.SetActive(true);
        while (true)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                scenes.transform.GetChild(7).gameObject.SetActive(false);
                yield return new WaitForSeconds(10.0f);
                EventManager.OnSwing();
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator Swing()
    {
        scenes.transform.GetChild(8).gameObject.SetActive(true);
        while (true)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                yield return new WaitForSeconds(1.0f);
                scenes.transform.GetChild(8).gameObject.SetActive(false);
                yield return new WaitForSeconds(10.0f);
                EventManager.OnDefense();
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator Defense()
    {
        scenes.transform.GetChild(9).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        scenes.transform.GetChild(9).gameObject.SetActive(false);
        scenes.transform.GetChild(10).gameObject.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        scenes.transform.GetChild(10).gameObject.SetActive(false);
        scenes.transform.GetChild(11).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        scenes.transform.GetChild(11).gameObject.SetActive(false);
        yield return new WaitForSeconds(20.0f);
        EventManager.OnTutorialEnd();
    }
}
