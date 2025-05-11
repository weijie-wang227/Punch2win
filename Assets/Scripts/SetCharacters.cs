using TMPro;
using UnityEngine;

public class SetCharacters : MonoBehaviour
{
    [SerializeField] Transform playerModel;
    [SerializeField] Transform opponentModel;
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text opponentName;

    
    SkinnedMeshRenderer playerGlovesSkin;
    SkinnedMeshRenderer playerShortsSkin;
    SkinnedMeshRenderer playerShoesSkin;
    SkinnedMeshRenderer opponentGlovesSkin;
    SkinnedMeshRenderer opponentShortsSkin;
    SkinnedMeshRenderer opponentShoesSkin;

    Material [] glovesMat = new Material[] {null,null};
    Material [] shortsMat = new Material[] {null,null};
    Material [] shoesMat = new Material[] {null,null,null};


    void Start()
    {
        playerGlovesSkin = playerModel.GetChild(2).GetComponent<SkinnedMeshRenderer>();
        playerShortsSkin = playerModel.GetChild(3).GetComponent<SkinnedMeshRenderer>();
        playerShoesSkin = playerModel.GetChild(4).GetComponent<SkinnedMeshRenderer>();

        glovesMat[0] = EquipStorage.glovesMaterials[PlayerManager.skin[0]["gloves"]];
        glovesMat[1] = EquipStorage.glovesMaterials[PlayerManager.skin[0]["gloves"]];
        playerGlovesSkin.materials = glovesMat;

        shortsMat[0] = EquipStorage.shortsMaterials[PlayerManager.skin[0]["shorts"]];
        shortsMat[1] = EquipStorage.shortsMaterials[PlayerManager.skin[0]["shorts"]];
        playerShortsSkin.materials = shortsMat;

        shoesMat[0] = EquipStorage.shoesMaterials[PlayerManager.skin[0]["shoes"]];
        shoesMat[1] = EquipStorage.shoesMaterials[PlayerManager.skin[0]["shoes"]];
        shoesMat[2] = EquipStorage.shoesMaterials[PlayerManager.skin[0]["shoes"]];
        playerShoesSkin.materials = shoesMat;
        
        if (opponentModel != null) {
            opponentGlovesSkin = opponentModel.GetChild(2).GetComponent<SkinnedMeshRenderer>();
            opponentShortsSkin = opponentModel.GetChild(3).GetComponent<SkinnedMeshRenderer>();
            opponentShoesSkin = opponentModel.GetChild(4).GetComponent<SkinnedMeshRenderer>();

            glovesMat[0] = EquipStorage.glovesMaterials[PlayerManager.skin[1]["gloves"]];
            glovesMat[1] = EquipStorage.glovesMaterials[PlayerManager.skin[1]["gloves"]];
            opponentGlovesSkin.materials = glovesMat;

            shortsMat[0] = EquipStorage.shortsMaterials[PlayerManager.skin[1]["shorts"]];
            shortsMat[1] = EquipStorage.shortsMaterials[PlayerManager.skin[1]["shorts"]];
            opponentShortsSkin.materials = shortsMat;

            shoesMat[0] = EquipStorage.shoesMaterials[PlayerManager.skin[1]["shoes"]];
            shoesMat[1] = EquipStorage.shoesMaterials[PlayerManager.skin[1]["shoes"]];
            shoesMat[2] = EquipStorage.shoesMaterials[PlayerManager.skin[1]["shoes"]];
            opponentShoesSkin.materials = shoesMat;
        }

        if (playerName != null && opponentName != null) {
            playerName.text = PlayerManager.names[0];
            opponentName.text = PlayerManager.names[1];
        }
        
    }
}
