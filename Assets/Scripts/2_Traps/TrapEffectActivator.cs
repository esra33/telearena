using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Note: When activated a trap cannot be deactivated by leaving before the delay finishes
 * thus the process cannot be stopped 
*/
public class TrapEffectActivator : MonoBehaviour {

    // Configuration
    // when to use
    public eApplicationMoment[] m_ApplicationMoments = { eApplicationMoment.None, eApplicationMoment.None };
    eApplicationMoment m_appMoment = eApplicationMoment.None;

    // when to deactivate
    public bool m_bDeactivateOnExit = true;

    // activation cooldown
    public float m_ActivationDelay = 0; // a value of -1 means destruction after usage
    float m_Delay = -1;

    // deactivation cooldown
    public float m_DeactivationCooldown = -1; // a value of -1 means destruction after usage
    float m_Cooldown = -1;

    // One shoot trap
    public bool m_bOneShoot = false;

    // List of observer traps
    public List<ITrapEffect> m_RelatedTraps;
    List<ITrapEffect> m_ToRemove = new List<ITrapEffect>();

    void Start()
    {
        if(m_ApplicationMoments.Length == 1)
        {
            m_appMoment = m_ApplicationMoments[0];
            return;
        }

        if (m_ApplicationMoments.Length > 1)
        {
            m_appMoment = m_ApplicationMoments[0] | m_ApplicationMoments[1];
        }

        foreach (ITrapEffect trap in m_RelatedTraps)
        {
            trap.RegisterActivator(this);
        }
        
    }

    // simple cooldown corutine
    IEnumerator SystemCooldown()
    {
        while (m_Cooldown + m_Delay > 0)
        {
            yield return new WaitForEndOfFrame();
            if (m_Delay <= 0)
                m_Cooldown -= Time.deltaTime;
        }

        // deactivate traps
        foreach (ITrapEffect trap in m_RelatedTraps)
        {
            trap.DeactivateTrap(this);
        }

        // remove observers if any
        m_ToRemove.RemoveAll(RemoveSubscriptor);

        // destroy if CD is -1
        if (m_bOneShoot)
            Destroy(gameObject); // may need to change to this
        
        yield return 0;
    }

    // simple cooldown corutine
    IEnumerator ActivationDelay()
    {
        while (m_Delay > 0)
        {
            yield return new WaitForEndOfFrame();
            m_Delay -= Time.deltaTime;
        }
                
        // activate traps
        foreach (ITrapEffect trap in m_RelatedTraps)
        {
            trap.ActivateTrap(this);
        }

        // remove observers if any
        m_ToRemove.RemoveAll(RemoveSubscriptor);

        yield return 0;
    }

    void StartCD()
    {
        m_Cooldown = m_DeactivationCooldown;
        StartCoroutine(SystemCooldown());
    }

    bool ActivateTraps(eApplicationMoment moment, string tag)
    {
        if ((moment & m_appMoment) == 0 || tag != "Player")
            return false;

        // start delay
        if (m_Delay <= 0)
        {
            m_Delay = m_ActivationDelay;
            StartCoroutine(ActivationDelay());
            m_Cooldown = !m_bDeactivateOnExit ? -1 : m_Cooldown;
        }

        // start CD 
        if (!m_bDeactivateOnExit && m_Cooldown <= 0)
            StartCD();

        return true;
    }

    bool RemoveSubscriptor(ITrapEffect trap)
    {
        m_RelatedTraps.Remove(trap);
        return true;
    }

    public void RemoveFromList(ITrapEffect trap)
    {
        m_ToRemove.Add(trap);
    }

    void OnTriggerEnter(Collider other)
    {
        ActivateTraps(eApplicationMoment.TriggerEnter, other.tag);        
    }

    void OnTriggerStay(Collider other)
    {
        ActivateTraps(eApplicationMoment.ConstantTrigger, other.tag);        
    }

    void OnTriggerExit(Collider other)
    {
        if(m_bDeactivateOnExit && other.tag == "Player")
        {
            StartCD();
        }
    }
}
