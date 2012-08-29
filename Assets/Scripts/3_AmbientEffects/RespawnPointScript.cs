using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RespawnPointScript : MonoBehaviour {
	
	
	//public Transform[] respawnPoint = new Transform[10]; // 
	public List<Transform> respawnPoints;
	public bool respawn;
	public SetRespawn respawnPointScript;
	//public Transform respawnPoint;
	

	// Use this for initialization
	void Start () {
		//respawnPoints = new List<Transform>();
		respawnPointScript = GetComponent<SetRespawn>();
	
	}
	
	// Update is called once per frame
	void Update () {
		if(respawn){
			transform.position = respawnPoints[0].position;
			respawn = false;
			
		}
	
	}
	
	public void setRespawn(){
		//Debug.Break();
		respawn = true;	
	}
	
	public void setRespawnPoint(Transform respawnPoint){
	
		respawnPoints[0] = respawnPoint;
		
	}
	
	
}
