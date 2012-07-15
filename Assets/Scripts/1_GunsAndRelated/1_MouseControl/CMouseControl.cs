using UnityEngine;
using System.Collections;

public class CMouseControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Screen.showCursor = false;
	}
	
	// Update is called once per frame
	void Update () {
        Screen.lockCursor = true;
	}
}
