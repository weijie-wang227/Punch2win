using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Functional
{

    // Update is called once per frame
    public static Transform FindChild(Transform parent, string childName){
        Transform current = parent;
        Transform child;
        child = current.Find(childName);
        if (child != null){
            return child;
        }
        else {
            foreach (Transform i in parent){
                Transform k = FindChild(i, childName);
                if (k != null){
                    return k;
                }
            }
            return null;
        }
    }

    public static Transform FindParent(Transform child, string parentName){
        Transform current = child;
        while (current != null)
            if (current.name == parentName){
                return current;
            }
            current = current.parent;

        return null;
    }

    public static GameObject FindCharacter(Transform child){
        Transform current = child;
        int counter = 0;
        while (current != null && counter < 50)
            if (current.tag == "Player"){
                return current.gameObject;
            }
            current = current.parent;
            counter ++;
        Debug.Log("Find fail");
        return null;
    }
}
