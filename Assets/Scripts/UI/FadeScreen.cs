using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    private Image blackScreen;
    void Start (){
        blackScreen = GetComponent<Image>();
        StartFadeIn();
    }

    void OnEnable(){
        EventManager.RestEnd += StartFadeOut;
        EventManager.RoundEnd += EventManagerRoundEnd;
        EventManager.KnockOut += EventManagerKnockOut;
    }
    void OnDisable(){
        EventManager.RestEnd -= StartFadeOut;
        EventManager.RoundEnd -= EventManagerRoundEnd;
        EventManager.KnockOut -= EventManagerKnockOut;
    }

    void EventManagerRoundEnd() {
        StartCoroutine(FadeOut(1.0f,1.0f));
    }

    void EventManagerKnockOut(bool _) {
        StartCoroutine(FadeOut(1.0f,1.0f));
    }

    public void StartFadeOut () {
        StartCoroutine(FadeOut());
    }

    public void StartFadeIn() {
        StartCoroutine(FadeIn(1.0f));
    }

    private IEnumerator FadeOut(float duration = 1.0f, float delay = 0f) {
		yield return new WaitForSeconds(delay);
        float timer = 0f;
		Color newColor = blackScreen.color;
		while (timer<duration){
			newColor.a = Mathf.SmoothStep(0, 1, timer/duration);
			blackScreen.color = newColor;
			timer += Time.deltaTime;
			yield return null;
		}
	}
    private IEnumerator FadeIn(float duration) {
		float timer = 0f;
		Color newColor = blackScreen.color;
		while (timer<duration){
			newColor.a = Mathf.SmoothStep(1, 0, timer/duration);
			blackScreen.color = newColor;
			timer += Time.deltaTime;
			yield return null;
		}
	}
}
