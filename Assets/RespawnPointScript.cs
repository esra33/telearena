using UnityEngine;
using System.Collections;

public class RespawnPointScript : MonoBehaviour {
	
	
	public Transform respawnPoint; // 
	public bool respawn;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(respawn){
			transform.position = respawnPoint.position;
			
		}
	
	}
	
	public void setRespawn(){
		//Debug.Break();
		respawn = true;	
	}
	
	
}
