using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class CustomOpp : MonoBehaviour
{
    // Start is called before the first frame update
    float height = 190f;
    int damagebonus = 0;
    int healthbonus = 100;
    float staminabonus = 1f;
    float aggressiveness = 0.5f;
    float skill = 0.5f;
    float difficulty;
    string oppName = "opponent";
    [SerializeField] Slider heightSlider;
    [SerializeField] Slider staminaSlider;
    [SerializeField] Slider aggroSlider;
    [SerializeField] Slider skillSlider;

    [SerializeField] TMP_Text heightText;
    [SerializeField] TMP_Text damageText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text staminaText;
    [SerializeField] TMP_Text aggroText;
    [SerializeField] TMP_Text skillText;
    [SerializeField] TMP_Text diffText;
    [SerializeField] TMP_Text nameText;

    [SerializeField] SkinnedMeshRenderer glovesSkin;
    [SerializeField] SkinnedMeshRenderer shortsSkin;
    [SerializeField] SkinnedMeshRenderer shoesSkin;

    Material [] glovesMat = new Material[] {null,null};
    Material [] shortsMat = new Material[] {null,null};
    Material [] shoesMat = new Material[] {null,null,null};

    private Color displayColor = Color.white;

    [SerializeField] Material skin;

    List<string> nameList;

    void Awake () {
        string readFromFilePath = Application.streamingAssetsPath + "/BoxerNames.txt";
        nameList = File.ReadAllLines(readFromFilePath).ToList();

        UpdateDifficulty();
    }

    public void UpdateHeight () {
        height = heightSlider.value;
        heightText.text = "Height: " + ((int)height).ToString() + "cm";
        UpdateDifficulty();
    }

    public void PlusDamage () {
        damagebonus = Mathf.Min(3, damagebonus+1);
        if (damagebonus >0) {
            damageText.text = "Damage\n +" + damagebonus.ToString();
        }
        else {
            damageText.text = "Damage\n" + damagebonus.ToString();
        }
        
        UpdateDifficulty();
    }
    public void MinusDamage () {
        damagebonus = Mathf.Max(0, damagebonus-1);
        if (damagebonus>0) {
            damageText.text = "Damage\n +" + damagebonus.ToString();
        }
        else {
            damageText.text = "Damage\n" + damagebonus.ToString();
        }
        UpdateDifficulty();
    }

    public void PlusHealth () {
        healthbonus = Mathf.Min(130,healthbonus+10);
        healthText.text = "Health\n" + healthbonus.ToString();
        UpdateDifficulty();
    }

    public void MinusHealth () {
        healthbonus = Mathf.Max(70,healthbonus-10);
        healthText.text = "Health\n" + healthbonus.ToString();
        UpdateDifficulty();
    }

    public void UpdateStamina () {
        staminabonus = staminaSlider.value;
        staminaText.text = "Stamina: x" + staminabonus.ToString("n1");
        UpdateDifficulty();
    }

    public void UpdateAggro () {
        aggressiveness = aggroSlider.value;
        aggroText.text = "Aggressiveness:\nx" + aggressiveness.ToString("n1");
        UpdateDifficulty();
    }

    public void UpdateSkill () {
        skill = skillSlider.value;
        skillText.text = "Skill: " + skill.ToString("n1");
        UpdateDifficulty();
    }

    void UpdateDifficulty () {
        difficulty = (height-170f)/20 + damagebonus + (healthbonus-70)/60 + (staminabonus-0.5f) + aggressiveness/2 + skill;
        if (difficulty < 0.5) {
            diffText.text = "Difficulty: Very Easy";
        }
        else if (difficulty < 1) {
            diffText.text = "Difficulty: Easy";
        }
        else if (difficulty < 3) {
            diffText.text = "Difficulty: Medium";
        }
        else if (difficulty < 5) {
            diffText.text = "Difficulty: Hard";
        }
        else if (difficulty < 7) {
            diffText.text = "Difficulty: Expert";
        }
        else {
            diffText.text = "Difficulty: Impossible";
        }

        PlayerManager.bonus[1]["height"] = height;
        PlayerManager.bonus[1]["damage"] = damagebonus;
        PlayerManager.maxHealth[1] = healthbonus;
        PlayerManager.bonus[1]["stamina"] = staminabonus;
        PlayerManager.bonus[1]["aggressiveness"] = aggressiveness;
        PlayerManager.bonus[1]["skill"] = skill;
        PlayerManager.names[1] = oppName;
        
    }

    public void RandomizeSkin () {
        int glovesId = Random.Range(0,EquipStorage.glovesMaterials.Count);
        int shortsId = Random.Range(0,EquipStorage.shoesMaterials.Count);
        int shoesId = Random.Range(0,EquipStorage.shortsMaterials.Count);
        
        PlayerManager.skin[1]["gloves"] = glovesId;
        glovesMat[0] = EquipStorage.glovesMaterials[glovesId];
        glovesMat[1] = EquipStorage.glovesMaterials[glovesId];
        glovesSkin.materials = glovesMat;

        PlayerManager.skin[1]["shorts"] = shortsId;
        shortsMat[0] = EquipStorage.shortsMaterials[shortsId];
        shortsMat[1] = EquipStorage.shortsMaterials[shortsId];
        shortsSkin.materials = shortsMat;

        PlayerManager.skin[1]["shoes"] = shoesId;
        shoesMat[0] = EquipStorage.shoesMaterials[shoesId];
        shoesMat[1] = EquipStorage.shoesMaterials[shoesId];
        shoesMat[2] = EquipStorage.shoesMaterials[shoesId];
        shoesSkin.materials = shoesMat;


        displayColor = Color.Lerp(EquipStorage.startColor, EquipStorage.endColor,Random.value);
        skin.color = displayColor;
        oppName = nameList[Random.Range(0,nameList.Count)];
        nameText.text = oppName;
        PlayerManager.names[1] = oppName;
    }

}
