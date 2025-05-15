using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MouseOver : MonoBehaviour
{
    public string punchType = "js";
    private bool canUpdate = true;
    private CharacterMotor player;
    [SerializeField] private GameObject middle;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject down;
    Transform middleTransform, leftTransform, rightTransform, downTransform;
    Slider middleMask, leftMask, rightMask, downMask;

    void Start()
    {
        middleTransform = middle.transform;
        leftTransform = left.transform;
        rightTransform = right.transform;
        downTransform = down.transform;

        middleMask = middleTransform.GetComponent<Slider>();
        leftMask = leftTransform.GetComponent<Slider>();
        rightMask = rightTransform.GetComponent<Slider>();
        downMask = downTransform.GetComponent<Slider>();
        player = GetComponent<CharacterMotor>();
    }

    void Update()
    {
        if (punchType == "js")
        {
            middleMask.value = 1 - ((float)player.stamina) / ((float)PunchData.stamina["2"]);
            middleTransform.position = Input.mousePosition;
        }
        else if (punchType == "lh")
        {
            leftMask.value = 1 - ((float)player.stamina) / ((float)PunchData.stamina["3"]);
            leftTransform.position = new Vector3(leftTransform.position.x, Input.mousePosition.y, leftTransform.position.z);
        }
        else if (punchType == "rh")
        {
            rightMask.value = 1 - ((float)player.stamina) / ((float)PunchData.stamina["4"]);
            rightTransform.position = new Vector3(rightTransform.position.x, Input.mousePosition.y, rightTransform.position.z);
        }
        else if (punchType == "uc")
        {
            downMask.value = 1 - ((float)player.stamina) / ((float)PunchData.stamina["6"]);
            downTransform.position = new Vector3(Input.mousePosition.x, downTransform.position.y, downTransform.position.z);
        }
    }
    void OnEnable()
    {
        EventManager.RoundStart += EventManagerRoundStart;
        EventManager.KnockDown += EventManagerKnockDown;
        EventManager.RoundEnd += RemoveDisplay;
        EventManager.Recover += EventManagerRecover;
    }

    void OnDisable()
    {
        EventManager.RoundStart -= EventManagerRoundStart;
        EventManager.KnockDown -= EventManagerKnockDown;
        EventManager.RoundEnd -= RemoveDisplay;
        EventManager.Recover -= EventManagerRecover;
    }

    void EventManagerRoundStart()
    {
        RemoveDisplay();
        StartCoroutine(ShowDisplay(2.0f));
    }

    void EventManagerKnockDown(int _, bool _b)
    {
        RemoveDisplay();
    }

    void EventManagerRecover(int _)
    {
        StartCoroutine(ShowDisplay(2.0f));
    }


    private IEnumerator ShowDisplay(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        canUpdate = true;
        UpdatePunch(punchType);
    }

    public void RemoveDisplay()
    {
        left.SetActive(false);
        right.SetActive(false);
        down.SetActive(false);
        middle.SetActive(false);
        canUpdate = false;
    }

    public void Straight()
    {
        UpdatePunch("js");
    }
    public void LeftHook()
    {
        UpdatePunch("lh");
    }
    public void RightHook()
    {
        UpdatePunch("rh");
    }
    public void UpperCut()
    {
        UpdatePunch("uc");
    }
    public void UpdatePunch(string type)
    {
        if (canUpdate)
        {
            punchType = type;
            RemoveDisplay();
            canUpdate = true;
            if (punchType == "js")
            {
                middle.SetActive(true);
            }
            else if (punchType == "lh")
            {
                left.SetActive(true);
            }
            else if (punchType == "rh")
            {
                right.SetActive(true);
            }
            else
            {
                down.SetActive(true);
            }
        }
    }

}
