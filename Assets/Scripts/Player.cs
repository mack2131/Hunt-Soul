using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [Header("Health Settings")]
    public int maxHealth;
    public int currentHealth;
    public Transform healthBarObject;
    [Header("Stamina Settings")]
    public float maxStamina;
    public float currentStamina;
    public float staminaResetTime;
    public Transform staminaBarObject;
    private float staminaResetCounter;
    public Image lungsBar;

    private float maxBonePos = 2.8f;
    private float minBonePos = -0.9f;
    private float healthValue;
    private float staminaValue;

    private GameObject myStomach;

    /* death */
    private bool startDeathAnim;
    private bool endDeathAnim;
    /* */

    // Use this for initialization
    void Start ()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        myStomach = GameObject.FindObjectOfType<HealthStomach>().gameObject;
        //Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (healthBarObject == null)
            healthBarObject = GameObject.Find("Wave Controller Health").gameObject.transform;
        if (staminaBarObject == null)
            staminaBarObject = GameObject.Find("Wave Controller Stamina").gameObject.transform;

        Death();
        ResetStamina();

        ShowCurrentHealth();
        ShowCurrentStamina();

       // Cursor.lockState = CursorLockMode.Locked;
	}

    void ShowCurrentHealth()
    {
        /*healthValue = (maxBonePos - minBonePos) / maxHealth;
        healthBarObject.localPosition = new Vector3(healthBarObject.localPosition.x, minBonePos + healthValue * currentHealth, healthBarObject.localPosition.z);*/

    }

    void ShowCurrentStamina()
    {
        /*staminaValue = (maxBonePos - minBonePos) / maxStamina;
        staminaBarObject.localPosition = new Vector3(staminaBarObject.localPosition.x, minBonePos + staminaValue * currentStamina, staminaBarObject.localPosition.z);*/
        lungsBar.fillAmount = 1 / maxStamina * currentStamina;

    }

    public bool IsDead()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            return true;
        }
        else return false;
    }

    public void GetHit(int damage)
    {
        currentHealth -= damage;
        myStomach.GetComponent<HealthStomach>().IWasteMyFood(damage);
    }

    void Death()
    {
        if(IsDead() && !endDeathAnim)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GetComponent<PlayerMovement>().PlayDeathAnimation();
            if (GetComponent<PlayerMovement>().anim[GetComponent<PlayerMovement>().anim_death.name].time > 0.9f * GetComponent<PlayerMovement>().anim[GetComponent<PlayerMovement>().anim_death.name].length)
                endDeathAnim = true;
        }
    }

    void ResetStamina()
    {
        if (currentStamina < maxStamina)
        {
            float staminaPerSecond = maxStamina / staminaResetTime;
            staminaResetCounter += Time.deltaTime;
            if (staminaResetCounter >= 1)
            {
                currentStamina += staminaPerSecond;
                staminaResetCounter = 0;
            }
        }
        else if (currentStamina > maxStamina)
            currentStamina = maxStamina;
        else if (currentStamina <= 0)
            currentStamina = 0;
    }

    public void GetHeal(int heal)
    {
        currentHealth += heal;
        GetComponent<HealthStomach>().IWantToEat();
    }
}
