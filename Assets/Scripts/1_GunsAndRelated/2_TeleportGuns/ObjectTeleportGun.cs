using UnityEngine;
using System.Collections;

public class ObjectTeleportGun : GenericTeleportGun
{
    // Capsule/beacon for the external object
    public Transform m_pRelatedCapsule; //Capsule attached to the object to teleport
    public Transform m_pRelatedObject; 

    // Report the capsule object to the gun
    public void ReportCapsule(Transform pCapsule, Transform pObject)
    {
        m_pRelatedCapsule = pCapsule;
        m_pRelatedObject = pObject;
    }
	
    // Activate the shoot by passing the coordinates
    // What happens when I activate the shoot
    protected override void ActivateShoot()
    {
        if(m_pRelatedCapsule)
            m_pRelatedCapsule.gameObject.SendMessage("TeleportItem", m_vNewCenterA, SendMessageOptions.DontRequireReceiver);
        return;
    }

    // update the system with the latest stats of the beacon
    protected override bool SpecialCaseShoot()
    {
        return InternalSetup();
    }

    // This is used to setup the offsets
    protected override bool InternalSetup()
    {
        if (!m_pRelatedObject)
            return false;

        Vector3 objectiveCollider = m_pRelatedObject.collider.bounds.size;
        m_ObjectOffsets = new Vector3(objectiveCollider.x, objectiveCollider.y, objectiveCollider.z);
        m_ObjectOffsetsWeight = new Vector3(0.51f, 0.51f, 0.51f);
        return true;
    }

    // Do a preprocess and post process of the Special case activate
    protected override bool SpecialCaseActivate()
    {
        m_pRelatedObject.gameObject.layer = LayerMask.NameToLayer("TeleportExclude");
        bool bReturn = base.SpecialCaseActivate();
        m_pRelatedObject.gameObject.layer = LayerMask.NameToLayer("Teleportable");
        return bReturn;
    }
}
