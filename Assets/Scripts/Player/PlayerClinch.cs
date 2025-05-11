using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClinch : MonoBehaviour
{
    GameObject button;

    // Start is called before the first frame update
    /**void Start()
    {
        button = transform.Find("Clinch").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.distance() < 60 && !button.activeInHierarchy) {
            button.SetActive(true);
        }
        else if (PlayerManager.distance() > 60) {
            button.SetActive(false);
        }
    }**/
}
