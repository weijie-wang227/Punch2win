using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private int id;
    private Animator animator;
    private TwoBoneIKConstraint armRig;
    private MultiAimConstraint rigDir;
    private MultiAimConstraint hookRig;
    private TwoBoneIKConstraint leftArm;
    private TwoBoneIKConstraint rightArm;
    private MultiAimConstraint leftDir;
    private MultiAimConstraint rightDir;
    private MultiAimConstraint leftUpperArm;
    private MultiAimConstraint rightUpperArm;
    private MultiAimConstraint headDir;
    private MultiAimConstraint bodyDir;
    private Transform punchTarget;
    private Transform headTarget;
    private Transform bodyTarget;
    private float punchTimer = 0f;
    private float hitTimer = 0f;
    public bool isBlocked = false;
    public bool isSquatting = false;
    public bool isSlipL = false;
    public bool isSlipR = false;
    private Transform leftHand;
    private Transform rightHand;
    private Coroutine lastCoroutine;
    private Dictionary<string, Transform> hintGuide = new Dictionary<string, Transform>();
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        Transform rig = Functional.FindChild(transform, "Rig");

        Transform left = rig.Find("LeftArmIK");
        leftArm = left.GetComponent<TwoBoneIKConstraint>();
        leftDir = left.GetComponent<MultiAimConstraint>();
        Transform right = rig.Find("RightArmIK");
        rightArm = right.GetComponent<TwoBoneIKConstraint>();
        rightDir = right.GetComponent<MultiAimConstraint>();

        leftUpperArm = rig.Find("LeftUpperArmIK").GetComponent<MultiAimConstraint>();
        rightUpperArm = rig.Find("RightUpperArmIK").GetComponent<MultiAimConstraint>();

        headDir = rig.Find("HeadDir").GetComponent<MultiAimConstraint>();
        bodyDir = rig.Find("BodyDir").GetComponent<MultiAimConstraint>();

        Transform tempTargets = transform.Find("TargetParent");
        punchTarget = tempTargets.Find("Target");
        headTarget = tempTargets.Find("HeadTarget");
        bodyTarget = tempTargets.Find("BodyTarget");

        Transform tempGuide = transform.Find("HintGuides");
        if (tempGuide == null)
        {
            Debug.Log("HintGuides not found");
            Debug.Log(transform.parent.name);
        }
        if (tempGuide.childCount < 2)
        {
            Debug.Log("HintGuides does not child");
        }

        hintGuide.Add("left", tempGuide.GetChild(0));
        hintGuide.Add("right", tempGuide.GetChild(1));
        hintGuide.Add("down", tempGuide.GetChild(2));

        armRig = rightArm;
        hookRig = rightUpperArm;
        rigDir = rightDir;

        leftHand = Functional.FindChild(transform, "Hand.L");
        rightHand = Functional.FindChild(transform, "Hand.R");

    }

    // Update is called once per frame
    void Update()
    {
        if (isSquatting)
        {
            animator.SetBool("isSquatting", true);
        }
        else
        {
            animator.SetBool("isSquatting", false);
        }

        if (isSlipL)
        {
            animator.SetBool("isSlipL", true);
        }
        else
        {
            animator.SetBool("isSlipL", false);
        }

        if (isSlipR)
        {
            animator.SetBool("isSlipR", true);
        }
        else
        {
            animator.SetBool("isSlipR", false);
        }
    }

    void OnEnable()
    {
        EventManager.KnockDown += EventManagerKnockDown; ;
        EventManager.Recover += EventManagerRecover;
        EventManager.Clinch += EventManagerClinch;
        EventManager.Break += EventManagerBreak;
    }
    void OnDisable()
    {
        EventManager.KnockDown -= EventManagerKnockDown; ;
        EventManager.Recover -= EventManagerRecover;
        EventManager.Clinch -= EventManagerClinch;
        EventManager.Break -= EventManagerBreak;
    }

    void EventManagerKnockDown(int checkId, bool closeToWall)
    {
        animator.SetBool("clinch", false);
        animator.SetBool("approach", false);
        animator.SetBool("isSquatting", false);
        animator.SetBool("isSlipL", false);
        animator.SetBool("isSlipR", false);

        isSquatting = false;
        isSlipL = false;
        isSlipR = false;

        if (checkId == id)
        {
            if (!closeToWall) { animator.SetBool("isDowned", true); }
            else { animator.SetBool("isDowned2", true); }
        }
    }
    void EventManagerRecover(int checkId)
    {
        if (checkId == id)
        {
            animator.SetBool("isDowned", false);
            animator.SetBool("isDowned2", false);
            isSquatting = false;
        }
    }

    void EventManagerClinch(int checkID)
    {
        if (checkID != id)
        {
            animator.SetBool("clinch", true);
        }
    }

    void EventManagerBreak()
    {
        animator.SetBool("clinch", false);
        animator.SetBool("approach", false);
    }

    public void Move(string direction)
    {
        animator.SetTrigger(direction);
    }

    public void Punch(string punchtype, float punchDuration, Vector3 target)
    {
        animator.SetTrigger(punchtype);
        if (target == Vector3.zero)
        {
            return;
        }
        if (punchtype == "1" || punchtype == "2")
        {
            if (punchtype == "1")
            {
                punchTarget.position = target + (target - leftHand.position).normalized * 100; ;
            }
            else
            {
                punchTarget.position = target + (target - rightHand.position).normalized * 100;
            }
            lastCoroutine = StartCoroutine(StraightPunches(punchtype, punchDuration));
        }
        else if (punchtype == "3" || punchtype == "4")
        {
            punchTarget.position = target;
            lastCoroutine = StartCoroutine(Hooks(punchtype, punchDuration));
        }
        else
        {
            punchTarget.position = target;
            lastCoroutine = StartCoroutine(UpperCuts(punchtype, punchDuration));
        }
    }

    IEnumerator StraightPunches(string type, float duration)
    {
        if (type == "1")
        {
            armRig = leftArm;
            rigDir = leftDir;

        }
        else if (type == "2")
        {
            armRig = rightArm;
            rigDir = rightDir;
        }
        else
        {
            yield break;
        }

        while (punchTimer < duration / 2)
        {
            yield return new WaitForFixedUpdate();
            armRig.weight = rigDir.weight = Mathf.SmoothStep(0, 1, punchTimer / (duration / 2));
            punchTimer += Time.fixedDeltaTime;
        }

        while (punchTimer < duration)
        {
            yield return new WaitForFixedUpdate();
            armRig.weight = rigDir.weight = Mathf.SmoothStep(1, 0, (punchTimer / (duration / 2) - 1));
            punchTimer += Time.fixedDeltaTime;
        }
        armRig.weight = 0;
        rigDir.weight = 0;
        punchTimer = 0;
        lastCoroutine = null;
    }

    IEnumerator Hooks(string type, float duration)
    {

        if (type == "3")
        {
            hookRig = leftUpperArm;
            punchTarget.position = new Vector3(hintGuide["left"].position.x, Mathf.Min(punchTarget.position.y, hintGuide["left"].position.y), hintGuide["left"].position.z);
        }
        else if (type == "4")
        {
            hookRig = rightUpperArm;
            punchTarget.position = new Vector3(hintGuide["right"].position.x, Mathf.Min(punchTarget.position.y, hintGuide["right"].position.y), hintGuide["right"].position.z);
        }
        else
        {
            yield break;
        }

        hookRig.data.constrainedXAxis = false;
        hookRig.data.constrainedZAxis = true;

        hookRig.weight = 0.0f;

        if (type == "4")
        {
            while (punchTimer < duration / 2)
            {
                yield return new WaitForFixedUpdate();
                hookRig.weight = Mathf.SmoothStep(0, 1, punchTimer / (duration / 2));
                punchTimer += Time.fixedDeltaTime;
            }

            float temp = punchTimer;

            while (punchTimer < duration)
            {
                yield return new WaitForFixedUpdate();
                hookRig.weight = Mathf.SmoothStep(1, 0, 2 * ((punchTimer - temp) / (duration - temp)));
                punchTimer += Time.fixedDeltaTime;
            }
        }
        else
        {
            while (punchTimer < duration / 4)
            {
                yield return new WaitForFixedUpdate();
                hookRig.weight = Mathf.SmoothStep(0, 1, punchTimer / (duration / 2));
                punchTimer += Time.fixedDeltaTime;
            }

            float temp = punchTimer;

            while (punchTimer < duration)
            {
                yield return new WaitForFixedUpdate();
                hookRig.weight = Mathf.SmoothStep(1, 0, 2 * ((punchTimer - temp) / (duration - temp)));
                punchTimer += Time.fixedDeltaTime;
            }
        }

        hookRig.weight = 0f;

        punchTimer = 0;
        lastCoroutine = null;
    }

    IEnumerator UpperCuts(string type, float duration)
    {
        if (type == "5")
        {
            hookRig = leftUpperArm;
        }
        else if (type == "6")
        {
            hookRig = rightUpperArm;
        }
        else
        {
            yield break;
        }

        hookRig.data.constrainedZAxis = false;
        hookRig.data.constrainedXAxis = true;

        punchTarget.position = new Vector3(punchTarget.position.x, hintGuide["down"].position.y, hintGuide["down"].position.z);

        hookRig.weight = 0.0f;


        while (punchTimer < duration / 4)
        {
            yield return new WaitForFixedUpdate();
            hookRig.weight = Mathf.SmoothStep(0, 1, punchTimer / (duration / 4));
            punchTimer += Time.fixedDeltaTime;
        }
        hookRig.weight = 1.0f;
        yield return new WaitForSeconds(duration * 1 / 2);
        float temp = punchTimer;

        while (punchTimer < duration)
        {
            yield return new WaitForFixedUpdate();
            hookRig.weight = Mathf.SmoothStep(1, 0, 2 * ((punchTimer - temp) / (duration - temp)));
            punchTimer += Time.fixedDeltaTime;
        }

        hookRig.weight = 0f;

        punchTimer = 0;
        lastCoroutine = null;
        punchTimer = 0;
        lastCoroutine = null;
    }

    public void CancelPunch()
    {
        if (lastCoroutine != null)
        {
            StopCoroutine(lastCoroutine);
        }
        if (!isBlocked)
        {
            StartCoroutine(PunchBack());
        }
    }

    IEnumerator PunchBack()
    {
        isBlocked = true;
        animator.SetFloat("statespeed", 0);
        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("block");

        float rigWeight = armRig.weight;
        float hookRigWeight = hookRig.weight;
        float duration = 0.2f;
        punchTimer = 0;
        while (punchTimer < duration)
        {
            yield return new WaitForFixedUpdate();
            armRig.weight = rigDir.weight = Mathf.SmoothStep(rigWeight, 0, punchTimer / duration);
            hookRig.weight = Mathf.SmoothStep(hookRigWeight, 0, punchTimer / duration);
            punchTimer += Time.fixedDeltaTime;
        }
        armRig.weight = hookRig.weight = rigDir.weight = 0;
        punchTimer = 0;
        yield return new WaitForSeconds(0.05f);
        animator.SetFloat("statespeed", 1);
        isBlocked = false;
    }

    public IEnumerator GetHit(Vector3 direction, bool isHead, float hitDelay)
    {
        if (isHead)
        {
            headTarget.position = direction;
            while (hitTimer < hitDelay / 5)
            {
                yield return new WaitForFixedUpdate();
                headDir.weight = Mathf.SmoothStep(0.0f, 0.5f, hitTimer / (hitDelay / 5));
                hitTimer += Time.fixedDeltaTime;
            }
            while (hitTimer < hitDelay)
            {
                yield return new WaitForFixedUpdate();
                headDir.weight = Mathf.SmoothStep(0.5f, 0.0f, (hitTimer - hitDelay * 1 / 5) * 4 / 5);
                hitTimer += Time.fixedDeltaTime;
            }
            headDir.weight = 0f;
            hitTimer = 0f;
        }
        else
        {
            bodyTarget.position = direction;
            while (hitTimer < hitDelay / 5)
            {
                yield return new WaitForFixedUpdate();
                bodyDir.weight = Mathf.SmoothStep(0.0f, 0.2f, hitTimer / (hitDelay / 5));
                hitTimer += Time.fixedDeltaTime;
            }
            while (hitTimer < hitDelay)
            {
                yield return new WaitForFixedUpdate();
                bodyDir.weight = Mathf.SmoothStep(0.2f, 0.0f, (hitTimer - hitDelay * 1 / 5) * 5 / 4);
                hitTimer += Time.fixedDeltaTime;
            }
            bodyDir.weight = 0f;
            hitTimer = 0f;
        }
    }

    public void AttemptClinch()
    {
        animator.SetBool("approach", true);
    }

    public void CancelApproach()
    {
        animator.SetBool("approach", false);
    }
}
