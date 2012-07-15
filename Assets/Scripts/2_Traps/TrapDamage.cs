using UnityEngine;
using System.Collections;

public class TrapDamage : MonoBehaviour {
	
	public bool damageOverTime; //type of trap
	public bool damageOnHit;   // type of trap
	public float damageOT;    // amount of damage
	public float damageHit; // amount of damage
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		
        Debug.Log("funiona ?");
		if(other.gameObject.tag != "Player"){
			return;
		}
		
		if(damageOverTime || damageOnHit){
		
			other.gameObject.SendMessage("ApplyDamage", damageOT, SendMessageOptions.DontRequireReceiver);
			
		}
	}
	
	void OnTriggerStay(Collider other) {
		if(other.gameObject.tag != "Player"){
			return;
		}
		
		if(damageOverTime){
			other.gameObject.SendMessage("ApplyDamage", damageOT, SendMessageOptions.DontRequireReceiver);
		
		}
		
	}
}
