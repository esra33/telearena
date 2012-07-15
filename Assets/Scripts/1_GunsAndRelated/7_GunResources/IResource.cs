using UnityEngine;
using System.Collections;

public enum GUN_SYSTEM
{
    NONE,
    PERSONAL_TELEPORT,
    OBJECT_TELEPORT,
    OBJECT_TELEPORT_MARKER,
    CAMERA_GUN,
    JUMP_PLATFORM_GUN,
}

public enum RESOURCE_SYSTEM
{
    NONE,
    COOLDOWN,
    ENERGY,
    AMMUNITION,
}

public enum EXHAUSTION_STATE
{
    ON_SHOOT,
    ON_ACTIVATION,
}

// This is the resource interface, resources are created inhereting from this interface

public abstract class IResource : MonoBehaviour {

    // floats are 32b == int therefore the memory value is exactly the same
    // A resource is unusable if its ussage cost is greater than the current value
    // A resource can only reach as much as its maximum value
    public float m_CurrentResourceValue = 0;
    public float m_MaxResourceValue = 0;
    public float m_StandarReplenishmentRate = 0;
    public float m_StandarResourceCost = 0; // Exhaustion 

    public EXHAUSTION_STATE m_ExhaustionState = EXHAUSTION_STATE.ON_SHOOT;

    protected GUN_SYSTEM m_RelatedGunSystem;

    // Accessor
    public GUN_SYSTEM gunSystem
    {
        get { return m_RelatedGunSystem; }
    }

    protected RESOURCE_SYSTEM m_ResourceSystem;

    // Accessor
    public RESOURCE_SYSTEM resourceSystem
    {
        get { return m_ResourceSystem; }
    }

    public bool ResourceReady()
    {
        return m_CurrentResourceValue - m_StandarResourceCost >= 0;
    }

    // Assign the origin of the gun we are using
    protected void CheckGun(GunInterface pGun)
    {
        // System table to check what is the source of our gun

        if( pGun is PersonalTeleportGun)
        {
            m_RelatedGunSystem = GUN_SYSTEM.PERSONAL_TELEPORT;
            return;
        }

        if (pGun is ObjectTeleportGun)
        {
            m_RelatedGunSystem = GUN_SYSTEM.OBJECT_TELEPORT;
            return;
        }

        if (pGun is TeleportBeaconGun)
        {
            m_RelatedGunSystem = GUN_SYSTEM.OBJECT_TELEPORT_MARKER;
            return;
        }

        if (pGun is CameraTeleportGun)
        {
            m_RelatedGunSystem = GUN_SYSTEM.CAMERA_GUN;
            return;
        }

        if (pGun is JumpPlatformGun)
        {
            m_RelatedGunSystem = GUN_SYSTEM.JUMP_PLATFORM_GUN;
            return;
        }
        
    }

    void Start()
    {
        SetupResource();
        CheckGun(transform.parent.GetComponent<GunInterface>().AddResource(this));
        GeneralResourceManager.RegisterResource(this);
    }

    // Setup the resource variable to use
    public abstract void SetupResource(); 
    
    // what happens when we use this resource
    public abstract void UseResource();

    // what happens when we get input
    public abstract void AddBonus(float bonus);
}
