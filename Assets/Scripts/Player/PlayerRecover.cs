using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRecover : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider bar;
    [SerializeField] private float speed = 1;
    [SerializeField] private float baseInputSpeed = 10;
    private float inputSpeed;
    private float barValue;
    void OnEnable()
    {
        bar = GetComponent<Slider>();
        bar.maxValue = 100f;
        barValue = bar.value = 0f;
        inputSpeed = baseInputSpeed - ((PlayerManager.knockDownCounter[0]-1)*2);
        StartCoroutine(Recover());
    }

    private IEnumerator Recover () {
        yield return new WaitForSeconds(1.0f);
        bool isRunning = false;
        while (true) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                barValue += inputSpeed;
                isRunning = true;
            }
            if (isRunning && barValue > 0){
                barValue -= speed;
            }
            if (barValue > 100){
                EventManager.OnRecover(0);
                bar.value = 100f;
                yield break;
            }
            
            bar.value = barValue;
            yield return null;
        }        
    }
}
