using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum interactionState
{
    Normal,
    DialogueMode,
    InventoryMode,
    CraftingMode,
    Aiming
}
public class PlayerInteraction : MonoBehaviour
{
    public TimeManager time;

    [Header("Camera Settings")]
    public Camera playerCamera;
    private CameraControl camControl;
    private bool isLockedOn;
    private Transform target;
    public float lockOnDamping;

    [Header("Movement Settings")]
    public bool FreeMove;
    private float currentMoveSpeed;
    public float aimmoveSpeed;
    private Vector3 forwardMovement;
    private Vector3 sidewaysMovement;

    [Header("Rotation Settings")]
    public bool FreeLook;
    public float turnSpeed;

    [Header("Interaction Settings")]
    public float interactRange;
    private List<DialogueLine> receivedDialogue;
    public interactionState currentInteractionState;
    private int dialogueIndex;


    public InkDisplayer dialogueDisplayer;
    private Combat combat;
    public PlayerHUD hud;
    private Inventory inventory;
    private InventoryDisplayer inventoryDisplayer;
    private AnimationManager animator;

    [Header("Archery Settings")]

    private float noDialogueTimer = 0.0f;
    public float dialogueTimeOut;
    public bool justLeftDialogue;
    public bool canSpeak = true;

    public GameObject crosshair;
    public bool aiming;

    public Transform aimTarget;
  //  public float radius = 2.0f;
  //  public float rotationSpeed = 80.0f;
 //   public float radiusSpeed = .5f;
    public bool bowDrawn = false;
    public GameObject arrowPrefab;
    public Transform bowPosition;
    public Crafter playerCrafter;
    public float shotForce;

    public float bowPowerTimer;
    public float shotHeight;
    public float maxBowPower;
    public float bowPowerModifer;
    public float bowPullBackRate;
    public Item arrow;
    public int arrowDamage;
    public float castRadius;
    public float shotRange;

    public GameObject axe;
    public GameObject fishingRod;

    public CraftingMenu craftingMenu;

