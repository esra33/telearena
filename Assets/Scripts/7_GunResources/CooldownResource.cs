using UnityEngine;
using System.Collections;

public class CooldownResource : IResource {

    // We can asume that a gun may have multiple instances of cooldowns (or multiple taps)
    // So the designer can setup the cooldown as he thinks it is best (one or multiple taps).

    // Setup the resource variable to use
    public override void SetupResource()
    {
        m_ResourceSystem = RESOURCE_SYSTEM.COOLDOWN;
    }

    // Cooldown corutine
    IEnumerator StartCoolDown()
    {
        while(m_CurrentResourceValue < m_MaxResourceValue)
        {
            m_CurrentResourceValue = Mathf.Min(m_CurrentResourceValue + m_StandarReplenishmentRate * Time.deltaTime, m_MaxResourceValue);
            yield return new WaitForEndOfFrame();
        }

        yield return 0;
    }

    // what happens when we use this resource
    public override void UseResource()
    {
        m_CurrentResourceValue -= m_StandarResourceCost; // should be 0
        StartCoroutine(StartCoolDown());
    }

    // what happens when we get input
    public override void AddBonus(float bonus)
    {
        m_CurrentResourceValue = Mathf.Min(m_CurrentResourceValue + bonus, m_MaxResourceValue);
    }

}
