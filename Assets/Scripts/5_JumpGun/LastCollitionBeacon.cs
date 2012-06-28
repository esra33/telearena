using UnityEngine;
using System.Collections;

public class LastCollitionBeacon : MonoBehaviour {

    // Range at which the platform can be telleported, -1 is infinite
    public float m_TeleportRange = -1;

    // Layers to ignore
    LayerMask m_LayersToIgnore = 0;

    public LayerMask layersToIgnore
    {
        set { m_LayersToIgnore = value; }
    }
    
    // Elements to take into account to determine the position the platform will occupy 
    Vector3 m_vLastValidSpot;
    Vector3 m_vLastValidNormal;

    // Found a successful spot
    bool m_bSuccess = false;

    void OnCollisionEnter(Collision other)
    {
        float currAngle = 180;
        float evalAngle;
        foreach(ContactPoint cp in other.contacts)
        {
            evalAngle = Vector3.Angle(Vector3.Normalize(cp.point - transform.position), rigidbody.velocity.normalized);
            if ((cp.otherCollider.gameObject.layer & m_LayersToIgnore) == cp.otherCollider.gameObject.layer && evalAngle < currAngle)
            {
                m_bSuccess = true;
                currAngle = evalAngle;
                m_vLastValidSpot = cp.point;
                m_vLastValidNormal = cp.normal;
            }
        }
    }

    public bool DepolyPlatform(float platHeight, out Vector3 norm)
    {
        norm = Vector3.up;
        if(m_bSuccess)
        {
            if (Vector3.Distance(transform.position, m_vLastValidSpot) < m_TeleportRange)
            {
                transform.position = m_vLastValidSpot + m_vLastValidNormal * 0.5f * platHeight;    
                norm = m_vLastValidNormal;
            }
            else
                m_bSuccess = false;
        }

        return m_bSuccess;
    }
}
