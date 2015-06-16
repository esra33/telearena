using UnityEngine;
using System.Collections;

public class CetripetalMovement : BaseLinearMovement
{
    protected override void HandlePlayer(GameObject player, float p)
    {
        CharacterMotor motor = player.GetComponent<CharacterMotor>();
        if (motor == null)
            return;
        Vector3 dir = Vector3.Normalize(transform.position - player.transform.position);

        motor.SetVelocity(motor.GetVelocity() + m_ForceStrenght * dir * p);
    }

    protected override void HandleGeneric(GameObject genericObject, float p)
    {
        if (genericObject.GetComponent<Rigidbody>() != null && !genericObject.GetComponent<Rigidbody>().isKinematic)
        {
            Vector3 dir = Vector3.Normalize(transform.position - genericObject.transform.position);
            genericObject.GetComponent<Rigidbody>().velocity = genericObject.GetComponent<Rigidbody>().velocity + m_ForceStrenght * dir * p;
        }
    }
}
