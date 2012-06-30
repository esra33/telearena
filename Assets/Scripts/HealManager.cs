using UnityEngine;
using System.Collections;



// Script for the Health manager of the character basically when he find a trigger and when something hit it 
// for the moment it didt have a health bar.

public class HealManager : MonoBehaviour {
	
	public float maxHealth = 100;
	public float healthRegenRate = 0.1f;
	public float currentHealth = 100;
	public bool intrigger = false;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//intrigger = false;
		print(currentHealth);
		
		if (currentHealth < maxHealth){
			if (!intrigger){
				currentHealth = currentHealth + healthRegenRate;
			}	
		}
	
	}
	
	public void ApplyDamage(float damage){
		
		currentHealth = currentHealth - damage;
		Debug.Log("healt" + currentHealth);
		if (currentHealth <= 0){
			Destroy(gameObject);	
		}
		
	}
	

	
	
		
		
}

