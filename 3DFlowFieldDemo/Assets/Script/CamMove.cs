using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    [Header("Movement Data")]
    public float mMoveSpeed = 5.0f;
    public float mLookHorizantalSpeed = 2.0f;
    public float mLookVerticleSpeed = 2.0f;
    public float mZoomSpeed = 2.0f;
    public float mDrag = 5.0f;

    private float x = 0.0f;
    private float y = 0.0f;

    // Update is called once per frame
    void Update()
    {
        CameraMove();
        CameraLook();
        CameraDrag();
    }

    void CameraMove()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        transform.position += move * mMoveSpeed * Time.deltaTime;
    }

    void CameraDrag()
    {
        //Drag camera with middle click
        if (Input.GetMouseButton(2))
        {
            transform.Translate(-Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mDrag, -Input.GetAxisRaw("Mouse X") * Time.deltaTime * mDrag, 0.0f);
        }
    }

    void CameraLook()
    {
        //Look around with right click
        if (Input.GetMouseButton(1))
        {
            x += mLookHorizantalSpeed * Input.GetAxis("Mouse Y");
            y += mLookVerticleSpeed * Input.GetAxis("Mouse X");

            transform.eulerAngles = new Vector3(x, y, 0.0f);
        }

        //Zoom in camera
        transform.Translate(0.0f, 0.0f, Input.GetAxis("Mouse ScrollWheel") * mZoomSpeed, Space.Self);
    }
}
