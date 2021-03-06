using UnityEngine;
using System.Collections;

public class PersonalTeleportGun : GenericTeleportGun
{
    Transform m_pPlayer = null;

    // What happens when I activate the shoot
    protected override void ActivateShoot()
    {
        // Cast to hit the ground
        m_pPlayer.position = m_vNewCenterA;

        return;
    }

    // This is used to setup the offsets
    protected override bool InternalSetup()
    {
        // Get the character controller
        m_pPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        CharacterController pCharacterController = m_pPlayer.gameObject.GetComponent<CharacterController>();

        // Setup the gun values
        m_ObjectOffsets = new Vector3(pCharacterController.radius * m_pPlayer.lossyScale.x * 2, pCharacterController.height * m_pPlayer.lossyScale.y, pCharacterController.radius * m_pPlayer.lossyScale.z * 2);
        m_ObjectOffsetsWeight = new Vector3(1.13f, 0.53f, 1.13f);

        return true;
    }
}
