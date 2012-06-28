using UnityEngine;
using System.Collections;

public class FreeFPSMovement : MonoBehaviour {

    public float m_StandardSpeed = 10.0f;
    CharacterController m_pCharacterController;

    void Start()
    {
        m_pCharacterController = gameObject.GetComponent<CharacterController>();
    }

	// Update is called once per frame
	void Update () {
        
        Vector3 vVelocity = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        m_pCharacterController.Move(vVelocity * m_StandardSpeed * Time.deltaTime);
	}
}
