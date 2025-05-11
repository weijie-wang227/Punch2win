using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreatePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SkinnedMeshRenderer glovesSkin;
    [SerializeField] SkinnedMeshRenderer shortsSkin;
    [SerializeField] SkinnedMeshRenderer shoesSkin;
    [SerializeField] List <Image> frames;
    [SerializeField] TMP_InputField playerName;

    Material [] glovesMat;
    Material [] shortsMat;
    Material [] shoesMat;
    void Start () {
        glovesMat = glovesSkin.materials;
        shortsMat = shortsSkin.materials;
        shoesMat = shoesSkin.materials;        
    }
    
    public void SetGloves1 () {
        PlayerManager.skin[0]["gloves"] = 0;
        glovesMat[0] = EquipStorage.glovesMaterials[0];
        glovesMat[1] = EquipStorage.glovesMaterials[0];
        glovesSkin.materials = glovesMat;
        frames[0].enabled = true;
        frames[1].enabled = false;
    }
    public void SetGloves2 () {
        PlayerManager.skin[0]["gloves"] = 1;
        glovesMat[0] = EquipStorage.glovesMaterials[1];
        glovesMat[1] = EquipStorage.glovesMaterials[1];
        glovesSkin.materials = glovesMat;
        frames[1].enabled = true;
        frames[0].enabled = false;
    }
    public void SetShorts1 () {
        PlayerManager.skin[0]["shorts"] = 0;
        shortsMat[0] = EquipStorage.shortsMaterials[0];
        shortsMat[1] = EquipStorage.shortsMaterials[0];
        shortsSkin.materials = shortsMat;
        frames[2].enabled = true;
        frames[3].enabled = false;
    }
    public void SetShorts2 () {
        PlayerManager.skin[0]["shorts"] = 1;
        shortsMat[0] = EquipStorage.shortsMaterials[1];
        shortsMat[1] = EquipStorage.shortsMaterials[1];
        shortsSkin.materials = shortsMat;
        frames[3].enabled = true;
        frames[2].enabled = false;
    }
    public void SetShoes1 () {
        PlayerManager.skin[0]["shoes"] = 0;
        shoesMat[0] = EquipStorage.shoesMaterials[0];
        shoesMat[1] = EquipStorage.shoesMaterials[0];
        shoesMat[2] = EquipStorage.shoesMaterials[0];
        shoesSkin.materials = shoesMat;
        frames[4].enabled = true;
        frames[5].enabled = false;
    }
    public void SetShoes2 () {
        PlayerManager.skin[0]["shoes"] = 1;
        shoesMat[0] = EquipStorage.shoesMaterials[1];
        shoesMat[1] = EquipStorage.shoesMaterials[1];
        shoesMat[2] = EquipStorage.shoesMaterials[1];
        shoesSkin.materials = shoesMat;
        frames[5].enabled = true;
        frames[4].enabled = false;
    }

    public void SetName () {
        PlayerManager.names[0] = playerName.text;
    }
}
