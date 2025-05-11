using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject target;
    [SerializeField] private int id;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.isPunching[id] || !PlayerManager.isDown[id]){
            Vector3 direction = (target.transform.position - transform.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0f,direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
