using UnityEngine;
using System.Collections;

public class AmmoResource : IResource {

    // Setup the resource variable to use
    public override void SetupResource()
    {
        m_ResourceSystem = RESOURCE_SYSTEM.AMMUNITION;
    }

    // what happens when we use this resource
    public override void UseResource()
    {
        m_CurrentResourceValue -= m_StandarResourceCost; // should be 0
    }

    // what happens when we get input
    public override void AddBonus(float bonus)
    {
        m_CurrentResourceValue = Mathf.Min(m_CurrentResourceValue + bonus + m_StandarReplenishmentRate, m_MaxResourceValue);
    }
}
