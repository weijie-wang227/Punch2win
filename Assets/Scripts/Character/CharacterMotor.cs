using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterMotor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int id;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float runSpeed = 20f;
    public int health = 100;
    public int stamina = 100;
    [SerializeField] private float staminaDelay = 0.2f;
    private float nextStamina = 0.0f;
    [SerializeField] public float moveInterval = 0.5f;
    [SerializeField] private float punchDelay = 0.2f;
    [SerializeField] private float moveDelay = 0.2f;
    private CharacterAnimator animator;
    private CharacterController characterController;
    private bool isMoving = false;
    private bool isPunching = false;
    public bool isPunchOut = false;
    public bool isHit = false;
    private float hitDelay = 0.5f;
    private float hitTimer = 0f;
    public string punchType;
    [SerializeField] private bool isDown = false;
    private bool roundOver = false;
    [SerializeField] private List<Collider> walls;
    [SerializeField] private Transform wallDetect;
    private float maxHealth;
    private bool inClinch = false;
    private Coroutine clinchRoutine;
    public bool isHurt = false;
    private float critChance = 0.02f;
    private Coroutine moveRoutine;


    void Start()
    {
        transform.localScale = transform.localScale * PlayerManager.bonus[id]["height"] / 190f;

        characterController = GetComponent<CharacterController>();
        characterController.enableOverlapRecovery = true;
        animator = GetComponent<CharacterAnimator>();
        maxHealth = PlayerManager.maxHealth[id];
        staminaDelay = staminaDelay / PlayerManager.bonus[id]["stamina"];
    }

    // Update is called once per frame
    void Update()
    {
        PlayerManager.position[id] = transform.position;
        if (stamina < 100 && Time.time > nextStamina && !isHurt)
        {
            nextStamina = Time.time + staminaDelay * (0.5f + ((maxHealth - health) / (maxHealth * 2)));
            stamina += 1;
        }

        if (isHurt)
        {
            stamina = 0;
        }
        if (health < 0 && !isDown)
        {
            isDown = true;
            PlayerManager.isDown[id] = true;
            PlayerManager.knockDownCounter[id] += 1;
            bool skip = false;
            foreach (Collider wall in walls)
            {
                if (wall.bounds.Contains(wallDetect.position))
                {
                    EventManager.OnKnockDown(id, true);
                    skip = true;
                    break;
                }
            }
            if (!skip)
            {
                EventManager.OnKnockDown(id, false);
            }

        }
    }

    private void OnEnable()
    {
        EventManager.Block += EventManagerBlock;
        EventManager.TakeHit += EventManagerTakeHit;
        EventManager.RoundStart += EventManagerRoundStart;
        EventManager.RoundEnd += EventManagerRoundEnd;
        EventManager.KnockDown += EventManagerKnockDown;
        EventManager.Recover += EventManagerRecover;
        EventManager.Clinch += EventManagerClinch;
        EventManager.Break += EventManagerBreak;
        EventManager.ClinchPunch += EventManagerClinchPunch;
    }
    private void OnDisable()
    {
        EventManager.Block -= EventManagerBlock;
        EventManager.TakeHit -= EventManagerTakeHit;
        EventManager.RoundStart -= EventManagerRoundStart;
        EventManager.RoundEnd -= EventManagerRoundEnd;
        EventManager.KnockDown -= EventManagerKnockDown;
        EventManager.Recover -= EventManagerRecover;
        EventManager.Clinch += EventManagerClinch;
        EventManager.Break -= EventManagerBreak;
        EventManager.ClinchPunch -= EventManagerClinchPunch;
    }

    private void EventManagerBlock(int checkID)
    {
        if (id == checkID)
        {
            animator.CancelPunch();
            StartCoroutine(Block());
        }
    }

    private IEnumerator Block()
    {
        yield return new WaitForSeconds(0.1f);
        isPunchOut = false;
        PlayerManager.isPunchOut[id] = false;
    }

    private void EventManagerTakeHit(int checkID, int damage, Vector3 direction, bool isHead)
    {
        if (checkID == id) { StartCoroutine(TakeHit(damage, direction, isHead)); }
    }

    private void EventManagerRoundStart()
    {
        PlayerManager.isDown[id] = false;
        PlayerManager.isPunching[id] = false;
        if (RoundData.counter == 1)
        {
            health = PlayerManager.maxHealth[id];
        }
        else
        {
            StartCoroutine(HealthRegen(10));
        }
        roundOver = false;
    }
    private void EventManagerRoundEnd()
    {
        PlayerManager.health[id] = health;
        roundOver = true;
        if (clinchRoutine != null)
        {
            StopCoroutine(clinchRoutine);
        }

    }

    private void EventManagerKnockDown(int checkId, bool _)
    {
        StopCoroutine(moveRoutine);
        if (checkId != id)
        {
            StartCoroutine(MoveBack());
        }
    }

    private IEnumerator MoveBack()
    {
        float movingTimer = 0f;
        while (movingTimer < 2f)
        {
            StartMove("backward");
            movingTimer += Time.deltaTime;
            yield return null;
        }
    }

    private void EventManagerRecover(int checkId)
    {
        PlayerManager.health[id] = health;
        if (checkId == id)
        {
            StartCoroutine(HealthRegen(40 / PlayerManager.knockDownCounter[id]));
        }
    }

    private void EventManagerClinch(int _)
    {
        EndSlip();
        StopCoroutine(moveRoutine);
        inClinch = true;
    }

    private void EventManagerBreak()
    {
        inClinch = false;
        StartCoroutine(Break());
    }

    private void EventManagerClinchPunch(int checkID)
    {
        if (checkID == id)
        {
            StartPunch("6", Vector3.zero);
        }
        else
        {
            StartCoroutine(GetHit());
        }
    }

    private IEnumerator GetHit()
    {
        yield return new WaitForSeconds(0.2f);
        Vector3 direction = PlayerManager.position[id == 0 ? 1 : 0] - transform.position;
        EventManager.OnTakeHit(id, PunchData.damage["6"], direction, false);
    }

    private IEnumerator HealthRegen(int healthAdd)
    {
        int oldHealth = PlayerManager.health[id];
        int newHealth = Mathf.Min(PlayerManager.maxHealth[id], PlayerManager.health[id] + healthAdd);
        float interval = 2.0f;
        float inttimer = 0f;
        while (inttimer < interval)
        {
            health = (int)Mathf.Lerp(oldHealth, newHealth, inttimer / interval);
            inttimer += Time.deltaTime;
            yield return null;
        }
        isDown = false;
        PlayerManager.isDown[id] = false;
    }

    public bool StartMove(string movedirection, bool isRunning = false)
    {
        if (!isMoving && !isDown && !roundOver)
        {
            if (!isMoving && stamina > 10 && isRunning)
            {
                moveRoutine = StartCoroutine(Move(movedirection, true));
                stamina -= 10;
                return true;
            }
            else if (!isMoving)
            {
                moveRoutine = StartCoroutine(Move(movedirection, false));
                return true;
            }
        }
        return false;
    }

    private IEnumerator Move(string direction, bool isRunning)
    {
        isMoving = true;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float moveTimer = 0f;
        Vector3 directionVector;
        Vector3 velocity;


        if (direction == "right")
        {
            directionVector = right;
        }
        else if (direction == "left")
        {
            directionVector = -right;
        }
        else if (direction == "forward")
        {
            directionVector = forward;
        }
        else
        {
            directionVector = -forward;
        }
        velocity = (isRunning ? runSpeed : walkSpeed) * directionVector.normalized;

        animator.Move(direction);

        while (moveTimer < moveInterval)
        {
            characterController.Move(velocity * Time.deltaTime);
            moveTimer += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(moveDelay);
        isMoving = false;
    }

    public bool StartPunch(string punchtype, Vector3 target)
    {
        if (!isPunching && !animator.isSlipL && !animator.isSlipR && stamina > PunchData.stamina[punchtype] && !isHit)
        {
            punchType = punchtype;
            PlayerManager.punchType[id] = punchtype;
            StartCoroutine(Punch(punchtype, target));
            return true;
        }
        return false;
    }

    private IEnumerator Punch(string punchtype, Vector3 target)
    {
        isPunching = true;
        PlayerManager.isPunching[id] = true;
        stamina -= PunchData.stamina[punchtype];
        animator.Punch(punchtype, PunchData.duration[punchtype], target);

        yield return new WaitForSeconds(PunchData.timeSplit[punchType][0]);
        isPunchOut = true;
        PlayerManager.isPunchOut[id] = true;

        yield return new WaitForSeconds(PunchData.timeSplit[punchType][1]);
        isPunchOut = false;
        PlayerManager.isPunchOut[id] = false;

        yield return new WaitForSeconds(PunchData.timeSplit[punchtype][2] + punchDelay);
        isPunching = false;
        PlayerManager.isPunching[id] = false;
    }


    public IEnumerator TakeHit(int damage, Vector3 direction, bool isHead = false)
    {
        health -= damage;
        isHit = true;
        if (Random.value < critChance)
        {
            isHurt = true;
            StartCoroutine(Hurt());
        }
        StartCoroutine(animator.GetHit(direction, isHead, hitDelay));
        if (!inClinch)
        {
            while (hitTimer < hitDelay / 4)
            {
                characterController.Move(new Vector3(direction.x, 0, direction.z).normalized * runSpeed * Time.deltaTime);
                hitTimer += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(hitDelay * 3 / 4);
        }
        hitTimer = 0;
        isHit = false;
    }

    private IEnumerator Hurt()
    {
        yield return new WaitForSeconds(5.0f);
        isHurt = false;
    }

    public void StartSquat()
    {
        animator.isSquatting = true;
    }
    public void EndSquat()
    {
        animator.isSquatting = false;
    }

    public void StartSlip(char side)
    {
        if (!isPunching)
        {
            if (side == 'L')
            {
                animator.isSlipL = true;
                animator.isSlipR = false;
            }
            else
            {
                animator.isSlipR = true;
                animator.isSlipL = false;
            }
        }
    }

    public void EndSlip()
    {
        animator.isSlipL = animator.isSlipR = false;
    }

    public void AttemptClinch()
    {
        if (!roundOver)
        {
            clinchRoutine = StartCoroutine(Approach());
        }

    }

    private IEnumerator Approach()
    {
        StartMove("forward", true);
        animator.AttemptClinch();
        float approachTimer = 0f;
        while (approachTimer < 0.3f)
        {
            if (isHit)
            {
                animator.CancelApproach();
                clinchRoutine = null;
                yield break;
            }
            if (PlayerManager.distance() > 30f)
            {
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 velocity = forward * runSpeed;
                characterController.Move(velocity * Time.deltaTime);
            }

            approachTimer += Time.deltaTime;
            yield return null;
        }
        if (PlayerManager.distance() < 30f)
        {
            EventManager.OnClinch(id);
            clinchRoutine = null;
        }
        else
        {
            animator.CancelApproach();
            clinchRoutine = null;
        }

    }

    private IEnumerator Break()
    {
        float temp = 0f;
        while (temp < 0.5f)
        {
            characterController.Move(-transform.TransformDirection(Vector3.forward) * runSpeed * Time.deltaTime);
            temp += Time.deltaTime;
            yield return null;
        }

    }

}
