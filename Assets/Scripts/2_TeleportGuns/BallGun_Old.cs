using UnityEngine;
using System.Collections;

public class BallGun_Old : GunInterface
{
    public float m_DefaultMaximumValue = 10000;

    float[] m_vKernel;
    Vector3 m_vNewCenter;
    CharacterController m_pCharacterController;

    // Base values to determine a new position
    float m_MaxX;
    float m_MaxY;
    float m_MaxZ;

    float m_MinX;
    float m_MinY;
    float m_MinZ;

    // Restart base values
    void ResetTestValues()
    {
        // set the base values
        m_MaxX = m_DefaultMaximumValue;
        m_MaxY = m_MaxX;
        m_MaxZ = m_MaxX;

        m_MinX = -m_MaxX;
        m_MinY = m_MinX;
        m_MinZ = m_MinX;
    }

    protected override void privateStart()
    {
        m_vKernel = new float[3] { 0, 0.5f, -0.5f };
        m_pCharacterController = transform.parent.parent.gameObject.GetComponent<CharacterController>();

        ResetTestValues();
    }

    protected override void ActivateShoot()
    {
        // Cast to hit the ground
        Transform pParent = transform.parent.parent;
        pParent.position = m_vNewCenter;

        return;
    }

    protected override bool SpecialCaseShoot()
    {
        // trow a ray from the parent to the transform and see if it hits anything
        Vector3 dir = transform.parent.position - transform.position;
        if (Physics.Raycast(transform.position, dir.normalized, 1.2f*dir.magnitude))
            return false;
        
        return true;
    }

    bool CheckDifferentValues(float target, float origen, float normalComp, ref float value)
    {
        if (normalComp == 0)
            return false;

        if(target != origen)
        {
            float modTarget = (target/Mathf.Abs(normalComp));
            float dto = modTarget - origen;
            float dvo = value - origen;

            // check region
            if(dto/Mathf.Abs(dto) == dvo/Mathf.Abs(dvo))
            {
                // check value
                if (Mathf.Abs(dto) < Mathf.Abs(dvo))
                {
                    value = target;
                    return true;
                }
            }
        }

        return false;
    }

    // used to get the best position between a maximum and a minimum based on a radius and an origin
    bool CalculateMovement(float M, float m, float r, float o, out Vector3 result, Vector3 reference)
    {
        result = Vector3.zero;
        if (Mathf.Abs(M - m) <= 2 * r)
        {
            return false;
        }

        if(M == Mathf.Infinity && m == -1*Mathf.Infinity)
        {
            result = Vector3.zero;
            return true;
        }

        if (Mathf.Abs(M - o) > Mathf.Abs(m - o))
            result = (o - m) * reference;
        else
            result = (o - M) * reference;

        if (result.magnitude > r)
        {
            result = Vector3.zero;
            return true;
        }

        result = (r - result.magnitude) * result.normalized;

        return true; 
    }

    protected override bool SpecialCaseActivate()
    {
        // Capsule cast elements
        LayerMask mask = 1 << 8 | 1 << 9;
        mask = ~mask;
        //Vector3 point1;
        //Vector3 point2;
        Vector3 newCenter;

        Vector3 vDirection;
        RaycastHit hit;
        //Color col;

        // Reset test values
        ResetTestValues();

        // Go through the kernel
        for(int conZ = 0; conZ < 3; conZ++)
        {
            for(int conX = 0; conX < 3; conX++)
            {
                for (int conY = 0; conY < 3; conY++)
                {
                    // Base calculation to get a fast new position
                    //col = new Color((128.0f + 120.0f * conX) / 256.0f, (128.0f + 120.0f * conY) / 256.0f, (128.0f + 120.0f * conZ) / 256.0f);
                    // Apply the desired capsule
                    /*newCenter = m_vKernel[conY]*Vector3.up*m_pCharacterController.height +
                                m_vKernel[conX]*Vector3.right*m_pCharacterController.radius +
                                m_vKernel[conZ]*Vector3.forward*m_pCharacterController.radius +
                                m_pCurrentShoot.position;

                    // Not really sure why if we are doing a mathematical translation that its compensated it does not work
                    point1 = newCenter - Vector3.up*m_pCharacterController.height*0.35f;
                    point2 = point1 + Vector3.up*m_pCharacterController.height*0.5f;
                    if (!Physics.CheckCapsule(point1, point2, m_pCharacterController.radius, mask))
                    {
                        //Debug.Log(m_vKernel[conX] + "||" + m_vKernel[conY] + "||" + m_vKernel[conZ] + "||" + m_pCurrentShoot.position);
                        //Debug.Break();
                        m_vNewCenter = newCenter;
                        return true;
                    }*/

                    // If nothing has happened at this point then start saving values
                    vDirection = new Vector3(m_vKernel[conX], m_vKernel[conY], m_vKernel[conZ]);
                    vDirection = vDirection.normalized;
                    if (Physics.Raycast(m_pCurrentShoot.position, vDirection, out hit, Mathf.Infinity, mask))
                    {
                        CheckDifferentValues(hit.point.x, m_pCurrentShoot.position.x, hit.normal.x, ref m_MaxX);
                        CheckDifferentValues(hit.point.y, m_pCurrentShoot.position.y, hit.normal.y, ref m_MaxY);
                        CheckDifferentValues(hit.point.z, m_pCurrentShoot.position.z, hit.normal.z, ref m_MaxZ);

                        CheckDifferentValues(hit.point.x, m_pCurrentShoot.position.x, hit.normal.x, ref m_MinX);
                        CheckDifferentValues(hit.point.y, m_pCurrentShoot.position.y, hit.normal.y, ref m_MinY);
                        CheckDifferentValues(hit.point.z, m_pCurrentShoot.position.z, hit.normal.z, ref m_MinZ);

                        //Debug.DrawLine(m_pCurrentShoot.position, hit.point, Color.red);
                    }
                }
            }
        }
        //Debug.Break();

        // Now we need to do the calculation of a new position based on the relationship of the gathered points
        Vector3 baseVector = Vector3.zero;
        newCenter = m_pCurrentShoot.position;
        bool retVal = true;

        // X
        retVal = retVal && CalculateMovement(m_MaxX, m_MinX, m_pCharacterController.radius*1.01f, m_pCurrentShoot.position.x, out baseVector, Vector3.right);
        //Debug.Log(retVal);
        newCenter += baseVector;
        // Y
        retVal = retVal && CalculateMovement(m_MaxY, m_MinY, m_pCharacterController.height * 0.51f, m_pCurrentShoot.position.y, out baseVector, Vector3.up);
        //Debug.Log(retVal);
        newCenter += baseVector;
        // Z
        retVal = retVal && CalculateMovement(m_MaxZ, m_MinZ, m_pCharacterController.radius*1.01f, m_pCurrentShoot.position.z, out baseVector, Vector3.forward);
        //Debug.Log(retVal);
        newCenter += baseVector;

        if(retVal)
            m_vNewCenter = newCenter;

        return retVal;
    }
}