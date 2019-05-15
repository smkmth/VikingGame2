using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private DialogueDisplayer dialogueDisplay;
    public LayerMask EnemyLayerMask;
    public float AttackDistance;
    private Inventory inventory;
    public EquipmentHolder equipmentHolder;
    public bool canInteract=true;
    public enum interactionState
    {
        Normal,
        DialogueMode,
        InventoryMode
    }
    public interactionState currentInteractionState;
    public List<DialogueLine> receivedDialogue;
    public int dialogueIndex;

    public float StartRotateSpeed;
    public float MaxRotateSpeed;
    public float Step;
    private float rot;

    public bool FreeLook =true;
    public bool FreeMove =true;

    public float MovementSpeed;
    public float DistanceToGround;


    private Vector3 movepos;

    private float currentMoveSpeed;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
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

                if (FreeMove)
                {
                    if (Input.GetAxis("Vertical") != 0)
                    {

                        transform.position += movepos + transform.forward * Input.GetAxis("Vertical") * MovementSpeed * Time.deltaTime;

                    }
                    if (Input.GetAxis("Horizontal") != 0)
                    {

                        transform.position += movepos + transform.right * Input.GetAxis("Horizontal") * MovementSpeed * Time.deltaTime;

                    }
                }

                if (FreeLook)
                {
                    if (Input.GetAxis("Mouse X") != 0)
                    {

                        rot = Mathf.Lerp(StartRotateSpeed, MaxRotateSpeed, Step * Time.deltaTime);
                    }
                    transform.Rotate(0, rot * Input.GetAxis("Mouse X"), 0);
                }

               
                if (Input.GetButtonDown("Fire1"))
                {
                    RaycastHit hit;
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100.0f, Color.yellow);
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
                    {

                        if (hit.transform.gameObject.tag == "NPC")
                        {
                            receivedDialogue = hit.transform.gameObject.GetComponent<DialogueContainer>().dialogue;
                            currentInteractionState = interactionState.DialogueMode;
                            dialogueIndex = 0;
                            return;
                        }
                        if (hit.transform.gameObject.tag == "Enemy")
                        {
                            if (equipmentHolder.equipedWeapon != null)
                            {
                                hit.transform.gameObject.GetComponent<Stats>().DoDamage(equipmentHolder.equipedWeapon.attackDamage);

                                Debug.Log("ATTACK!");
                            }
                            else
                            {
                                Debug.Log("No Weapon Equiped");
                            }


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
