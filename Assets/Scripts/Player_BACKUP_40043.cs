using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : LivingEntity {

    public float moveSpeed = 5;
    public float pulseCooldown = 5;
    public float nextPulseAvailableTime;
    
    public float maxDrops = 10;
    protected float dropsLeft;

    public bool  NoInput = false;

    PlayerController controller;

	public Animation anim;
    public event System.Action OnDeath;
    public event System.Action OnTriggerPulse;
    public event System.Action OnTriggerDrop;

    public Vector3 dropLocation;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        dropsLeft = maxDrops;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeSinceLevelLoad > nextPulseAvailableTime)
            {
                nextPulseAvailableTime = Time.timeSinceLevelLoad + pulseCooldown;
                // Play pulse
                var audioClip = Resources.Load<AudioClip>("ed_pulse_4c");
                AudioSource.PlayClipAtPoint(audioClip, new Vector3(5, 1, 2));
                triggerPulse();
<<<<<<< Updated upstream
                //print("Pulse");
=======
                print("Pulse");
				pulseSlider.value = 0;
				AnimatePulseUISlider (pulseCooldown);
>>>>>>> Stashed changes
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (dropsLeft > 0)
            {
                dropsLeft--;
                triggerDrop();
                //print("Drop");
            }
        }
    }

	public void AnimatePulseUISlider(float pulseCooldown) {
		
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


    public void triggerDrop()
    {
        if (OnTriggerDrop != null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {                
                dropLocation = GameObject.FindGameObjectWithTag("Player").transform.position;
            }
            OnTriggerDrop();
        }
    }
}
