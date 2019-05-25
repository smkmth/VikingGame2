using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Rigidbody rb;
    private Combat combat;
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
    public bool FreeMove;
    public bool FreeLook;

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
    public float dodgeDistance;
    public bool dodging;
    private AnimationManager animator;
    private Quaternion targetRotation;
    public float dodgeSpeed;
    Vector3 dodgePos;
    Vector3 dodgeVect;
    Vector3 forwardMovement;
    Vector3 sidewaysMovement;
    public bool isLockedOn;
    public Transform target;
    public float Damping;
    private bool attacking;
    public WeaponHit weapon;
    public float anticipationTime;
    public float attackTime;

    private void Start()
    {
        animator = GetComponent<AnimationManager>();
        inventory = GetComponent<Inventory>();
        combat = GetComponent<Combat>();
        rb = GetComponent<Rigidbody>();
        combat = GetComponent<Combat>();
        equipmentHolder = GetComponent<EquipmentHolder>();
        dialogueDisplay = GetComponent<DialogueDisplayer>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        FreeMove = true;
        FreeLook= true;

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
                if (Input.GetButtonDown("LockOn"))
                {
                    if (isLockedOn)
                    {
                        isLockedOn = false;
                        Camera.main.GetComponent<CameraControl>().ToggleLockOn(false);


                    }
                    else
                    {
                        isLockedOn = true;
                        Camera.main.GetComponent<CameraControl>().ToggleLockOn(true);


                    }
                }
                if (isLockedOn)
                {
                    var lookPos = target.position - transform.position;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Damping);

                }

                if (FreeLook)
                {
                    if (controllerInput)
                    {
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
                    forwardMovement = transform.forward * Input.GetAxis("Vertical");
                    sidewaysMovement = transform.right * Input.GetAxis("Horizontal");


                    Vector3 nextMovePos = (forwardMovement + sidewaysMovement) * Time.deltaTime * combat.currentMovementSpeed;

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

                if (Input.GetButtonDown("Dodge"))
                {
                    combat.Dodge(forwardMovement, sidewaysMovement);
                }

                if (Input.GetButtonDown("Attack"))
                {
                    combat.Attack();
                }
                if (Input.GetButton("Block"))
                {
                    combat.Block(true);
                }
                else
                {
                    combat.Block(false);
                }

                if (Input.GetButtonDown("Interact"))
                {
                    RaycastHit interact;
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100.0f, Color.yellow);
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out interact, interactRange))
                    {


                        if (interact.transform.gameObject.tag == "Item")
                        {
                            ItemContainer item = interact.transform.gameObject.GetComponent<ItemContainer>();
                            if (item.hitsToHarvest == 0)
                            {
                                inventory.AddItem(item.containedItem);
                                Destroy(interact.transform.gameObject);
                            }
                            else
                            {
                                item.hitsToHarvest -= 1;
                            }
                        }
                        if (interact.transform.gameObject.tag == "NPC")
                        {
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

                if (Input.GetButtonDown("Interact"))
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
