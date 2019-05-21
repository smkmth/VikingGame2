using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCameraControl : MonoBehaviour {

    public Transform player;
    public float turnSpeed;
    public bool mouseControlsCamera;
    private Vector3 offset;
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

    private Vector3 heightOffset= Vector3.zero; 

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
        transform.position = player.transform.position + offset;
        if (mouseControlsCamera)
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        }
        else
        {
            float rot = Input.GetAxisRaw("CameraRot");

            if (Input.GetButtonDown("PosCameraRot"))
            {
                offset = Quaternion.AngleAxis(turnSpeed, Vector3.up) * offset;

            }
            else if (Input.GetButtonDown("NegCameraRot"))
            {
                offset = Quaternion.AngleAxis(-turnSpeed, Vector3.up) * offset;

            }
           // offset = Quaternion.AngleAxis(Input.GetAxisRaw("CameraRot") * turnSpeed, Vector3.up) * offset;

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



        transform.position = player.position + offset + heightOffset;
        transform.LookAt(player.position + (Vector3.up * camHeight));
    }
}
