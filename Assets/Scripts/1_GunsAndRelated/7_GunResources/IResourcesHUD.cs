using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IResourcesHUD : MonoBehaviour {

    protected GunInterface m_pThisGun;
    
    // Use this for initialization
	void Start () {
        m_pThisGun = gameObject.GetComponent<GunInterface>();
	}
	
    void OnGUI()
    {
        if (!ProcessComponent())
            return;

        ProcessAllResources(OnGUIPreprocess);
    }

    void Update()
    {
        if (!ProcessComponent())
            return;

        ProcessAllResources(OnUpdatePreprocess);
    }

    bool ProcessComponent()
    {
        return m_pThisGun != null && transform.parent.renderer.enabled;
    }

    protected delegate void ProcessResource(IResource resource);

    protected void ProcessAllResources(ProcessResource resourceFunct)
    {
        if (m_pThisGun.onShootResources != null)
        {
            foreach (IResource resource in m_pThisGun.onShootResources)
            {
                resourceFunct(resource);
            }
        }
        if (m_pThisGun.onActivateResources != null)
        {
            foreach (IResource resource in m_pThisGun.onActivateResources)
            {
                resourceFunct(resource);
            }
        }
    }
    
    protected virtual void OnGUIPreprocess(IResource resource)
    {
        // simple display gui function
        GUILayout.Label(resource.GetType().ToString() + ": " + resource.m_CurrentResourceValue + " / " + resource.m_MaxResourceValue);
    }

    protected virtual void OnUpdatePreprocess(IResource resource)
    {

    }



}
