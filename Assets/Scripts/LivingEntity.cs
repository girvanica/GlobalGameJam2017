using System;
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
	GameObject[] diedObjects;
	public bool died;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
		
		hideDied();
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
        var audioClip = Resources.Load<AudioClip>("ed_hero_hit");
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
            ChangePlayerSound("breathing4");
        }

        if (((health / startingHealth) < 0.4) && (((health + damage) / startingHealth) >= 0.4))
        {
            ChangePlayerSound("breathing5");
        }
    }


    protected void ChangePlayerSound(string clipName)
    {
        var breathe = Resources.Load<AudioClip>(clipName);
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<AudioSource>().clip = breathe;
            player.GetComponent<AudioSource>().loop = true;
            player.GetComponent<AudioSource>().Play();
        }
    }

    protected void Die()
    {
        healthSlider.value = 0;
        // Play sound
        var audioClip = Resources.Load<AudioClip>("ed_hero_death_ws");
        AudioSource.PlayClipAtPoint(audioClip, new Vector3(5, 1, 2));
        dead = true;
		diedObjects = GameObject.FindGameObjectsWithTag("ShowOnDeath");
		showDied();
        if(OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }

	public void showDied()
	{
		foreach (GameObject g in diedObjects)
		{
			g.SetActive(true);
		}
	}

	public void hideDied()
	{
		foreach (GameObject g in diedObjects)
		{
			g.SetActive(false);
		}
	}

	public void diedControl()
	{
		if (died)
		{
			Time.timeScale = 0;
			showDied();
		}
		else
		{
			Time.timeScale = 1;
			hideDied();
		}

	}
}
