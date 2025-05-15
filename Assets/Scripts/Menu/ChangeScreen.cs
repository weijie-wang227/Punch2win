using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScreen : MonoBehaviour
{
    [SerializeField] List<RectTransform> panels;
    bool istransition = false;
    int index = 0;
    float duration = 0.5f;
    List<int> previousPanels = new List<int>();

    public void Play()
    {
        SceneManager.LoadScene("RoundScene");
    }
    public void Forward()
    {
        if (!istransition)
        {
            StartCoroutine(NextScreen());
        }
    }
    public void Tutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    IEnumerator NextScreen(int nextID = -1)
    {
        istransition = true;
        if (nextID == -1)
        {
            nextID = index + 1;
        }

        float temp = 0f;
        float currentX;
        Vector2 currentPos = panels[index].anchoredPosition;
        Vector2 nextPos = panels[nextID].anchoredPosition;

        while (temp < duration)
        {
            currentX = Mathf.SmoothStep(0, -800f, temp / duration);
            currentPos.x = currentX;
            nextPos.x = currentX + 800f;
            panels[index].anchoredPosition = currentPos;
            panels[nextID].anchoredPosition = nextPos;
            temp += Time.deltaTime;
            yield return null;
        }

        previousPanels.Add(index);
        index = nextID;
        istransition = false;

    }

    public void Back()
    {
        if (!istransition)
        {
            StartCoroutine(PreviousScreen());
        }
    }
    IEnumerator PreviousScreen()
    {
        istransition = true;
        float temp = 0f;
        float currentX;
        int nextID = previousPanels[^1];
        Vector2 currentPos = panels[index].anchoredPosition;
        Vector2 nextPos = panels[nextID].anchoredPosition;


        while (temp < duration)
        {
            currentX = Mathf.SmoothStep(0, 800f, temp / duration);
            currentPos.x = currentX;
            nextPos.x = currentX - 800f;
            panels[index].anchoredPosition = currentPos;
            panels[nextID].anchoredPosition = nextPos;
            temp += Time.deltaTime;
            yield return null;
        }

        previousPanels.Remove(previousPanels.Count - 1);
        index = nextID;
        istransition = false;

    }


}
