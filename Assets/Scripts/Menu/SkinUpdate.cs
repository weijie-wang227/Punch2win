using UnityEngine;
using UnityEngine.UI;

public class SkinUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Material skin;
    [SerializeField] private Slider slider;
    private Image image;
    private Color displayColor = Color.white;
    
    void Start () {
        displayColor = EquipStorage.startColor;
        skin.color = EquipStorage.startColor;
        image = GetComponent<Image>();
    }

    public void UpdateSkin () {
        displayColor = Color.Lerp(EquipStorage.startColor, EquipStorage.endColor,slider.value);
        skin.color = displayColor;
        image.color = displayColor;
        
    }
}
