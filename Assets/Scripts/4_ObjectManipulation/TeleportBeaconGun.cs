using UnityEngine;
using System.Collections;

public class TeleportBeaconGun : GunInterface
{
    // related teleport gun
    public ObjectTeleportGun m_pMyTeleportGun;

    // Special shoot behavior
    protected override void SpecialShoot()
    {
        //Debug.Break();
        //Time.timeScale = 0.03f;
        // setup the shoot
        ObjectTeleportBeacon beacon = m_pCurrentShoot.gameObject.GetComponent<ObjectTeleportBeacon>();
        beacon.SetTeleportGun(m_pMyTeleportGun);
        beacon.SetOriginalGun((GunInterface)this);
        return;
    }
}
