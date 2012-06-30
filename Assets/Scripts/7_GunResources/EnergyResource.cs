using UnityEngine;
using System.Collections;

public class EnergyResource : IResource {

    public float m_BonusDuration = 0.3f; // ~300ms
    float m_CurrentBonusValue = 0;
    float m_InternalBonusDuration = 0;
    bool m_bCorutineStarted = false;

    // Setup the resource variable to use
    public override void SetupResource()
    {
        m_ResourceSystem = RESOURCE_SYSTEM.ENERGY;
    }

    // Cooldown corutine
    IEnumerator UseBonusEffect()
    {
        while (m_InternalBonusDuration > 0)
        {
            m_InternalBonusDuration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        m_bCorutineStarted = false;
        m_InternalBonusDuration = 0;
        m_CurrentBonusValue = 0;
        yield return 0;
    }

    // what happens when we use this resource
    public override void UseResource()
    {
        m_CurrentResourceValue -= m_StandarResourceCost; // should be 0
    }

    void Update()
    {
        m_CurrentResourceValue = Mathf.Min(m_CurrentResourceValue + (m_StandarReplenishmentRate + m_CurrentBonusValue) * Time.deltaTime, m_MaxResourceValue);
    }

    // what happens when we get input
    public override void AddBonus(float bonus)
    {
        if (!m_bCorutineStarted)
        {
            StartCoroutine(UseBonusEffect());
            m_bCorutineStarted = true;
        }

        m_CurrentBonusValue = bonus;
        m_InternalBonusDuration = m_BonusDuration;  
    }
}
