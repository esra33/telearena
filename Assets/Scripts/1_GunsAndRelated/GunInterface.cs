using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WEAPON_SWITCH
{
    ACTIVATING,
    DEACTIVATING,
}

public class GunInterface : MonoBehaviour
{
    public Transform m_pBaseShoot = null;
    public float m_BaseForce;
    protected Transform m_pCurrentShoot = null;

    // Resources
    List<IResource> m_lOnShootResources = null;
    List<IResource> m_lOnActivateResources = null;

    // Accessors
    public IResource [] onShootResources
    {
        get {
            if (m_lOnShootResources != null)
                return m_lOnShootResources.ToArray();
            else
                return null;
        }
    }

    public IResource[] onActivateResources
    {
        get
        {
            if (m_lOnActivateResources != null)
                return m_lOnActivateResources.ToArray();
            else
                return null;
        }
    }

    // Add resources to the lists
    public GunInterface AddResource(IResource pResource)
    {
        // use on shoot
        if(pResource.m_ExhaustionState == EXHAUSTION_STATE.ON_SHOOT)
        {
            if (m_lOnShootResources == null)
                m_lOnShootResources = new List<IResource>();
            m_lOnShootResources.Add(pResource);
        }
        else
        {
            // use on activation
            if (m_lOnActivateResources == null)
                m_lOnActivateResources = new List<IResource>();
            m_lOnActivateResources.Add(pResource);
        }

        return this;
    }


    // Reset shoot
    public void ResetShoot()
    {
        m_pCurrentShoot = null;
    }

    // Start the system
    void Start()
    {
        privateStart();
    }

    protected virtual void privateStart()
    {
        return;
    }

    // Set the Shoot Signal
    public void DoGunShoot()
    {
        if (CanShoot())
            BaseShoot();
    }

    // Set the activation signal
    public void DoGunActivation()
    {
        if (CanActivate())
            Activate();
    }

    // Identify if you can shoot, may use special behavior cases 
    // depending of the shooting solution
    private bool CanShoot()
    {
        bool bShoot = true;
        bShoot = bShoot && BaseCanShoot();
        bShoot = bShoot && SpecialCaseShoot();

        if (m_lOnShootResources == null)
            return bShoot;
        
        foreach(IResource res in m_lOnShootResources)
        {
            bShoot = bShoot && res.ResourceReady();
        }

        return bShoot;
    }

    // Basic elements that allow you to shoot
    private bool BaseCanShoot()
    {
        // trow a ray from the parent to the transform and see if it hits anything
        Vector3 dir = transform.parent.position - transform.position;
        if (Physics.Raycast(transform.position, dir.normalized, 1.2f * dir.magnitude))
            return false;

        // simple check to see if I have something to shoot
        if (m_pBaseShoot == null)
            return false;

        return true;
    }

    protected virtual bool SpecialCaseShoot()
    {
        return true;
    }

    // Special shoot behavior
    protected virtual void SpecialShoot()
    {
        // Special shooting mechanics
        return;
    }

    protected virtual void BaseShoot()
    {
        // Use the shoot resources
        if (m_lOnShootResources != null)
        {
            foreach (IResource res in m_lOnShootResources)
                res.UseResource();
        }

        // Common shooting mechanic
        if (m_pCurrentShoot)
            Destroy(m_pCurrentShoot.gameObject);

        m_pCurrentShoot = (Transform)Instantiate(m_pBaseShoot, transform.position, Quaternion.identity);
        m_pCurrentShoot.GetComponent<Rigidbody>().velocity = transform.forward * m_BaseForce;
        SpecialShoot();
    }

    // Identify if you can shoot, may use special behavior cases 
    // depending of the shooting solution
    private bool CanActivate()
    {
        bool bActivate = true;
        bActivate = bActivate && BaseCanActivate();
        bActivate = bActivate && SpecialCaseActivate();

        if (m_lOnActivateResources == null)
            return bActivate;

        foreach (IResource res in m_lOnActivateResources)
        {
            bActivate = bActivate && res.ResourceReady();
        }

        return bActivate;
    }

    // simple check to see if I have something to shoot
    private bool BaseCanActivate()
    {
       return m_pCurrentShoot && m_pCurrentShoot.gameObject;
    }

    protected virtual bool SpecialCaseActivate()
    {
        return true;
    }

    // Activate the system
    protected void Activate()
    {
        // Use the shoot resources
        if (m_lOnActivateResources != null)
        {
            foreach (IResource res in m_lOnActivateResources)
                res.UseResource();
        }

        ActivateShoot();
        Destroy(m_pCurrentShoot.gameObject);
        return;
    }

    // Activate the system
    protected virtual void ActivateShoot()
    {
        return;
    }

    // Event to do when a weapon is changed
    public virtual void OnWeaponChange(WEAPON_SWITCH swState)
    {
        return;
    }
}