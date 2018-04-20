using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Programmer: Dibbie Knight - www.simpleminded.x10host.com - dibbieknight@gmail.com
/// The purpose of this script is to allow the player to look around with the mouse, in an FPS feeling.
/// This script should be applied to the camera.
/// </summary>

public class MouseLook : MonoBehaviour
{
    [Range(70, 150)]
    public float rotateSpeed = 70f; //how fast to rotate the camera around the y and x when moving the mouse
    public float minY = -45f, maxY = 45f; //the clamps of up/down (y axis) motion, how far up/down can the camera look?

    [Space]

    public bool rotateCharacter; //should the player model itself also rotate with the camera?
    public Transform character; //the player model to be rotating, if rotateCharacter is checked.

    private float yRot;

    void Update()
    {
        Vector3 cameraRot = transform.eulerAngles;
        Vector3 playerRot = character.eulerAngles;
        
        //LEFT/RIGHT rotation
        if (rotateCharacter) { playerRot.y += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime; }
        else { cameraRot.y += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime; }

        //UP/DOWN rotation (with clamp)
        yRot -= Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
        yRot = Mathf.Clamp(yRot, minY, maxY);
        cameraRot.x = yRot;

        //apply variables
        transform.eulerAngles = cameraRot;
        character.eulerAngles = playerRot;
    }
}
