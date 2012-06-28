using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunManager : MonoBehaviour {

    public List<GameObject> m_lGuns;
    public int m_CurrentGunId = 0;

    public KeyCode[] m_vCodes = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };

	// Use this for initialization
	void Start () {
	    
        for(int con = 0; con < m_lGuns.Count; con++)
        {
            m_lGuns[con].renderer.enabled = false;
        }

        m_lGuns[m_CurrentGunId].renderer.enabled = true;

	}

    // Update is called once per frame
    void Update()
    {
	    // Choose weapon
        int con = (m_CurrentGunId + 1) % m_vCodes.Length;
        while(con != m_CurrentGunId)
        {
            if (Input.GetKeyUp(m_vCodes[con]))
            {
                m_lGuns[m_CurrentGunId].BroadcastMessage("OnWeaponChange", WEAPON_SWITCH.DEACTIVATING, SendMessageOptions.DontRequireReceiver);
                m_lGuns[m_CurrentGunId].renderer.enabled = false;
                m_CurrentGunId = con;
                m_lGuns[m_CurrentGunId].renderer.enabled = true;
                m_lGuns[m_CurrentGunId].BroadcastMessage("OnWeaponChange", WEAPON_SWITCH.ACTIVATING, SendMessageOptions.DontRequireReceiver);
                break;
            }

            con = (con + 1) % m_vCodes.Length;
        }
        
        // shoot and activate the guns
        if (Input.GetButtonDown("Fire1"))
        {
            m_lGuns[m_CurrentGunId].BroadcastMessage("DoGunShoot", null, SendMessageOptions.DontRequireReceiver);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            m_lGuns[m_CurrentGunId].BroadcastMessage("DoGunActivation", null, SendMessageOptions.DontRequireReceiver);
        }

	}
}
