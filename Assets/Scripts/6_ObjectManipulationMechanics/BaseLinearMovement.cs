using UnityEngine;
using System.Collections;

/*
 * This script will apply a force to an object when it gets into 
 * a trigger. The force will be applied in the up direction of the object
*/

public enum eApplicationMoment{
    
    None = 0,
    TriggerEnter = 1,
    ConstantTrigger = 1 << 1,

}

public class BaseLinearMovement : MonoBehaviour {

    // strength of the force
    public float m_ForceStrenght = 10.0f;
    public LayerMask m_LayersToIgnore = 0;
    eApplicationMoment m_ApplicationMoment = eApplicationMoment.None;
    public eApplicationMoment[] m_ApplicationMoments = { eApplicationMoment.None, eApplicationMoment.None };

    void Start()
    {
        // invert the layers
        m_LayersToIgnore = ~m_LayersToIgnore;

        if (m_ApplicationMoments.Length == 1)
            m_ApplicationMoment = m_ApplicationMoments[0];
        else if (m_ApplicationMoments.Length > 1)
            m_ApplicationMoment = m_ApplicationMoments[0] | m_ApplicationMoments[1];
    }
    
    protected virtual void HandlePlayer(GameObject player, float p)
    {
        CharacterMotor motor = player.GetComponent<CharacterMotor>();
        if (motor == null)
            return;

        motor.SetVelocity(motor.GetVelocity() + m_ForceStrenght * transform.up * p);
    }

    protected virtual void HandleGeneric(GameObject genericObject, float p)
    {
        if(genericObject.rigidbody != null)
        {
            genericObject.rigidbody.velocity = genericObject.rigidbody.velocity + m_ForceStrenght * transform.up * p;
        }
    }

    protected GameObject BaseCheck(Collider other, eApplicationMoment enumValue)
    {
        // check that we can use this trigger event
        if ((m_ApplicationMoment & enumValue) == 0)
            return null;

        // test for ignore layers
        if ((other.gameObject.layer & m_LayersToIgnore) == 0)
            return null;

        return other.gameObject;
    }

	void OnTriggerEnter(Collider other)
    {
        GameObject obj = BaseCheck(other, eApplicationMoment.TriggerEnter);
        if (obj == null)
            return;

        // Handle the player in a different way
        if(other.gameObject.tag == "Player")
        {
            HandlePlayer(obj, 1);
            return;
        }

        HandleGeneric(obj, 1);
    }

    void OnTriggerStay(Collider other)
    {
        GameObject obj = BaseCheck(other, eApplicationMoment.ConstantTrigger);
        if (obj == null)
            return;

        // Handle the player in a different way
        if (other.gameObject.tag == "Player")
        {
            HandlePlayer(obj, Time.deltaTime);
            return;
        }

        HandleGeneric(obj, Time.deltaTime);
    }
}
