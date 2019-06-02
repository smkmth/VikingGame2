using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCameraControl : MonoBehaviour {

    public Transform player;
    public float turnSpeed;
    public bool mouseControlsCamera;
    public Vector3 offset =Vector3.zero;
    private Camera cam;
    public float camHeight;
    public float maxCamHeight;
    public float minCamHeight;
    public float maxOrthSize = 15.0f;
    public float minOrthSize = 3.0f;
    public float adjust =0.1f;
    public float yoffsetSpeed =1.0f;
    public float zoomSpeed;
    public float heightSpeed;
    public bool zoomEffectsPan;
    public float rotSpeed;

    private float startTime;
    private float movedist;

    private Vector3 heightOffset= Vector3.zero; 
    private Vector3 oldpos = Vector3.zero;
    private Vector3 desiredOffset;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();

        //offset = new Vector3(player.position.x, player.position.y + 8.0f, player.position.z + 7.0f);
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if (mouseControlsCamera)
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        }
        else
        {


            if (Input.GetButtonDown("PosCameraRot"))
            {
             
                offset = Quaternion.AngleAxis(turnSpeed, Vector3.up) * offset;
                startTime = Time.time;
                movedist = Vector3.Distance(transform.position, offset);
                oldpos = transform.position;



            }
            else if (Input.GetButtonDown("NegCameraRot"))
            {
                offset = Quaternion.AngleAxis(-turnSpeed, Vector3.up) * offset;
                startTime = Time.time;
                movedist = Vector3.Distance(transform.position, offset);
                oldpos = transform.position;

            }

        }

        if (cam.orthographicSize <minOrthSize)
        {
            cam.orthographicSize = (minOrthSize + adjust);
        }
        else if(cam.orthographicSize > maxOrthSize)
        {
            cam.orthographicSize = (maxOrthSize - adjust);
        }
        else
        {
            cam.orthographicSize -= Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed;
            //cam.orthographicSize -= RsVertical * Time.deltaTime * zoomSpeed;

        }

        if (zoomEffectsPan)
        {
            if (camHeight < minCamHeight)
            {
                camHeight = minCamHeight + adjust;
            }
            else if (camHeight > maxCamHeight)
            {
                camHeight = maxCamHeight - adjust;
            }
            else
            {
                camHeight += Input.mouseScrollDelta.y * Time.deltaTime * heightSpeed;
                heightOffset.y -= Input.mouseScrollDelta.y * Time.deltaTime * yoffsetSpeed;
            }
        }

        if (transform.position != player.position + offset)
        {
            float distMoved = (Time.time - startTime) * rotSpeed;
            float fracJourney = distMoved / movedist;

            Vector3 lerpoff = Vector3.Lerp(oldpos, offset + player.position, fracJourney);
            transform.position = lerpoff;
        }

        transform.LookAt(player.position + (Vector3.up * camHeight));
    }

  
}
