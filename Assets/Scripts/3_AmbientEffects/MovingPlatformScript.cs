using UnityEngine;
using System.Collections;

public class MovingPlatformScript : MonoBehaviour {
	
	
	public Transform activePlatform;
	public Transform pointA;
	public Transform pointB;
	public Vector3 newTargetPoint;
	public float speed = 1.0f;
	public bool reach;

	
	// Use this for initialization
	void Start () {
		
		newTargetPoint = pointA.position;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(activePlatform != null)
		{
			MoveHorizontal();
			reach = true;
			
			if(activePlatform.position == pointA.position && reach)
				ReturnHorizontal();
			
			
			
						
		}
	
	}
	
	private void MoveHorizontal(){
		
		activePlatform.position = Vector3.Slerp(activePlatform.position, newTargetPoint, Time.deltaTime * speed);
		
//		if(activePlatform.position == pointA.position)
//			Debug.Log("entre");
//			newTargetPoint = pointB.position;
		
		
		
	}
	
	private void ReturnHorizontal(){
		
		activePlatform.position = Vector3.MoveTowards(activePlatform.position, pointB.position, Time.deltaTime * speed);
		
	}
}
