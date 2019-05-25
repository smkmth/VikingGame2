using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{


    private AnimationManager animator;
    private EquipmentHolder equipmentHolder;
    private WeaponHit weapon;
    public float attackTime;
    public float anticipationTime;
    public int MaxHealth;
    public int Health;
    private Rigidbody rb;
    public Vector3 startPos;
    public float staggeredRecoveryTime;
    private float staggerTimer;
    public effectType currentEffect = effectType.nothing;
    public bool isBlocking;
    public bool attacking;
    public bool FreeCombat = true;
    public float dodgeDistance;
    public float dodgeSpeed;
    private bool dodging;
    private Vector3 dodgeVect;
    private Vector3 dodgePos;
    public int poise;
    public int maxPoise;
    private float poiseTimer;
    public float currentMovementSpeed;

    public float normalMovementSpeed;
    public float slowedMovementSpeed;


    private void Start()
    {
        currentMovementSpeed = normalMovementSpeed;
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
            currentMovementSpeed = slowedMovementSpeed;


        }
        else
        {
            if (isBlocking)
            {
                isBlocking = false;
                animator.SetBool("Block", false);
                currentMovementSpeed = normalMovementSpeed;

            }


        }

    }

    public void Dodge(Vector3 forwardMovement, Vector3 sidewaysMovement)
    {

        if (!dodging)
        {

            dodgePos = (forwardMovement + sidewaysMovement * dodgeDistance + transform.position);
            dodgeVect = forwardMovement + sidewaysMovement;
            dodging = true;

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

        attacking = true;
        currentMovementSpeed = 0.0f;
        animator.SetBool("ReadyAttack", true);
        yield return new WaitForSeconds(anticipationTime);
        animator.SetTrigger("Attack");
        weapon.doDamage = true;
        yield return new WaitForSeconds(attackTime);
        weapon.doDamage = false;
        attacking = false;
        currentMovementSpeed = normalMovementSpeed;
    }


    public void StopAttack()
    {
        if (attacking)
        {

            StopCoroutine(DoAttack());
            animator.SetTrigger("Interrupt");
            animator.SetBool("ReadyAttack", false);
            weapon.doDamage = false;
            attacking = false;
            currentMovementSpeed = normalMovementSpeed;
        }


    }

    public void TakeDamage(int amount, float force, Combat enemy)
    {
        if (!isBlocking)
        {
            Health -= amount;
            rb.AddForce(-transform.forward * force, ForceMode.Impulse);
            StopAttack();

        
            if (Health <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            poise -= amount;
            if (poise <= 0)
            {
                Stagger();
            }
            else
            {
                enemy.Stagger();
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

        if (dodging)
        {
            if (dodgeVect != Vector3.zero)
            {
                if (Mathf.Abs(Vector3.Distance(transform.position, dodgePos)) > dodgeDistance)
                {
                    dodging = false;
                    dodgePos = Vector3.zero;
                }
                else
                {
                    transform.position += dodgeVect * Time.deltaTime * dodgeSpeed;
                }

            }
            else
            {

                dodging = false;
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
                if (staggerTimer >= staggeredRecoveryTime)
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


