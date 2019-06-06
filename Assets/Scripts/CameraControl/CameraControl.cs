using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraControl : MonoBehaviour
{

    [Header("Camera Properties")]

    private float DistanceAway;

    [Tooltip("Max distance from the player")]
    public float minDistance = 1;                //min camera distance
    [Tooltip("Min distance from the player")]
    public float maxDistance = 2;                //max camera distance

    private float DistanceUp = -2;                    //how high the camera is above the player
    public float smooth = 4.0f;
    public float currentSmooth;//how smooth the camera moves into place
    public float rotateAround = 70f;
    public float minDown;                      //the angle at which you will rotate the camera (on an axis)
    public float minUp;
    [Header("Player to follow")]
    public Transform target;                    //the target the camera follows

    [Header("Layer(s) to include")]
    public LayerMask CamOcclusion;                //the layers that will be affected by collision

    [Header("Map coordinate script")]

    RaycastHit hit;
    float cameraHeight = 55f;
    float cameraPan = 0f;
    public float camPanSpeed;
    private float camRotateSpeed = 180f;
    public float mouseRotateSpeed = 180.0f;
    public float controllerRotateSpeed = 3000.0f;
    public float distToGround;
     Vector3 camPosition;
    Vector3 camMask;
    Vector3 followMask;
    public bool controllerInput;

    Vector3 targetOffset;
    Vector3 hosMove;

    private float HorizontalAxis;
    private float VerticalAxis;
    public float overShoulderMod;
    public bool lockedOn;
    public Transform lockOnTarget;
    public Transform aimTarget;
    public PlayerInteraction player;
    public float followDistance;
    public float followHeight;
    private GameObject sceneManager;
    public float castRadius;
    public float shotRange;
    public GameObject crosshair;



    // Use this for initialization
    void Start()
    {
        currentSmooth = smooth;
        
        
        sceneManager = GameObject.Find("SceneManager");
        //the statement below automatically positions the camera behind the target.
        //hosMove = transform.position - target.transform.position;
        controllerPluggedIn controller = ControllerDetector.CheckController();
        if (controller != controllerPluggedIn.noController)
        {
            controllerInput = true;
        }

        if (controllerInput)
        {
            Debug.Log("Xbox controller plugged in");

            camRotateSpeed = controllerRotateSpeed;
        }
        else
        {
            Debug.Log("No controller plugged in");
            camRotateSpeed = mouseRotateSpeed;
        }

    }
    public void ToggleLockOn(bool lockOn)
    {
       if (lockOn)
        {
            transform.parent = target.transform;
            lockedOn = true;
        }
       else
        {
            transform.parent = sceneManager.transform;
            lockedOn = false;

        }

    }
    void LateUpdate()
    {
        if (player.currentInteractionState == interactionState.Normal || player.currentInteractionState == interactionState.Aiming)
        {
         
            HorizontalAxis = Input.GetAxis("Mouse X") + Input.GetAxis("RightStickHorizontal");
            VerticalAxis = Input.GetAxis("Mouse Y") + Input.GetAxis("RightStickVertical");

            //Offset of the targets transform (Since the pivot point is usually at the feet).
            Vector3 targetOffset = new Vector3(target.position.x, (target.position.y + 1f), target.position.z);
            Quaternion rotation = Quaternion.Euler(cameraHeight, rotateAround, cameraPan);
            Vector3 vectorMask = Vector3.one;
            Vector3 rotateVector = rotation * vectorMask;
            //this determines where both the camera and it's mask will be.
            //the camMask is for forcing the camera to push away from walls.
            camPosition = targetOffset + Vector3.up * DistanceUp - rotateVector * DistanceAway;
            camMask = targetOffset + Vector3.up * DistanceUp - rotateVector * DistanceAway;


            OccludeRay(ref targetOffset);
            //GroundRay();
            SmoothCamMethod();

            if (player.aiming)
            {
                transform.LookAt(aimTarget);
            }
            else
            {
                transform.LookAt(target);

            }

            if (rotateAround > 360)
            {
                rotateAround = 0f;
            }
            else if (rotateAround < 0f)
            {
                rotateAround = (rotateAround + 360f);
            }
            
            rotateAround += HorizontalAxis * camRotateSpeed * Time.deltaTime;

            if (player.aiming)
            {
                RaycastHit hitInfo;
                Physics.SphereCast(crosshair.transform.position, castRadius, transform.forward, out hitInfo, shotRange);
                if (hitInfo.collider != null)
                {
                    if (hitInfo.collider.gameObject.tag == "Enemy")
                    {
                        Debug.Log("Aiming");
                    }
                }
                DistanceAway -= overShoulderMod;
            }
            
            DistanceUp = Mathf.Clamp(DistanceUp += VerticalAxis * camPanSpeed * Time.deltaTime, minDown, minUp);
            DistanceAway = Mathf.Clamp(DistanceAway += VerticalAxis * camPanSpeed * Time.deltaTime, minDistance, maxDistance);
        }


    }
    void SmoothCamMethod()
    {
        currentSmooth = smooth;
        transform.position = Vector3.Lerp(transform.position, camPosition, Time.deltaTime * currentSmooth);
    }
    void OccludeRay(ref Vector3 targetFollow)
    {
        //declare a new raycast hit.
        RaycastHit wallHit = new RaycastHit();
        //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
        if (Physics.Linecast(targetFollow, camMask, out wallHit, CamOcclusion))
        {
            //the smooth is increased so you detect geometry collisions faster.
            currentSmooth = smooth *2;
            //the x and z coordinates are pushed away from the wall by hit.normal.
            //the y coordinate stays the same.
            camPosition = new Vector3(wallHit.point.x + wallHit.normal.x * 0.5f, camPosition.y, wallHit.point.z + wallHit.normal.z * 0.5f);
        }
    }
    void GroundRay()
    {

        //declare a new raycast hit.
        RaycastHit floorHit = new RaycastHit();
        //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
        if (Physics.Raycast(transform.position, Vector3.down,out floorHit, distToGround, CamOcclusion ))
        {
            
            minDown = (floorHit.transform.position.y -transform.position.y);
     
        }
    }
}