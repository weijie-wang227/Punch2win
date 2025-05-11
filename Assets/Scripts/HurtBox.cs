using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private bool isHead;
    [SerializeField]private int enemyId;
    private int damage;
    private string checkTag;

    void Start () {
        checkTag = id == 0 ? "Opponent" : "Player";
    }

    void OnTriggerEnter (Collider other){
        
        if (checkTag == other.tag && PlayerManager.isPunchOut[enemyId] && !RoundData.inClinch){
            Vector3 direction;
            if (isHead || other.transform.position.y > transform.position.y){
                direction = transform.position - other.transform.position;
            }
            else {
                direction = other.transform.position - transform.position;
            }
            direction = new Vector3(direction.x, 0 , direction.z).normalized * 50.0f;
            direction += transform.position;
            damage = (isHead ? 2 : 1) * PunchData.damage[PlayerManager.punchType[enemyId]] + (int)PlayerManager.bonus[enemyId]["damage"];
            EventManager.OnTakeHit(id,damage,direction,isHead);
        }
    }
}
