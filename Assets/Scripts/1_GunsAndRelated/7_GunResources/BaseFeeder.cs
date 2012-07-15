using UnityEngine;
using System.Collections;

public class BaseFeeder : MonoBehaviour {

    // when to use
    public eApplicationMoment[] m_ApplicationMoments = { eApplicationMoment.None, eApplicationMoment.None };
    eApplicationMoment m_appMoment = eApplicationMoment.None;

    // activation cooldown
    public float m_ActivationCooldown = -1; // a value of -1 means destruction after usage
    float m_resourceCooldown = -1;

    // Associated gun
    public GUN_SYSTEM m_AssociatedGun;

    // associated resources
    public RESOURCE_SYSTEM[] m_AssociatedResources = null;
    public float[] m_RelatedValues = null; // fill this with the related values for the system

    // Associated usage key
    // <Only usable when using the on trigger stay moment>
    public KeyCode m_KeyCode;

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
    }

    // simple cooldown corutine
    IEnumerator SystemCooldown()
    {
        while(m_resourceCooldown > 0)
        {
            yield return new WaitForEndOfFrame();
            m_resourceCooldown -= Time.deltaTime;
        }

        yield return 0;
    }

    void FillResource(string tag, eApplicationMoment moment)
    {
        if (m_resourceCooldown > 0)
            return;

        if (tag == "Player" && (m_appMoment & moment) != 0)
        {
            int minlength = Mathf.Min(m_AssociatedResources.Length, m_RelatedValues.Length);

            for (int con = 0; con < minlength; con++)
            {
                GeneralResourceManager.FillResource(m_AssociatedGun, m_AssociatedResources[con], m_RelatedValues[con]);
            }

            if (m_ActivationCooldown < 0)
                Destroy(gameObject);

            m_resourceCooldown = m_ActivationCooldown;
            StartCoroutine(SystemCooldown());

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (m_AssociatedGun == GUN_SYSTEM.NONE)
            return;

        FillResource(other.transform.tag, eApplicationMoment.TriggerEnter);
    }

    void OnTriggerStay(Collider other)
    {
        // the key was not pressed and it was assigned
        if (m_KeyCode != KeyCode.None && !Input.GetKeyDown(m_KeyCode))
            return;

        if (m_AssociatedGun == GUN_SYSTEM.NONE)
            return;

        FillResource(other.transform.tag, eApplicationMoment.ConstantTrigger);
    }
    
}
