using UnityEngine;
using System.Collections;

public class TimeRespawnSystem : MonoBehaviour {

    // Time
    public float m_StartingTime;
    float m_CurrentTime;
    bool m_bTrackTime = true;

    // Player tracker
    Vector3 m_OriginalPosition;
    Vector3 m_OriginalRotation;
    GameObject m_Player;

    // Use this for initialization
	void Start () {

        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_OriginalPosition = m_Player.transform.position;
        m_OriginalRotation = m_Player.transform.eulerAngles;

        m_CurrentTime = m_StartingTime;

	}

    void SystemReset()
    {
        m_CurrentTime = m_StartingTime;
        m_Player.transform.position = m_OriginalPosition;
        m_Player.transform.eulerAngles = m_OriginalRotation;
        m_Player.GetComponent<CharacterMotor>().SetVelocity(Vector3.zero);
    }

    void OnGUI()
    {
        GUILayout.Label("Time: " + ((int)m_CurrentTime/60) + ":" + ((int)m_CurrentTime%60));
    }
	
	// Update is called once per frame
	void Update () {

        if (!m_bTrackTime)
            return;

        m_CurrentTime -= Time.deltaTime;
        if(m_CurrentTime < 0)
        {
            SystemReset();
        }

	}

    // End state
    public void EndStateReached()
    {
        m_bTrackTime = false;
    }
}
