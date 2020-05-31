using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    //public 
    public float m_rotationSpeedX = 120f;       // rotation Speed
    public float m_rotationSpeedY = 120f;       // rotation Speed
    public float m_CamMoveSpeed = 30f;          // Cam Switch Speed


    //private
    private float m_rotX = 0;                    // for mouse rotX
    private Transform m_player;                  // Player


    private void Awake()
    {
        m_player = transform.parent;

    }

    private void Update()
    {

            ProcessRotation();
        
    }

    private void ProcessRotation()
    {
        Quaternion playerRot = m_player.transform.rotation;
        Quaternion cameraRot = transform.localRotation;

        Vector3 VecPlayerRot = playerRot.eulerAngles; //Convert the Player Quaternion to Vector3    is needed for Adding the MousMovement
        Vector3 VecCameraRot = cameraRot.eulerAngles; //Convert the Cam Quaternion to Vector3       is needed for Adding the MousMovement

        m_rotX = VecPlayerRot.y + Input.GetAxis("Mouse X") * m_rotationSpeedX * Time.deltaTime; //Get the X-Axis
        VecCameraRot.x = VecCameraRot.x + (-Input.GetAxis("Mouse Y")) * m_rotationSpeedY * Time.deltaTime; //Get the Y-Axis
        VecCameraRot.x = Mathf.Clamp(VecCameraRot.x, 0f, 55f); //Clamp the Y Axis to dont look to high or to low

        VecPlayerRot.y = m_rotX;

        cameraRot = Quaternion.Euler(VecCameraRot); // convert Back to Quaternion
        playerRot = Quaternion.Euler(VecPlayerRot); // convert Back to Quaternion

        transform.localRotation = cameraRot; // rotatet the cam
        m_player.transform.rotation = playerRot; // rotate the player
    }
}
