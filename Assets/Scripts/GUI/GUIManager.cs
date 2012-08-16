using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	
	public HealManager HealthScript;
	// Use this for initialization
	void Start () {
		HealthScript = GetComponent<HealManager>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
	
		//GUI.Label (new Rect (10,70, 100, 20), /*"Health: "*/ HealthScript.currentHealth.ToString() );
		
	}
	
}
