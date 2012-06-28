using UnityEngine;
using System.Collections;

public class ObjectTeleportBeacon : MonoBehaviour {

    // Related gun
    ObjectTeleportGun m_pRelatedGun = null;
    
    // Original Gun
    GunInterface m_pOriginalGun = null;

    // Object to encapsulate/Object attached to the capsule-beacon
    Transform m_pEncapsulatedObject; 

    // Play "animation"
    bool m_bPlayAnimation = false;

    // Set the original gun
    public void SetOriginalGun(GunInterface originalGun)
    {
        if (originalGun != null)
            m_pOriginalGun = originalGun;
    }

    // Set the teleport gun
    public void SetTeleportGun(ObjectTeleportGun teleportGun)
    {
        if(teleportGun != null)
            m_pRelatedGun = teleportGun;
    }

    // Teleport the related object when indicated
    public void TeleportItem(Vector3 teleportLocation)
    {
        // set the position
        m_pEncapsulatedObject.position = teleportLocation;

        // inform the original gun that deactivation is in process
        if (m_pOriginalGun)
            m_pOriginalGun.ResetShoot();

        // Stop animation
        m_bPlayAnimation = false;

        // destroy myself (the gun should detect the change as the memory is deallocated)
        Destroy(gameObject);
    }

    // create graphical object on location, teleport to the middle of the object and encapsulate it
	void OnCollisionEnter(Collision other)
    {
        // check for teleportable objects
        if (other.transform.tag != "Teleportable")
            return;

        m_pEncapsulatedObject = other.transform;

        // stop movement
        rigidbody.velocity = Vector3.zero;
        rigidbody.isKinematic = true;

        // Set animation play true
        m_bPlayAnimation = true;

        // Report the capsule to the main gun
        m_pRelatedGun.ReportCapsule(transform, m_pEncapsulatedObject);

        // Destroy collider
        Destroy(transform.collider);
    }

    // Animation coroutine
    IEnumerator PlayAnimation()
    {
        while (m_bPlayAnimation)
        {
            renderer.materials[0].color = Color.red;
            yield return new WaitForSeconds(0.5f);
            renderer.materials[0].color = Color.blue;
            yield return new WaitForSeconds(0.5f);
        }
        yield return 0;
    }

    // play animations
    void Update()
    {
        StartCoroutine(PlayAnimation());
    }
}
