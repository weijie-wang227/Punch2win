using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestManager : MonoBehaviour
{
    private bool isEnd = false;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnRestStart();
        isEnd = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isEnd)
        {
            isEnd = true;
            EventManager.OnRestEnd();
        }
    }
    private void OnEnable()
    {
        EventManager.RestEnd += EventManagerOnRestEnd;
    }
    private void OnDisable()
    {
        EventManager.RestEnd -= EventManagerOnRestEnd;
    }
    public void EventManagerOnRestEnd()
    {
        RoundData.counter++;
        StartCoroutine(RestEnd());
    }

    private IEnumerator RestEnd()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("RoundScene");
    }


}
