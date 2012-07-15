using UnityEngine;
using System.Collections;

public class CameraTeleportGun : GunInterface
{
    // We will keep the camera beacon as an object that can be enabled or
    // disabled at will and we will just teleport it to its destination position
    // when required.
    public FlyingCameraBeacon m_pCameraBeacon;
    public FlyingCameraBehavior m_pScreenBehavior;

    public void ShutDownCamera()
    {
        // Turn off the camera beacon
        m_pScreenBehavior.SQClose();
    }

    protected override void privateStart()
    {
        m_pCameraBeacon.cameraGun = this;
        return;
    }

    // Special shoot behavior
    protected override void SpecialShoot()
    {
        m_pCameraBeacon.DeactivateCamera();
        ShutDownCamera();
    }

    protected override bool SpecialCaseActivate()
    {
       // Don't double activate, movement is directly managed in the flying camera beacon
       return m_pCurrentShoot != null;
    }

    // Activate the system
    protected override void ActivateShoot()
    {
        // Position the camera acordingly
        m_pCameraBeacon.transform.position = m_pCurrentShoot.transform.position;
        m_pCameraBeacon.transform.rotation = m_pCurrentShoot.transform.rotation;

        // Open the secondary camera viewer
        m_pScreenBehavior.SQOpen();

        // Activate the camera
        m_pCameraBeacon.ActivateCamera();

        return;
    }

    // Event to do when a weapon is changed
    public override void OnWeaponChange(WEAPON_SWITCH swState)
    {
        m_pCameraBeacon.PauseCamera(swState == WEAPON_SWITCH.DEACTIVATING);
    }

    public bool ScreenClosed()
    {
        return m_pScreenBehavior.state == VIEWPOINT_STATE.CLOSE;
    }
}
