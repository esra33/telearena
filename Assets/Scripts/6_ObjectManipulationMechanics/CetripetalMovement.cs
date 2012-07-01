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
        if (genericObject.rigidbody != null)
        {
            Vector3 dir = Vector3.Normalize(transform.position - genericObject.transform.position);
            genericObject.rigidbody.velocity = genericObject.rigidbody.velocity + m_ForceStrenght * dir * p;
        }
    }
}
