using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    private CharacterMotor playerMotor;
    private CharacterMotor motor;
    private float maxHealth;
    private float distance;
    private Coroutine dodgeRoutine;
    private Coroutine punchRoutine;
    private Coroutine uncornerRoutine;
    private Transform headTarget;
    private Transform bodyTarget;
    private Vector3 headAim;
    private Vector3 bodyAim;
    [SerializeField] private float aggression = 0.8f;
    [SerializeField] private float skill = 1f;
    private float moveTimer = 0.0f;
    private float retreatTimer = 0.0f;
    //private float clinchTimer = 0.0f;
    private float resetAimTimer = 0.0f;
    private float baseAimDelay = 0.15f;
    private float moveDelay = 0.5f;
    [SerializeField] private float clinchFactor;
    [SerializeField] private float retreatFactor;
    [SerializeField] private Transform center;
    private float centerDistance;
    private float scale;

    // Start is called before the first frame update
    void Start()
    {
        scale = PlayerManager.bonus[1]["height"] / 190f;
        aggression = PlayerManager.bonus[1]["aggressiveness"];
        skill = PlayerManager.bonus[1]["aggressiveness"];

        player = GameObject.Find("Player").transform;
        playerMotor = player.GetComponent<CharacterMotor>();
        motor = GetComponent<CharacterMotor>();
        headTarget = Functional.FindChild(player, "HeadHurt");
        bodyTarget = Functional.FindChild(player, "BodyHurt");
        maxHealth = PlayerManager.maxHealth[1];
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(player.position, center.position - player.position);
        centerDistance = Vector3.Cross(ray.direction, transform.position - ray.origin).magnitude;
        float angle = Vector3.Angle(transform.position - center.position, transform.TransformDirection(Vector3.right));
        bool closer = (transform.position - center.position).magnitude - (player.position - center.position).magnitude > 0 ? false : true;

        distance = PlayerManager.distance() / scale;
        if (distance > 115)
        {
            motor.StartMove("forward");
        }
        /** else if (motor.stamina < 20 && distance < 60 && Time.time > clinchTimer){
            clinchTimer = Time.time + moveDelay;
            if (Random.value < aggression * clinchFactor * (1 - (motor.health / maxHealth))) {
                motor.AttemptClinch();
            }
        } **/
        else if (motor.stamina < 20 && distance < 85 && Time.time > retreatTimer)
        {
            retreatTimer = Time.time + moveDelay;
            if (Random.value > aggression * retreatFactor)
            {
                motor.StartMove("backward");
            }
        }
        else if (distance > 85 && Time.time > moveTimer)
        {
            moveTimer = Time.time + moveDelay;
            if ((Random.value < aggression * (motor.stamina + 100) / 200) || (playerMotor.isHurt && distance > 60))
            {
                motor.StartMove("forward", true);
            }
        }
        else if ((closer && centerDistance > 20f) || (!closer) && Time.time > moveTimer)
        {
            moveTimer = Time.time + moveDelay;
            if (Random.value < (skill) / 50)
                if (angle > 90)
                {
                    motor.StartMove("right");
                }
                else
                {
                    motor.StartMove("left");
                }
        }

        if (PlayerManager.isPunchOut[0] && dodgeRoutine == null && distance < 90 && punchRoutine == null)
        {
            dodgeRoutine = StartCoroutine(Dodge());
        }

        if (motor.stamina > 0 && punchRoutine == null && distance < 85 && dodgeRoutine == null)
        {
            punchRoutine = StartCoroutine(Punch());
        }

        if (Time.time > resetAimTimer)
        {
            resetAimTimer = Time.time + baseAimDelay + 0.25f * (1 - skill);
            headAim = headTarget.position;
            bodyAim = bodyTarget.position;
        }
    }

    void OnEnable()
    {
        EventManager.KnockDown += EventManagerKnockDown;
        EventManager.RoundEnd += EventManagerRoundEnd;
        EventManager.Clinch += EventManagerClinch;
    }

    void OnDisable()
    {
        EventManager.KnockDown -= EventManagerKnockDown;
        EventManager.RoundEnd -= EventManagerRoundEnd;
        EventManager.Clinch -= EventManagerClinch;
    }

    void EventManagerKnockDown(int checkId, bool _m)
    {
        if (punchRoutine != null)
        {
            StopCoroutine(punchRoutine);
            punchRoutine = null;
        }
        if (checkId != 0)
        {
            StartCoroutine(AttemptRecover(checkId));
        }
    }

    void EventManagerRoundEnd()
    {
        if (punchRoutine != null)
        {
            StopCoroutine(punchRoutine);
            punchRoutine = null;
        }
    }

    void EventManagerClinch(int _)
    {
        if (punchRoutine != null)
        {
            StopCoroutine(punchRoutine);
            punchRoutine = null;
        }
    }

    private IEnumerator AttemptRecover(int checkId)
    {
        float baseChance = 0.3f;
        float chance = baseChance / ((float)PlayerManager.knockDownCounter[checkId]);
        yield return new WaitForSeconds(1.0f);
        for (int i = 10; i > 1; i--)
        {
            yield return new WaitForSeconds(1.0f);
            if (Random.value < chance)
            {
                EventManager.OnRecover(checkId);
                yield break;
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Wall" && uncornerRoutine == null && this.enabled)
        {
            uncornerRoutine = StartCoroutine(Uncorner(hit.normal));
        }


    }

    private IEnumerator Uncorner(Vector3 normal)
    {
        string direction;
        float angle = Vector3.Angle(normal, transform.TransformDirection(Vector3.right));
        if (angle > 90)
        {
            direction = "left";
        }
        else
        {
            direction = "right";
        }

        for (int i = 0; i < 7; i++)
        {
            motor.StartMove(direction, true);
            yield return new WaitForSeconds(motor.moveInterval);
        }
        uncornerRoutine = null;
    }

    private IEnumerator Dodge()
    {
        if (Random.value < skill * 0.75)
        {
            yield return new WaitForSeconds(0.5f);
            dodgeRoutine = null;
            yield break;
        }

        if (Random.value > 0.33)
        {
            motor.StartSlip('L');
        }
        else if (Random.value > 0.5)
        {
            motor.StartSlip('R');
        }
        else
        {
            motor.StartSquat();
        }
        yield return new WaitForSeconds(0.5f);
        motor.EndSlip();
        motor.EndSquat();
        dodgeRoutine = null;

    }

    private IEnumerator Punch()
    {
        if (Random.value > skill && !playerMotor.isHurt)
        {
            yield return new WaitForSeconds(0.5f);
        }

        List<string> combo = Combos[Random.Range(0, Combos.Count)];

        foreach (string action in combo)
        {
            if (action == "slipL")
            {
                motor.StartSlip('L');
                yield return new WaitForSeconds(0.75f);
                motor.EndSlip();
            }
            else if (action == "slipR")
            {
                motor.StartSlip('R');
                yield return new WaitForSeconds(0.75f);
                motor.EndSlip();
            }
            else if (action == "left" || action == "right" || action == "forward" || action == "backward")
            {
                motor.StartMove(action);
            }
            else if (action == "squat")
            {
                motor.StartSquat();
            }
            else if (action == "endsquat")
            {
                motor.EndSquat();
            }
            else
            {
                Vector3 destination;
                if (Random.value < 0.5)
                {
                    destination = headAim + Random.insideUnitSphere * 10f;
                }
                else
                {
                    destination = bodyAim + Random.insideUnitSphere * 10f;
                }
                if (!motor.StartPunch(action, destination))
                {
                    break;
                }
                yield return new WaitForSeconds(PunchData.duration[action]);
            }
        }
        if (Random.value > aggression)
        {
            motor.StartMove("backward");
        }
        motor.EndSquat();
        yield return new WaitForSeconds(0.1f);
        punchRoutine = null;
    }

    private List<List<string>> Combos = new List<List<string>>() {
        new List<string> (){"1"},
        new List<string> (){"1","2"},
        new List<string> (){"1","2","1"},
        new List<string> (){"2","3","2"},
        new List<string> (){"1","slipL","3"},
        new List<string> (){"1", "squat","2","endsquat","forward", "3"},
        new List<string> (){"6", "5", "6"},
        new List<string> (){"1","squat","4"}
    };

}
