using UnityEngine;
using System.Collections;

public class TrapDamage : MonoBehaviour {
	
	public bool damageOverTime;
	public bool damageOnHit;
	public float damage;
	

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
		
			other.gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
			
		}
	}
	
	void OnTriggerStay(Collider other) {
		if(other.gameObject.tag != "Player"){
			return;
		}
		
		if(damageOverTime){
			other.gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
		
		}
		
	}
}
