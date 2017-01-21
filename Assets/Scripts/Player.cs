using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : LivingEntity {

    public float moveSpeed = 5;
    public float pulseCooldown = 5;
    public float nextPulseAvailableTime;

    PlayerController controller;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        controller = GetComponent<PlayerController>();
        nextPulseAvailableTime = Time.timeSinceLevelLoad;
    }
	
	// Update is called once per frame
	void Update () {
        //Movement Input
        bool rotate = false;
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            rotate = true;
        }
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity, rotate);

        //Pulse Input
        if (Input.GetAxis("Jump") != 0)
        {
            if (Time.timeSinceLevelLoad > nextPulseAvailableTime)
            {
                nextPulseAvailableTime = Time.timeSinceLevelLoad + pulseCooldown;
                print("Jump");
            }
        }
    }
}
