using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Programmer: Dibbie Knight - www.simpleminded.x10host.com - dibbieknight@gmail.com
/// The purpose of this script is to allow the player to move around the gameworld with movement settings from InputManager.
/// This script should be applied to the player.
/// </summary>

public class InputHandler : MonoBehaviour {

    public float baseSpeed = 7f; //how fast to move normally, when not "sprinting"
    public float runSpeed = 12f; //how fast to move when holding down the sprint key
    public float jumpHeight = 9f; //how high to jump
    public float gravity = 25f; //how fast to fall back to the ground, when airborne

    [Space]

    public KeyCode primarySprint = KeyCode.LeftShift; //main key for sprinting
    public KeyCode secondarySprint = KeyCode.RightShift; //alt key for sprinting
    public KeyCode inventoryKey = KeyCode.E; //E for Enventory. :D

    [Space]

    public Camera cam; //the camera child of this player

    private float moveSpeed;

    private CharacterController self;
    private Vector3 direction = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;

    private UIHandler uIHandler;

    // Use this for initialization
    void Start () {
        self = GetComponent<CharacterController>();
        uIHandler = GetComponent<UIHandler>();
        moveSpeed = baseSpeed;
	}

    private void Update()
    {
        if (Input.GetKeyUp(inventoryKey))
        {
            if (uIHandler.currState == "main_inventory") { uIHandler.currState = ""; }
            else { uIHandler.currState = "main_inventory"; }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (self.isGrounded)
        {

            //Movement
            direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); //get the current directional input
            moveDirection = direction; //apply the directional input (moveDirection changes constantly, direction doesnt)
            moveDirection += cam.transform.forward * direction.z * moveSpeed; //add the cameras forward (z) to the directional input, and speed
            moveDirection += cam.transform.right * direction.x * moveSpeed; //add the cameras right (x) to the directional input, and speed
            moveDirection.y = 0f;

            //Sprinting
            if (Input.GetKey(primarySprint) || Input.GetKey(secondarySprint))
            { moveSpeed = runSpeed; }
            else if (Input.GetKeyUp(primarySprint) || Input.GetKeyUp(secondarySprint))
            { moveSpeed = baseSpeed; }

            //Jumping
            if (Input.GetButton("Jump"))
            { moveDirection.y = jumpHeight; }

        }

        //apply variables
        moveDirection.y -= gravity * Time.deltaTime;
        self.Move(moveDirection * Time.deltaTime);
    }
}
