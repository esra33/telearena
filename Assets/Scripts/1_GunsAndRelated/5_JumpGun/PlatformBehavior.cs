using UnityEngine;
using System.Collections;

public class PlatformBehavior : MonoBehaviour {

    // Platform Life Cooldown in seconds
    public float m_Lifespawn = 10;
    float m_InternalLifespan;
    bool m_bActive = false;

    // Objects to ignore
    LayerMask m_LayersToIgnore = 0;
    
    // Hideout position
    Vector3 m_HidePosition;

    void Start()
    {
        m_HidePosition = transform.position;
    }

    public void SetLayersToIgnore(LayerMask mask)
    {
        m_LayersToIgnore = mask;
    }

    public void Activate(bool attach)
    {
        rigidbody.isKinematic = attach; // set as kinematic
        m_InternalLifespan = m_Lifespawn;

        if (!m_bActive)
        {
            m_bActive = true;
            StartCoroutine(StartCooldown());
        }
    }

    void OnCollision(Collision other)
    {
       if (!rigidbody.isKinematic)
       {
           float currAngle = 90;
           float evalAngle;

           foreach (ContactPoint cp in other.contacts)
           {
               // The mask doesn't come inverted so only accept if the mask fails
               evalAngle = Vector3.Angle(Vector3.Normalize(cp.point - transform.position), rigidbody.velocity.normalized);
               if (((1 << cp.otherCollider.gameObject.layer) & m_LayersToIgnore.value) == 0 && evalAngle < currAngle)
               {
                   currAngle = evalAngle;
                   transform.position = cp.point;
                   transform.up = cp.normal;
                   rigidbody.isKinematic = true;
               }
           }
       }
    }

    IEnumerator StartCooldown()
    {
        while (!rigidbody.isKinematic)
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
