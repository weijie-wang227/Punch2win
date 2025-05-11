using System.Collections;
using UnityEngine;
using TMPro;

public class ClinchUI : MonoBehaviour
{
    private TMP_Text displayText;
    private float countDown = 0.75f;
    private float between;
    private Coroutine routine;
    private bool press = false;
    private string letters = "qwertyuiopasdfghjkzxcvbnm";
    void Start()
    {
        between = 0.5f + (Random.value * 2) + Time.time;
        displayText = transform.GetChild(0).GetComponent<TMP_Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > between && routine == null) {
            routine = StartCoroutine(QuickTime());
        }
    }
    private IEnumerator QuickTime () {
        char letter = letters[Random.Range(0, letters.Length)];
        float temp = 0f;
        Debug.Log("clinch");
        displayText.text = "Press: " + letter;
        while (temp < countDown) {
            if (Input.anyKeyDown) {
                if (Input.inputString != null) {
                    if (Input.inputString[0] == letter) {
                        EventManager.OnClinchPunch(0);
                        press = true;
                        break;
                    }
                    else {
                        EventManager.OnClinchPunch(1);
                        press = true;
                        break;
                    }
                }
                else {
                    EventManager.OnClinchPunch(1);
                    press = true;
                    break;
                }
                
            }
            temp += Time.deltaTime;
            yield return null;
        }
        displayText.text = "";
        if (!press) {
            EventManager.OnClinchPunch(1);
        }
        press = false;
        between = 0.5f + (Random.value * 2) + Time.time;
        routine = null;
    }
    void OnDisable () {
        displayText.text = "";
    }
}
