using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerUCDetect : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] PlayerController player;
    public void OnPointerClick (PointerEventData data) {
        if (data.button == PointerEventData.InputButton.Left){
            player.LeftUpperCut();
        }
        else if (data.button == PointerEventData.InputButton.Right){
            player.RightUpperCut();
        }
    }
}
