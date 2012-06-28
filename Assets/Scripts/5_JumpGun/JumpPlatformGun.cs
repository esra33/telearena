using UnityEngine;
using System.Collections;

public class JumpPlatformGun : GenericTeleportGun{

    public GameObject m_pCurrentPlatformGO = null;
    bool m_bActivate;
    float []m_baseOffsets;
    // What happens when I activate the shoot
    protected override void ActivateShoot()
    {
        m_pCurrentPlatformGO.SendMessage("Activate", m_bActivate, SendMessageOptions.DontRequireReceiver);
        m_pCurrentPlatformGO.transform.position = m_vNewCenterA;

        // Debug
        /*Vector3[] directions = { m_pCurrentPlatformGO.transform.right, m_pCurrentPlatformGO.transform.up, m_pCurrentPlatformGO.transform.forward };
        for (int con = 0; con < 3; con++)
        {
            Debug.DrawLine(m_pCurrentPlatformGO.transform.position, m_pCurrentPlatformGO.transform.position - directions[con] * m_baseOffsets[con] * 0.5f, Color.blue);
            Debug.DrawLine(m_pCurrentPlatformGO.transform.position, m_pCurrentPlatformGO.transform.position + directions[con] * m_baseOffsets[con] * 0.5f, Color.blue);
        }

        Debug.Break();*/
    }

    void OrganiceOffsets()
    {
        Vector3[] directions = { m_pCurrentPlatformGO.transform.right, m_pCurrentPlatformGO.transform.up, m_pCurrentPlatformGO.transform.forward };
        m_ObjectOffsets = Vector3.zero;
        Vector3 calculatedOffset;

        for(int con = 0; con < 3; con++)
        {
            //Debug.Log(directions[con] + " || " + m_baseOffsets[con]);
            calculatedOffset = directions[con] * m_baseOffsets[con];
            m_ObjectOffsets.x = Mathf.Max(Mathf.Abs(calculatedOffset.x), Mathf.Abs(m_ObjectOffsets.x));
            m_ObjectOffsets.y = Mathf.Max(Mathf.Abs(calculatedOffset.y), Mathf.Abs(m_ObjectOffsets.y));
            m_ObjectOffsets.z = Mathf.Max(Mathf.Abs(calculatedOffset.z), Mathf.Abs(m_ObjectOffsets.z));
        }

        m_ObjectOffsetsWeight = new Vector3(0.51f, 0.51f, 0.51f);
    }

    // This is used to setup the offsets
    protected override bool InternalSetup()
    {
        // Get the original disposition values, otherwise the values are changed when reordering the normal
        Vector3 objectiveCollider = m_pCurrentPlatformGO.collider.bounds.size;
        m_baseOffsets = new float[3]{ objectiveCollider.x, objectiveCollider.y, objectiveCollider.z };
        OrganiceOffsets();
        m_pCurrentPlatformGO.layer = LayerMask.NameToLayer("Teleportable");
        m_pCurrentPlatformGO.SendMessage("SetLayersToIgnore", m_ExcludedLayers, SendMessageOptions.DontRequireReceiver);
        return true;
    }

    // Do a preprocess and post process of the Special case activate
    protected override bool SpecialCaseActivate()
    {
        Vector3 norm;
        m_bActivate = m_pCurrentShoot.gameObject.GetComponent<LastCollitionBeacon>().DepolyPlatform(m_baseOffsets[1], out norm);
        m_pCurrentPlatformGO.transform.up = norm;

        OrganiceOffsets();

        m_pCurrentPlatformGO.layer = LayerMask.NameToLayer("TeleportExclude");
        bool bReturn = base.SpecialCaseActivate();
        m_pCurrentPlatformGO.layer = LayerMask.NameToLayer("Teleportable");
        return bReturn;
    }
}