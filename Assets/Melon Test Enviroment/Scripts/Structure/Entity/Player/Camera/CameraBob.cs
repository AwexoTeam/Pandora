using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Programmer: Dibbie Knight - www.simpleminded.x10host.com - dibbieknight@gmail.com
/// The purpose of this script is to create a "headbob" motion to the camera as you move.
/// This script should be applied to the camera.
/// </summary>

public class CameraBob : MonoBehaviour {

    public float bobbingSpeed = 0.2f; //rate that the camera will "bob"
    public float bobbingAmount = 0.085f; //how far from the center the camera will "bob" - the 'intensity'
    public float midpoint = 2f; //center to return to after bobbing

    private float timer = 0.0f;

    void Update()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 bob = transform.localPosition;

        //reset timer if no input is given
        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        //set data for bobbing
        else
        {
            waveslice = Mathf.Sin(timer); //set direction based on time
            timer = timer + bobbingSpeed;

            if (timer > Mathf.PI * 2) //reverse direction
            {
                timer = timer - (Mathf.PI * 2);
            }
        }

        //translate to the bobbed direction 
        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            bob.y = midpoint + translateChange;
        }
        //reset bob to center
        else
        {
            bob.y = midpoint;
        }

        //apply bob to camera
        transform.localPosition = bob;
    }
}
