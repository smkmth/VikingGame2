using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCameraControl : MonoBehaviour {

    public Transform player;
    public float turnSpeed;
    public bool mouseControlsCamera;
    private Vector3 offset;  

    // Use this for initialization
    void Start()
    {

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

 
        transform.position = player.position + offset;
        transform.LookAt(player.position);
    }
}
