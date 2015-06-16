using UnityEngine;
using System.Collections;

public class PlatformBehavior : MonoBehaviour {

    // Platform Life Cooldown in seconds
    public float m_Lifespawn = 10;
    float m_InternalLifespan;
    bool m_bActive = false;

    // Hideout position
    Vector3 m_HidePosition;

    void Start()
    {
        m_HidePosition = transform.position;
    }

    public void Activate(bool attach)
    {
        GetComponent<Rigidbody>().isKinematic = attach; // set as kinematic
        m_InternalLifespan = m_Lifespawn;

        if (!m_bActive)
        {
            m_bActive = true;
            StartCoroutine(StartCooldown());
        }
    }

    void OnCollisionEnter(Collision other)
    {
       if (!GetComponent<Rigidbody>().isKinematic)
       {
           Vector3 currNormal = Vector3.zero;
           Vector3 currPos = Vector3.zero;

           foreach (ContactPoint cp in other.contacts)
           {
               currPos += cp.point;
               currNormal += cp.normal;
           }

           transform.up = currNormal / other.contacts.Length;
           transform.position = currPos / other.contacts.Length + Vector3.up * transform.lossyScale.y;

           GetComponent<Rigidbody>().isKinematic = true;
       }
    }

    IEnumerator StartCooldown()
    {
        while (!GetComponent<Rigidbody>().isKinematic)
        {
            yield return new WaitForFixedUpdate();
        }

        // Activate jump system
        while (m_InternalLifespan > 0)
        {
            m_InternalLifespan -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        m_bActive = false;
        transform.position = m_HidePosition; // Hide
        yield return 0;
    }
}
