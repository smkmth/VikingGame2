using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{

    //tooltips to the thing below for inspector layout

    private AnimationManager animator;
    private EquipmentHolder equipmentHolder;
    private Rigidbody rb;
    private WeaponHit weapon;

    [HideInInspector]
    public Vector3 startPos;

    [Header("Health Settings")]                                         [Tooltip("Max Health - How much damage character can take")]
    public int MaxHealth;                                               [Tooltip("Current Health - DONT CHANGE THIS - Set to Max Health value on start up - in inspector for debug reasons")]
    public int Health;                                                  [Tooltip("Current Effect - DONT CHANGE THIS - Any damage effects applied to player are set here")]
    public effectType currentEffect = effectType.nothing;

    [Header("Attack Settings")]
    [Tooltip("Free Combat - Can the player currently fight?")]
    public bool FreeCombat = true;
    [Tooltip("Base Attack Time - How long in seconds will the players weapon do damage?")]
    public float baseAttackTime;
    [Tooltip("Base Anticipation Time - How long in seconds should the animation wait before attacking")]
    public float baseAnticipationTime;
    [Tooltip("Base Stagger Recovery Time - How long in seconds will it take for the character to recover from Stagger?")]
    public float baseStaggerRecoveryTime;
    private float staggerTimer;
    [HideInInspector]
    public bool isAttacking;                                             //is the player currently attacking?

    [Header("Dodge Settings")]
    [Tooltip("Dodge Distance - How far will a dodge take you?")]
    public float dodgeDistance;
    [Tooltip("Dodge Speed - How quickly will you move when dodging?")]
    public float dodgeSpeed;
    private Vector3 dodgeVect;                                          //the direction to dodge to 
    private Vector3 dodgePos;                                           //the position marking the end of a dodge
    [HideInInspector]
    public bool isDodging;                                              //is the player currenly dodging?   

    [Header("Defence Settings")]
    [Tooltip("Max Poise - How much damage can you block before a shield break?")]
    public int maxPoise;
    private int currentPoise;                                           //current amount of poise
    private float poiseTimer;
    [HideInInspector]
    public bool isBlocking;

    [Header("Movement Settings")]
    [Tooltip("Normal Movement Speed - how fast you normally move without any modifiers")]
    public float baseMovementSpeed;
    [Tooltip("Blocking Movement Speed- how fast you move while blocking")]
    public float blockingMovementSpeed;
    [HideInInspector]
    public float currentMovementSpeed;                                  //the players current movement speed
    GameObject sceneManager;


    private void Start()
    {
        sceneManager = GameObject.Find("SceneManager");
        currentMovementSpeed = baseMovementSpeed;
        equipmentHolder = GetComponent<EquipmentHolder>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<AnimationManager>();
        weapon = GetComponentInChildren<WeaponHit>();
        Health = MaxHealth;
        startPos = transform.position;

    }
    public void Block(bool block)
    {
        if (block)
        {

            isBlocking = true;
            animator.SetBool("Block", true);
            currentMovementSpeed = blockingMovementSpeed;


        }
        else
        {
            if (isBlocking)
            {
                isBlocking = false;
                animator.SetBool("Block", false);
                currentMovementSpeed = baseMovementSpeed;

            }


        }

    }

    public void Dodge(Vector3 forwardMovement, Vector3 sidewaysMovement)
    {

        if (!isDodging)
        {

            dodgePos = (forwardMovement + sidewaysMovement * dodgeDistance + transform.position);
            dodgeVect = forwardMovement + sidewaysMovement;
            isDodging = true;

        }
    }


    public void Attack()
    {
        if (FreeCombat)
        {

            if (equipmentHolder.equipedWeapon != null)
            {

                StopCoroutine(DoAttack());
                StartCoroutine(DoAttack());

            }
            else
            {
                Debug.Log("No Weapon Equiped");
            }
        }

    }

    IEnumerator DoAttack()
    {

        isAttacking = true;
        currentMovementSpeed = 0.0f;
        animator.SetBool("ReadyAttack", true);
        yield return new WaitForSeconds(baseAnticipationTime);
        animator.SetTrigger("Attack");
        weapon.doDamage = true;
        yield return new WaitForSeconds(baseAttackTime);
        weapon.doDamage = false;
        isAttacking = false;
        currentMovementSpeed = baseMovementSpeed;
    }


    public void StopAttack()
    {
        if (isAttacking)
        {

            StopCoroutine(DoAttack());
            animator.SetTrigger("Interrupt");
            animator.SetBool("ReadyAttack", false);
            weapon.doDamage = false;
            isAttacking = false;
            currentMovementSpeed = baseMovementSpeed;
        }


    }

    public void TakeDamage(int amount, float force)
    {
        if (!isBlocking)
        {
            Health -= amount;
            rb.AddForce(-transform.forward * force, ForceMode.Impulse);
            StopAttack();

        
            if (Health <= 0)
            {
                Arrow[] arrows = GetComponentsInChildren<Arrow>();
                foreach (Arrow arrow in arrows)
                {
                    arrow.gameObject.transform.parent = sceneManager.transform;
                    arrow.gameObject.SetActive(false);

                }
                Destroy(gameObject);
            }
        }
        else
        {
            currentPoise -= amount;
            if (currentPoise <= 0)
            {
                Stagger();
            }

        }


    }


    private void Stagger()
    {
        if (currentEffect != effectType.staggered)
        {
            currentEffect = effectType.staggered;
            staggerTimer = 0.0f;
            FreeCombat = false;

            animator.SetBool("ReadyAttack", false);

            animator.SetBool("Stagger", true);

        }
        

    }
    private void Update()
    {

        if (isDodging)
        {
            if (dodgeVect != Vector3.zero)
            {
                if (Mathf.Abs(Vector3.Distance(transform.position, dodgePos)) > dodgeDistance)
                {
                    isDodging = false;
                    dodgePos = Vector3.zero;
                }
                else
                {
                    transform.position += dodgeVect * Time.deltaTime * dodgeSpeed;
                }

            }
            else
            {

                isDodging = false;
                dodgePos = Vector3.zero;

            }
        }
        switch (currentEffect)
        {
            case effectType.bleeding:
                break;
            case effectType.instaKill:
                break;
            case effectType.poison:
                break;
            case effectType.slowed:
                break;
            case effectType.staggered:
                if (staggerTimer >= baseStaggerRecoveryTime)
                {
  
                    FreeCombat = true;
                    animator.SetBool("Stagger", false);

                    currentEffect = effectType.nothing;
                    staggerTimer = 0.0f;
                }
                else
                {
                    staggerTimer += Time.deltaTime;
                }
                break;

        }
    }

}


