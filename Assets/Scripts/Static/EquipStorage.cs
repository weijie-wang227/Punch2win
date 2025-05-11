using UnityEngine;
using System.Collections.Generic;

public class EquipStorage : MonoBehaviour
{
    public static EquipStorage instance;
 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        startColor = _startColor;
        endColor = _endColor;
        glovesMaterials = _glovesMaterials;
        shortsMaterials = _shortsMaterials;
        shoesMaterials = _shoesMaterials;
    }

    public static Color startColor;
    public static Color endColor;
    
    public static List<Material> glovesMaterials = new List<Material> ();
    public static List<Material> shortsMaterials = new List<Material> ();
    public static List<Material> shoesMaterials = new List<Material> ();

    [SerializeField] private  Color _startColor;
    [SerializeField] private  Color _endColor;
    
    [SerializeField] private  List<Material> _glovesMaterials = new List<Material> ();
    [SerializeField] private  List<Material> _shortsMaterials = new List<Material> ();
    [SerializeField] private  List<Material> _shoesMaterials = new List<Material> ();

}


