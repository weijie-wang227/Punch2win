using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBox : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int id;
    private string checkTag;

    void Start() {
        checkTag = id == 0 ? "Opponent" : "Player";
    }
    

    
    void OnTriggerEnter(Collider other){
        if (other.tag == checkTag && PlayerManager.isPunchOut[id] && !PlayerManager.isBlocked[id]){
            EventManager.OnBlock(id);
        }
    }
}
