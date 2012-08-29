using UnityEngine;
using System.Collections;

public class SetRespawn : MonoBehaviour {
	
	public RespawnPointScript playerscript;
	public int respawnIdentifier;
	// Use this for initialization
	void Start () {
		//RespawnPointScript playerscript;
		playerscript = GetComponent<RespawnPointScript>();
		
		
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
	
		if(other.gameObject.tag == "Player"){
			//Debug.Break();
			//playerscript.respawn = true;
			other.gameObject.SendMessage("setRespawn", SendMessageOptions.DontRequireReceiver);
		}
			
			
			
		
	}
}
