using UnityEngine;
using System.Collections;

public class RotatorAnimation : ITrapEffect
{
    public GameObject[] m_DoorAnimations = { null, null };

    // Open --> TrapDoorAnimation1
    // Close --> TrapDoorAnimation2

    protected override void PrivateActivateTrap()
    {
        foreach (GameObject anim in m_DoorAnimations)
        {
            anim.GetComponent<Animation>().Play("TrapDoorAnimation1");
        }
    }

    protected override void PrivateDeactivateTrap()
    {
        foreach (GameObject anim in m_DoorAnimations)
        {
            anim.GetComponent<Animation>().Play("TrapDoorAnimation2");
        }
    }
}
