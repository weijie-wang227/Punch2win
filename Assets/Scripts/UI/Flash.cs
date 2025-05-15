using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;
public class Flash : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    private TMP_Text text;
    private float duration = 2f;
    private float timer;
    private bool isFlashing = true;
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > duration)
        {
            timer = 0;
        }

        if (isFlashing)
        {
            if (timer < duration / 2)
            {
                float frac = timer / (3 * duration);
                text.color = new Color(frac, frac, frac, 1.0f);
            }
            else
            {
                float frac = (1 - timer / duration) / 3;
                text.color = new Color(frac, frac, frac, 1.0f);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isFlashing = false;
        text.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isFlashing = true;
    }
}
