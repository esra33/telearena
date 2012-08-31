using UnityEngine;
using System.Collections;

public class SetNewCheckPoint : MonoBehaviour {
	
	public RespawnPointScript playerscript;
	
	
	// Use this for initialization
	void Start () {
		playerscript = GameObject.FindGameObjectWithTag("Player").GetComponent<RespawnPointScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
	
		if(other.gameObject.tag == "Player"){
			Transform myTransform = transform;
			
			playerscript.setRespawnPoint(myTransform);
		}
			
			
			
		
	}
}
