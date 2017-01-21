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
	public Slider pulseSlider;
	public Image damageImage;
	public float flashSpeed = 5f;
	public Color flashColour = new Color(1f,0f,0f,0.1f);
	bool damaged;

    public event System.Action OnDeath;


    protected virtual void Start()
    {
        health = startingHealth;
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

        if (healthSlider != null)
            healthSlider.value = health;

        //print("Hit Taken");
        //print("Health: " + health);
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    protected void Die()
    {
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
