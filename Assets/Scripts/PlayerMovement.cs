using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Movement speed")]
    public float speed;
    public float fastRunSpeed;
    public float sprintCostStamina;
    public float riftCostStamina;
    private Rigidbody rb;
    [Header("Movement States")]
    public bool moving;
    public bool rifting;
    public bool grounded;
    public bool sprinting;
    public bool healing;
    [Header("Is player grounded ray cast start positions")]
    public Transform[] cheks = new Transform[4];
    [Header("Healing")]
    public int healCount;
    public int healPoints;
    public GameObject healingDonut;
    public Transform leftHandArmature;
    private int currenHealCount;

    /* animation */
    [Header("Animations")]
    public Animation anim;
    public AnimationClip ainm_idle;
    public AnimationClip anim_run;
    public AnimationClip anim_rift;
    public AnimationClip anim_fastRun;
    public AnimationClip anim_death;
    public AnimationClip anim_falling;
    public AnimationClip anim_healing;
    /*  */

    /* falling */
    [Header("Absolute values for counting falling damage")]
    public float absUnits = 10;
    public float absFallTime = 1.39f;
    private float fallCounter = 0;
    /* */

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!GetComponent<Player>().IsDead())
        {
            if (moving && !rifting && !sprinting)
                anim.CrossFade(anim_run.name);
            else if (moving && !rifting && sprinting)
                anim.CrossFade(anim_fastRun.name);
            else if (rifting)
                anim.CrossFade(anim_rift.name);
            else anim.CrossFade(ainm_idle.name);

            /*if (Input.GetKeyUp(KeyCode.Space))
            {
                GetComponent<Player>().currentStamina -= riftCostStamina;
                rifting = true;
            }*/

            Falling();

            if (Input.GetKeyUp(KeyCode.Q) && !rifting && grounded)
            {
                healing = true;
                moving = false;
            }
            if (healing)
                Healing();
        }
    }

    void FixedUpdate()
    {
        IsGrounded();
        if (!GetComponent<Player>().IsDead() && grounded && !healing)
            Move();
    }

    void IsGrounded()
    {
        /*if (Physics.Raycast(groundRightLeg.position, Vector3.down, 0.5f) || Physics.Raycast(groundLeftLeg.position, Vector3.down, 0.5f))
            grounded = true;
        else grounded = false;*/

        int j = 0;
        for(int i = 0; i < cheks.Length; i++)
        {
            if (Physics.Raycast(cheks[i].position, Vector3.down, 0.5f))
                j++;
        }

        if (j > 1)
            grounded = true;
        else grounded = false;
    }

    void Move()
    {
        if(Input.GetKeyDown(KeyCode.W))
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);//ставим камеру за персонажа

        if (Input.GetKey(KeyCode.W) && !rifting)
        {
            moving = true;
            float hozizontal = Input.GetAxis("Mouse X") * Camera.main.GetComponent<SoulsCamera>().speedX * Time.deltaTime;
            transform.Rotate(0, hozizontal, 0);
            //rb.MovePosition(rb.position + inputs * Time.fixedDeltaTime * speed);
            //rb.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
            /*
            if (Input.GetKey(KeyCode.A))
            {
                rb.transform.Rotate(Vector3.up, -90 * Time.fixedDeltaTime * 2.5f);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rb.transform.Rotate(Vector3.up, 90 * Time.fixedDeltaTime * 2.5f);
            }
            */
            if (Input.GetKey(KeyCode.LeftShift) && GetComponent<Player>().currentStamina - sprintCostStamina * Time.deltaTime >= 0)
            {
                GetComponent<Player>().currentStamina -= sprintCostStamina * Time.deltaTime;
                sprinting = true;
                rb.MovePosition(rb.position + rb.transform.forward * fastRunSpeed * Time.fixedDeltaTime);
            }
            else
            {
                sprinting = false;
                rb.MovePosition(rb.position + rb.transform.forward * speed * Time.fixedDeltaTime);
            }
        }
        else moving = false;

        if (Input.GetKey(KeyCode.A) && !moving && !rifting)
        {
            rb.transform.Rotate(Vector3.up, -90 * Time.fixedDeltaTime * 4f);
        }
        else if (Input.GetKey(KeyCode.D) && !moving && !rifting)
        {
            rb.transform.Rotate(Vector3.up, 90 * Time.fixedDeltaTime * 4f);
        }

        if (Input.GetKeyUp(KeyCode.Space) && GetComponent<Player>().currentStamina - riftCostStamina >= 0 && !rifting)
        {
            GetComponent<Player>().currentStamina -= riftCostStamina;
            rifting = true;
        }

        if (rifting)
        {
            if (anim[anim_rift.name].time < 0.90f * anim[anim_rift.name].length)
            {
                rb.MovePosition(rb.position + rb.transform.forward * speed * Time.fixedDeltaTime * 2.5f);
            }
            else rifting = false;
        }
    }

    void Falling()
    {
        /* 10 единиц (absUnits) по у падает за 1,39 секунды (absFallTime) */
        /* пускай за падение с высоты в 10 единиц игрок получает урон равный 1/3 от максимального (damagePerHeightUnit) */
        /* пускай игрок будет получать урон только, если падает больше эталонного времени падения 10 единиц */
        float damagePerHeightUnit = (GetComponent<Player>().maxHealth / 3) / absUnits;
        float unitsPerFalling;
        float fallDamage = 0;

        if (!grounded)
        {
            anim.CrossFade(anim_falling.name);
            rifting = false;
            rb.MovePosition(rb.position + rb.transform.forward * speed * Time.fixedDeltaTime);
        }

        if (fallCounter > absFallTime && grounded)
        {
            moving = false;
            unitsPerFalling = (fallCounter * absUnits) / absFallTime;
            fallDamage = unitsPerFalling * damagePerHeightUnit;
            Debug.Log("fall damage for " + (fallCounter) + "sec is " + fallDamage);
            GetComponent<Player>().GetHit((int)fallDamage);
        }

        if (grounded)
            fallCounter = 0;
        else fallCounter += Time.deltaTime;
    }

    public void PlayDeathAnimation()
    {
        anim.CrossFade(anim_death.name);
    }

    void Healing()
    {
        anim.CrossFade(anim_healing.name);
        if(anim[anim_healing.name].time > 0.9 * anim[anim_healing.name].length)
        {
            healing = false;
            moving = false;
        }
    }
}