    private void Start()
    {
        playerCrafter = GetComponent<Crafter>();
        inventoryDisplayer = GetComponent<InventoryDisplayer>();
        animator = GetComponent<AnimationManager>();
        inventory = GetComponent<Inventory>();
        craftingMenu = GetComponent<CraftingMenu>();
        
        hud = GetComponent<PlayerHUD>();
        combat = GetComponent<Combat>();
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


    public void SetDialogueMode()
    {
        if (currentInteractionState == interactionState.Normal)
        {
            hud.ToggleHUD(true);
            canSpeak = false;
            Cursor.visible = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            currentInteractionState = interactionState.DialogueMode;
        }
        else if (currentInteractionState == interactionState.DialogueMode)
        {
            hud.ToggleHUD(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            currentInteractionState = interactionState.Normal;
            noDialogueTimer = 0.0f;

        }
    }

    void SetInventoryMode(bool toggleOn)
    {
        if (toggleOn)
        {
            hud.ToggleHUD(true);

            inventoryDisplayer.ToggleInventoryMenu(true);
            Cursor.visible = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            currentInteractionState = interactionState.InventoryMode;

        }
        else if (!toggleOn)
        {
            hud.ToggleHUD(false);
            inventoryDisplayer.ToggleInventoryMenu(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            currentInteractionState = interactionState.Normal;


        }
    }


    public void SetCraftingMode(Crafter crafter)
    {
        if (currentInteractionState == interactionState.Normal)
        {
            craftingMenu.ToggleCraftingMenu(true,crafter);
            hud.ToggleHUD(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            currentInteractionState = interactionState.CraftingMode;

        }
        else if (currentInteractionState == interactionState.CraftingMode)
        {
            craftingMenu.ToggleCraftingMenu(false, crafter);
            hud.ToggleHUD(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            currentInteractionState = interactionState.Normal;


        }
    }



    // Update is called once per frame
    void Update()
    {
   

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCursorState(CursorLockMode.None);
        }

        if (Input.GetButtonDown("Crafting"))
        {
            SetCraftingMode(playerCrafter);
        }

        switch (currentInteractionState) 
        {

            case interactionState.Normal :

                if (Input.GetAxisRaw("Aim") == 1)
                {
                    currentInteractionState = interactionState.Aiming;

                }

                if (Input.GetButtonDown("Inventory"))
                {
                    SetInventoryMode(true);
                }


                if (FreeLook)
                {
                    /*
                    
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
                    */

                }
                if (FreeMove)
                {
                    forwardMovement = transform.forward * Input.GetAxis("Vertical");
                    sidewaysMovement = transform.right * Input.GetAxis("Horizontal");
                    float cameraFacing = Camera.main.transform.eulerAngles.y;

                    if (forwardMovement != Vector3.zero || sidewaysMovement != Vector3.zero)
                    {

                        float angle = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg;
                       
                        animator.SetBool("Run", true);
                        
                      
                        transform.rotation = Quaternion.AngleAxis(angle + cameraFacing, Vector3.up);
                        transform.position += transform.forward * 10.0f * Time.deltaTime;
                    }
                    else
                    {
                        animator.SetBool("Run", false);

                    }


                }

                if (Input.GetButtonDown("Dodge"))
                {
                    combat.Dodge(forwardMovement, sidewaysMovement);
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
                                animator.SetTrigger("AxeSwing");
                                item.hitsToHarvest -= 1;
                            }
                        }
                        if (interact.transform.gameObject.tag == "NPC")
                        {
                            if (canSpeak)
                            {

                                noDialogueTimer = 0.0f;
                                interact.transform.gameObject.GetComponent<DialogueContainer>().Talk();
                                SetDialogueMode();
                                return;
                            }

                        }
                        if (interact.transform.gameObject.tag == "Interactable")
                        {
                            Interactable interactable = interact.transform.gameObject.GetComponent<Interactable>();
                            Debug.Log("hit interact");
                            interactable.UseInteractable(gameObject);
                        }
                    }
                }
                if (!canSpeak)
                {
                    if (noDialogueTimer == dialogueTimeOut)
                    {
                        dialogueTimeOut =+ Time.deltaTime;
                    }
                    else
                    {
                        noDialogueTimer = 0;
                        canSpeak = true;
                    }

                }

                break;
            case interactionState.Aiming:

                if (Input.GetAxisRaw("Aim") == 0)
                {

                    bowDrawn = false;
                    aiming = false;
                    FreeMove = true;
                    currentInteractionState = interactionState.Normal;

                }
                else
                {





                    crosshair.transform.LookAt(transform);
                    aiming = true;
                    FreeMove = false;
                    animator.SetBool("Run", false);

                    Vector3 cameraDirection = new Vector3(playerCamera.transform.forward.x, 0f, playerCamera.transform.forward.z);
                    Vector3 playerDirection = new Vector3(transform.forward.x, 0f, transform.forward.z);

                    forwardMovement = transform.forward * Input.GetAxis("Vertical");
                    sidewaysMovement = transform.right * Input.GetAxis("Horizontal");


                    Vector3 nextMovePos = (forwardMovement + sidewaysMovement) * Time.deltaTime * combat.currentMovementSpeed;



                    transform.position += nextMovePos;


                    if (Vector3.Angle(cameraDirection, playerDirection) > 0.5f)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(cameraDirection, transform.up);

                        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

                    }
                    if (Input.GetAxis("PullBack") > 0)
                    {

                        if (bowPowerModifer < maxBowPower)
                        {
                            bowPowerModifer += (Time.deltaTime * bowPullBackRate);
                        }
                        else
                        {
                            bowPowerModifer = maxBowPower;
                        }
                        bowDrawn = true;

                    }

                    if (bowDrawn == true)
                    {

                        if (Input.GetAxisRaw("PullBack") == 0)
                        {
                            if (inventory.RemoveItem(arrow))
                            {
                                bowDrawn = false;

                                // GameObject arrow = Instantiate(arrowPrefab, bowPosition.position, playerCamera.transform.rotation);
                                GameObject arrow = ObjectPooler.PoolerInstance.GetPooledObject("Arrow");
                                arrow.SetActive(true);

                                arrow.transform.position = bowPosition.position;
                                arrow.transform.rotation = playerCamera.transform.rotation;
                                // GameObject arrow = Instantiate(arrowPrefab, bowPosition.position, playerCamera.transform.rotation);

                                Rigidbody arrowrb = arrow.GetComponent<Rigidbody>();
                                arrowrb.gameObject.GetComponent<Arrow>().damage = arrowDamage;
                                arrowrb.isKinematic = false;
                                arrowrb.AddForce(playerCamera.transform.forward * (shotForce + bowPowerModifer), ForceMode.Impulse);
                                arrow.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * (shotForce + bowPowerModifer), ForceMode.Impulse);
                                bowPowerTimer = 0;
                                bowPowerModifer = 0;
                            }
                        }
                    }
                }

        
        break;
            case interactionState.DialogueMode:
             
                break;
            case interactionState.InventoryMode:
                if (Input.GetButtonDown("Inventory"))
                {
                    SetInventoryMode(false);
                }

                break;
            case interactionState.CraftingMode:
                if (Input.GetButtonDown("Inventory"))
                {
                    SetInventoryMode(false);
                }

                break;
        }
    }
}
