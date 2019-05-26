using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum interactionState
{
    Normal,
    DialogueMode,
    InventoryMode
}
public class PlayerInteraction : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera playerCamera;
    private CameraControl camControl;
    private bool isLockedOn;
    private Transform target;
    public float lockOnDamping;

    [Header("Movement Settings")]
    public bool FreeMove;
    private float currentMoveSpeed;
    private Vector3 forwardMovement;
    private Vector3 sidewaysMovement;

    [Header("Rotation Settings")]
    public bool FreeLook;
    public float turnSpeed;

    [Header("Interaction Settings")]
    public float interactRange;
    private DialogueDisplayer dialogueDisplay;
    private List<DialogueLine> receivedDialogue;
    public interactionState currentInteractionState;
    private int dialogueIndex;

    private Combat combat;

    private Inventory inventory;
    private InventoryDisplayer inventoryDisplayer;

    private AnimationManager animator;
 

    private void Start()

    {
        inventoryDisplayer = GetComponent<InventoryDisplayer>();
        animator = GetComponent<AnimationManager>();
        inventory = GetComponent<Inventory>();
        combat = GetComponent<Combat>();
        dialogueDisplay = GetComponent<DialogueDisplayer>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        FreeMove = true;
        FreeLook= true;
        camControl = playerCamera.GetComponent<CameraControl>();

    }

    void SetCursorState(CursorLockMode wantedMode)
    {
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
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
            inventoryDisplayer.ToggleInventoryMenu();
        }
        switch (currentInteractionState) 
        {

            case interactionState.Normal :
                if (Input.GetButtonDown("LockOn"))
                {
                    if (isLockedOn)
                    {
                        isLockedOn = false;
                        camControl.ToggleLockOn(false);


                    }
                    else
                    {
                        isLockedOn = true;
                        camControl.ToggleLockOn(true);


                    }
                }
                if (isLockedOn)
                {
                    var lookPos = target.position - transform.position;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * lockOnDamping);

                }

                if (FreeLook)
                {
                    
                    //  only check on the X-Z plane:
                    Vector3 cameraDirection = new Vector3(playerCamera.transform.forward.x, 0f, playerCamera.transform.forward.z);
                    Vector3 playerDirection = new Vector3(transform.forward.x, 0f, transform.forward.z);
                    if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
                    {
                        if (Vector3.Angle(cameraDirection, playerDirection) > 15f)
                        {
                            Quaternion targetRotation = Quaternion.LookRotation(cameraDirection, transform.up);

                            transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation, turnSpeed * Time.deltaTime);
                                    
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
