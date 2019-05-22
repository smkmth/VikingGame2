﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private Rigidbody rb; 
    private DialogueDisplayer dialogueDisplay;
    public LayerMask EnemyLayerMask;
    public LayerMask GroundLayerMask;
    public LayerMask BlockingLayerMask;
    public float AttackDistance;
    private Inventory inventory;
    public EquipmentHolder equipmentHolder;
    public bool canInteract=true;
    public Camera cam;
    public enum interactionState
    {
        Normal,
        DialogueMode,
        InventoryMode
    }
    public interactionState currentInteractionState;
    public List<DialogueLine> receivedDialogue;
    public int dialogueIndex;

    public float rotationSpeed;
    public float Step;
    private float rot;

    public bool FreeLook =true;
    public bool FreeMove =true;

    public float MovementSpeed;
    public float DistanceToGround;
    public bool onGround;
    public float playerHeight;
    public float gravity;
    public float slopeAngle;
    public bool onSlope =false;
    public float interactRange;
    public bool stardewControls;
    private float currentMoveSpeed;
    public bool controllerInput;
    public Animator animator;
    private Quaternion targetRotation;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        rb = GetComponent<Rigidbody>();
        equipmentHolder = GetComponent<EquipmentHolder>();
        dialogueDisplay = GetComponent<DialogueDisplayer>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    void SetCursorState(CursorLockMode wantedMode)
    {
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }
    public bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, DistanceToGround + 0.1f);
    }
    // Update is called once per frame
    void Update()
    {
       

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCursorState(CursorLockMode.None);
        }

        if (Input.GetButtonDown("Inventory"))
        {
            inventory.ToggleInventoryMenu();
        }
        switch (currentInteractionState) 
        {

            case interactionState.Normal :
                if (!stardewControls)
                {

                    if (FreeLook)
                    {
                        if (controllerInput)
                        {

                            // Vector3 lookDirection = new Vector3(Input.GetAxis("RightStickHorizontal"), 0, Input.GetAxis("RightStickVertical"));
                            /*
                               Vector3 transformedLookDirection = cam.transform.TransformVector(transform.forward);
                               transformedLookDirection.y = 0;
                               if (transformedLookDirection != transform.forward)
                               {
                                   transform.rotation = Quaternion.LookRotation(transformedLookDirection);

                               }
                               */

                            //  only check on the X-Z plane:
                            Vector3 cameraDirection = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z);
                            Vector3 playerDirection = new Vector3(transform.forward.x, 0f, transform.forward.z);
                            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
                            {
                                if (Vector3.Angle(cameraDirection, playerDirection) > 15f)
                                {
                                    targetRotation = Quaternion.LookRotation(cameraDirection, transform.up);

                                    transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation, rotationSpeed * Time.deltaTime);
                                    
                                }

                            }


                        }
                    



                    }
                    if (FreeMove)
                    {
                        Vector3 forwardMovement = transform.forward * Input.GetAxis("Vertical");
                        Vector3 sidewaysMovement = transform.right * Input.GetAxis("Horizontal");


                        Vector3 nextMovePos = (forwardMovement + sidewaysMovement) * Time.deltaTime * MovementSpeed;

                        if (nextMovePos.x != 0 && nextMovePos.z != 0)
                        {
                            animator.SetBool("Run", true);
                        }
                        else
                        {
                            animator.SetBool("Run", false);
                        }

                        transform.position += nextMovePos;

                    }

                    
                }
                else
                {
                    float hozMovement = Input.GetAxis("Horizontal");
                    float vertMovement = Input.GetAxis("Vertical");
                    float rawHozMovement = Input.GetAxisRaw("Horizontal");
                    float rawVertMovement = Input.GetAxisRaw("Vertical");

                    Vector3 movement = new Vector3(rawHozMovement, 0.0f, rawVertMovement);
                    if (movement != Vector3.zero)
                    {
                        transform.rotation = Quaternion.LookRotation(movement);

                    }

                    if (rawHozMovement != 0 || rawVertMovement != 0)
                    {
                        animator.SetBool("Run", true);
                    }
                    else
                    {
                        animator.SetBool("Run", false);
                    }



                    transform.Translate(movement * MovementSpeed * Time.deltaTime, Space.World);
                }

               
                if (Input.GetButtonDown("Attack"))
                {
                    RaycastHit hit;
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100.0f, Color.yellow);
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange))
                    {


                        if (hit.transform.gameObject.tag == "Item")
                        {
                            ItemContainer item = hit.transform.gameObject.GetComponent<ItemContainer>();
                            if(item.hitsToHarvest ==0)
                            {
                                inventory.AddItem(item.containedItem);
                                Destroy(hit.transform.gameObject);
                            }
                            else
                            {
                                item.hitsToHarvest -= 1;
                            }
                        }
                        if (hit.transform.gameObject.tag == "Enemy")
                        {
                            if (equipmentHolder.equipedWeapon != null)
                            {
                                hit.transform.gameObject.GetComponent<Stats>().DoDamage(equipmentHolder.equipedWeapon.attackDamage);

                            }
                            else
                            {
                                Debug.Log("No Weapon Equiped");
                            }


                        }

                    }

                   
                }

                if (Input.GetButtonDown("Interact"))
                {
                    Debug.Log("here");
                    RaycastHit interact;
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100.0f, Color.yellow);
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out interact, interactRange))
                    {
                        if (interact.transform.gameObject.tag == "NPC")
                        {
                            Debug.Log("NPC");
                            receivedDialogue = interact.transform.gameObject.GetComponent<DialogueContainer>().dialogue;
                            currentInteractionState = interactionState.DialogueMode;
                            dialogueIndex = 0;
                            return;
                        }
                    }
                }
                break;
            case interactionState.DialogueMode:
               
                dialogueDisplay.DisplayDialogue(receivedDialogue[dialogueIndex]);

                if (Input.GetButtonDown("Attack"))
                {
                    if (dialogueIndex + 1 < receivedDialogue.Count)
                    {
                        dialogueIndex++;
                        dialogueDisplay.isDisplayingDialogue = false;
                    }
                    else
                    {
                        dialogueDisplay.FinishDisplayingDialogue();
                        currentInteractionState = interactionState.Normal;
                    }


                }
                break;
            case interactionState.InventoryMode:
                break;
        }
    }
}
