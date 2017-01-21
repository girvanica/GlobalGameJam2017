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

    public event System.Action OnDeath;


    protected virtual void Start()
    {
        health = startingHealth;
    }

	void Update() {
		if (damaged) 
		{
			damageImage.color = flashColour;	
		} 
		else
		{
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
		damaged = true;

        health -= damage;

		healthSlider.value = health;

        print("Hit Taken");
        print("Health: " + health);
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    protected void Die()
    {
        dead = true;
        if(OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
