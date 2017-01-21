using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : LivingEntity {

    public float moveSpeed = 5;
    PlayerController controller;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        controller = GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        bool rotate = false;
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            rotate = true;
        }
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity, rotate);
    }
}
