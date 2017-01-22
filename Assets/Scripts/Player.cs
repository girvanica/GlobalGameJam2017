using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public event System.Action OnTriggerPulse;
    public event System.Action OnTriggerDrop;

    public Slider pulseSlider;
    bool pulseAnim = false;
    float pulseAnimTime;
    public Vector3 dropLocation;

    public Transform Decoy;

    public bool hasKey = false;
    Key key;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        dropsLeft = maxDrops;
        controller = GetComponent<PlayerController>();
        nextPulseAvailableTime = Time.timeSinceLevelLoad;
        pulseSlider = GameObject.FindGameObjectWithTag("PulseSlider").GetComponent<Slider>();

        health = startingHealth;
        healthSlider = GameObject.FindGameObjectWithTag("HealthSlider").GetComponent<Slider>();

        key = GameObject.FindGameObjectWithTag("Key").transform.GetComponent<Key>();
        key.OnKeyPickup += OnKeyPickup;
    }

    private void OnKeyPickup()
    {
        hasKey = true;
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
        if (Input.GetButtonDown("Jump"))
        {
            if (Time.timeSinceLevelLoad > nextPulseAvailableTime)
            {
                nextPulseAvailableTime = Time.timeSinceLevelLoad + pulseCooldown;
                // Play pulse
                var audioClip = Resources.Load<AudioClip>("ed_pulse_4c");
                AudioSource.PlayClipAtPoint(audioClip, transform.position);
                triggerPulse();
                //print("Pulse");
                if (pulseSlider != null)
				    pulseSlider.value = 0;
                pulseAnim = true;
                pulseAnimTime = Time.realtimeSinceStartup;
                AnimatePulseUISlider (pulseCooldown);
            }
        }

        if (pulseAnim && (pulseAnimTime < Time.realtimeSinceStartup + pulseCooldown))
        {
            if (pulseSlider != null)
                pulseSlider.value += (1 * Time.deltaTime/ 4) * 100;

            if (pulseSlider.value == 100)
                pulseAnim = false;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            //TODO: Display pause menu
        }


        if (Input.GetButtonDown("Fire3"))
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
                Instantiate(Decoy, dropLocation, Quaternion.identity);
            }
            OnTriggerDrop();
        }
    }
}
