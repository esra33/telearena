using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// this class will gather resources and will manage them

public static class GeneralResourceManager {

    static List<IResource> m_lWorldResources = new List<IResource>();

    public static void RegisterResource(IResource resource)
    {
        if (m_lWorldResources.Contains(resource))
            return;

        m_lWorldResources.Add(resource);
    }

    // check which of our registered objects is the reciever of the message
    public static void FillResource(GUN_SYSTEM relatedGunSystem, RESOURCE_SYSTEM relatedResourceSystem, float fillValue)
    {
        foreach (IResource resource in m_lWorldResources)
        {
            if(resource.gunSystem == relatedGunSystem && resource.resourceSystem == relatedResourceSystem)
            {
                resource.AddBonus(fillValue);
                return;
            }
        }
    }

    // clear the current list
    public static void ClearList()
    {
        m_lWorldResources.Clear();
    }
}

// Use this class as a proxy, may not be needed though
public class ResourceSceneInterface : MonoBehaviour
{
    void OnLevelWasLoaded(int level)
    {
        GeneralResourceManager.ClearList();
    }
}
