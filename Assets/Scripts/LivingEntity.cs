﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamageable
{

    public float startingHealth;
    protected float health;
    protected bool dead;
	public Slider healthSlider;
	public Image damageImage;
	public float flashSpeed = 5f;
	public Color flashColour = new Color(1f,0f,0f,0.1f);
	bool damaged;

    public event System.Action OnDeath;

    protected virtual void Start()
    {

    }

	void Update() {
		if (damaged) 
		{
            if (damageImage != null)
			    damageImage.color = flashColour;	
		} 
		else
		{
            if (damageImage != null)
                damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		damaged = false;
	}

	public void TakeHit(float damage, RaycastHit hit)

    {
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        // Play sound
        var audioClip = Resources.Load<AudioClip>("ed_enemy_hit");
        AudioSource.PlayClipAtPoint(audioClip, new Vector3(5, 1, 2));

        damaged = true;

        health -= damage;
        //print("health: " + health + " | " + startingHealth);
        if (healthSlider != null)
            healthSlider.value = 100 / startingHealth * health;

        //print("Hit Taken");
        //print("Health: " + health);
        if (health <= 0 && !dead)
        {
            Die();
        }

        if (((health / startingHealth) < 0.7) && (((health + damage) / startingHealth) >= 0.7))
        {
            // Update sound
            print("Sound 4");
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                var breathe2 = Resources.Load<AudioClip>("breathing4");
                player.GetComponent<AudioSource>().clip = breathe2;
                player.GetComponent<AudioSource>().loop = true;
                player.GetComponent<AudioSource>().Play();
            }
        }

        if (((health / startingHealth) < 0.4) && (((health + damage) / startingHealth) >= 0.4))
        {
            // Update sound
            print("Sound 5");

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                var breathe3 = Resources.Load<AudioClip>("breathing5");
                player.GetComponent<AudioSource>().clip = breathe3;
                player.GetComponent<AudioSource>().loop = true;
                player.GetComponent<AudioSource>().Play();
            }
        }
    }

    protected void Die()
    {
        healthSlider.value = 0;
        // Play sound
        var audioClip = Resources.Load<AudioClip>("ed_hero_death");
        AudioSource.PlayClipAtPoint(audioClip, new Vector3(5, 1, 2));
        dead = true;
        if(OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
