using UnityEngine;
using System.Collections;

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
        // Common shooting mechanic
        if (m_pCurrentShoot)
            Destroy(m_pCurrentShoot.gameObject);

        m_pCurrentShoot = (Transform)Instantiate(m_pBaseShoot, transform.position, Quaternion.identity);
        m_pCurrentShoot.rigidbody.velocity = transform.forward * m_BaseForce;
        SpecialShoot();
    }

    // Identify if you can shoot, may use special behavior cases 
    // depending of the shooting solution
    private bool CanActivate()
    {
        bool bActivate = true;
        bActivate = bActivate && BaseCanActivate();
        bActivate = bActivate && SpecialCaseActivate();
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