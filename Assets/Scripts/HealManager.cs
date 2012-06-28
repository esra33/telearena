using UnityEngine;
using System.Collections;



// Script for the Health manager of the character basically when he find a trigger and when something hit it 
// for the moment it didt have a health bar.

public class HealManager : MonoBehaviour {
	
	public float maxHealth = 100;
	public float healthRegenRate = 0.1f;
	public float currentHealth = 100;
	public bool intrigger = false;
	public bool overTime;
	public bool hit;
	public float damageOT = 5;
	public float damageHit = 30;
	
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
	
	void OnTriggerStay(Collider other) {
		
		if(other.gameObject.tag == "DamageOT" && intrigger){
			currentHealth = currentHealth - damageOT;
			Debug.Log("currhealth" + currentHealth);
			
			
		}
		intrigger = true;
        //currentHealth -=1;
		
    }
	
	  void OnTriggerExit(Collider other) {
        intrigger = false;
    }
	
	 void OnTriggerEnter(Collider other) {
       	if(other.gameObject.tag == "DamageHit"){
			currentHealth = currentHealth - damageHit;
		
		}
    }
}
