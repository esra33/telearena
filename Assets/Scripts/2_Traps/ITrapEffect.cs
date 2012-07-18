using UnityEngine;
using System.Collections;

/*
 * This class is used as an interface to activate a series of traps
 * triggers that activate a moving trap will call functions from this 
 * interface.
*/

public abstract class ITrapEffect : MonoBehaviour {

    protected TrapEffectActivator m_Activator = null;

    public void RegisterActivator(TrapEffectActivator activator)
    {
        // Keep trak of the activators one per trap only
        if (m_Activator != null)
            Debug.Log(gameObject.name + "Switching activator from: " + m_Activator.gameObject.name + " To " + activator.gameObject.name);

        m_Activator = activator;
    }

    public void ActivateTrap(TrapEffectActivator activator)
    {
        if (m_Activator != activator)
            return;

        PrivateActivateTrap();
    }

    public void DeactivateTrap(TrapEffectActivator activator)
    {
        if (m_Activator != activator)
            return;

        PrivateDeactivateTrap();
    }

    protected abstract void PrivateActivateTrap();
    protected abstract void PrivateDeactivateTrap();	
}
