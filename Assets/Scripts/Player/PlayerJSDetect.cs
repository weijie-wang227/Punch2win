using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerJSDetect : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] PlayerController player;
    public void OnPointerClick (PointerEventData data) {
        if (data.button == PointerEventData.InputButton.Left){
            player.Jab();
        }
        else if (data.button == PointerEventData.InputButton.Right){
            player.Straight();
        }
    }
}
