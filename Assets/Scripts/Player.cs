using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : LivingEntity {

    public float moveSpeed = 5;
    public float pulseCooldown = 5;
    public float nextPulseAvailableTime;

    public bool  NoInput = false;

    PlayerController controller;

    public event System.Action OnDeath;
    public event System.Action OnTriggerPulse;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        controller = GetComponent<PlayerController>();
        nextPulseAvailableTime = Time.timeSinceLevelLoad;
    }
	
	// Update is called once per frame
	void Update () {
        if (NoInput)
        {
            return;
        }

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
                // Play pulse
                var audioClip = Resources.Load<AudioClip>("ed_pulse_4");
                AudioSource.PlayClipAtPoint(audioClip, new Vector3(5, 1, 2));
                triggerPulse();
                print("Pulse");
            }
        }
    }

    public void Goto(Vector3 pos)
    {
        NoInput = true;
        controller.Move(Vector3.zero, false);
        controller.MovePos(pos);
    }

    public void Standing() {
        NoInput = false;
    }

    public void triggerPulse()
    {
        if (OnTriggerPulse != null)
        {
            OnTriggerPulse();
        }
    }
}
