//#define USE_DEBUG

using UnityEngine;
using System.Collections;

public delegate float Ecuation(float a, float b);
public abstract class GenericTeleportGun : GunInterface
{
    // Maximum check range value when doing the teleport raycast check
    public float m_DefaultMaximumValue = Mathf.Infinity;
    public LayerMask m_ExcludedLayers = 1 << 8 | 1 << 9;

    // Check kernel 
    float[] m_vKernel;
    Vector3 m_vNewCenter;

    // accessor only gets
    protected Vector3 m_vNewCenterA
    {
        get { return m_vNewCenter; }
    }

    // Base values to determine a new position
    Vector3 m_Max;
    Vector3 m_Min;

    // Movement calculation values
    protected Vector3 m_ObjectOffsets;
    protected Vector3 m_ObjectOffsetsWeight;

    // Restart base values
    void ResetTestValues()
    {
        // set the base values
        m_Max = -m_DefaultMaximumValue*Vector3.one;
        m_Min = m_DefaultMaximumValue*Vector3.one;
    }

    protected override void privateStart()
    {
        m_vKernel = new float[3] { 0, 0.5f, -0.5f };
        ResetTestValues();

        // do the internal gun setup to get the calculation values
        InternalSetup();
    }
	
	float GetGreatestDimention(){return Mathf.Max(Mathf.Max(m_ObjectOffsets.x * m_ObjectOffsetsWeight.x, m_ObjectOffsets.y * m_ObjectOffsetsWeight.y), m_ObjectOffsets.z * m_ObjectOffsetsWeight.z);}
    float Greater(float a, float b){return a > b? a : b;}
    float Smaller(float a, float b){return a < b? a : b;}

    // Assign the min max values to determine the space around the beacon
    bool CheckDifferentValues(Vector3 target, Vector3 origen, Vector3 normalComp, ref Vector3 value)
    {
        if(target != origen)
        {
            float dto = Vector3.Dot(target, normalComp) - Vector3.Dot(origen, normalComp);
            float dvo = Vector3.Dot(value, normalComp) - Vector3.Dot(origen, normalComp);

            // check region
            if(dto/Mathf.Abs(dto) == dvo/Mathf.Abs(dvo))
            {
                // check value
                if (Mathf.Abs(dto) < Mathf.Abs(dvo))
                {
                    Vector3 opNormal = Vector3.one - normalComp;
                    value = BlendVectors(target, normalComp) + BlendVectors(value, opNormal);
                    return true;
                }
            }
        }

        return false;
    }

    Vector3 EvaluateVectors(Vector3 v, Vector3 u, Ecuation eq){return new Vector3(eq(v.x, u.x), eq(v.y, u.y), eq(v.z, u.z));}
    Vector3 BlendVectors(Vector3 v, Vector3 u){ return new Vector3(v.x * u.x, v.y * u.y, v.z * u.z);}

    bool CalculateMovement(float M, float m, float r, float o, out Vector3 result, Vector3 reference)
    {
        bool bResult = true;
        result = Vector3.zero;
        if (Mathf.Abs(M - m) <= 2 * r)
        {
            bResult = false;
        }

        if (Mathf.Abs(M - o) > Mathf.Abs(m - o))
            result = (o - m) * reference;
        else
            result = (o - M) * reference;

        if (result.magnitude > r)
        {
            result = Vector3.zero;
            return bResult;
        }

        result = (r - result.magnitude) * result.normalized;

        return bResult;
    }

    // used to get the best position between a maximum and a minimum based on a radius and an origin
    bool CalculateMovement(Vector3 M, Vector3 m, float r, Vector3 o, out Vector3 result, Vector3 reference)
    {
        // first filter
        if (Vector3.Distance(M, m) <= 2 * r)
        {
            result = Vector3.zero;
            return false;
        }

        // Before returning true
        Vector3 maxVector;// 	= Vector3.one * m_DefaultMaximumValue;
       	Vector3	minVector;// 	= -maxVector;

        // Get the opose of the reference vector
        Vector3 vOpose = Vector3.one - reference;
        Vector3 normalM	;
		Vector3 normalm	;
		Vector3 offset, vDirection;
		
        bool bReady;
        RaycastHit hit;
        LayerMask mask = ~(m_ExcludedLayers | 1 << LayerMask.NameToLayer("Ignore Raycast"));
		
		bool firstM;
		bool firstm;
		int attempts = 2;
		
		do
		{
			firstM = true;
			firstm = true;
			maxVector 	= Vector3.one * m_DefaultMaximumValue;
       		minVector 	= -maxVector;
			normalM 	= Vector3.zero;//reference;
			normalm 	= Vector3.zero;//-reference;
			
	        // Cast the rays
	        for(int conZ = 0; conZ < 3; conZ++)
	        {
	            for(int conX = 0; conX < 3; conX++)
	            {
	                for (int conY = 0; conY < 3; conY++)
	                {
	                   // If nothing has happened at this point then start saving values
	                    vDirection = new Vector3(m_vKernel[conX], m_vKernel[conY], m_vKernel[conZ]);
	                    vDirection = vDirection.normalized;
	                    if (Physics.Raycast(o, vDirection, out hit, Mathf.Infinity, mask))
	                    {
	                        if (Vector3.Dot(hit.normal, reference) != 0)
	                        {
	                            if((firstM || (Vector3.Dot(normalM, -reference)) <  (Vector3.Dot(hit.normal, -reference))) && CheckDifferentValues(hit.point, o, reference, ref maxVector))
								{
									firstM = false;
									normalM = hit.normal;
#if USE_DEBUG
		                       		Debug.DrawLine(o, hit.point, Color.white);
#endif
								}
	                            if((firstm || (Vector3.Dot(normalm, reference)) <  (Vector3.Dot(hit.normal, reference))) && CheckDifferentValues(hit.point, o, reference, ref minVector))
								{
									firstm = false;
									normalm = hit.normal;
#if USE_DEBUG
		                       		Debug.DrawLine(o, hit.point, Color.white);
#endif
								}
	                        }
							
#if USE_DEBUG
                       		Debug.DrawLine(o, hit.point, Color.yellow);
#endif
	                    }
	                }
	            }
	        } // calculates the closest points to this particular o
#if USE_DEBUG
			Debug.DrawLine(o, maxVector, Color.red);
			Debug.DrawLine(o, minVector, Color.red);		
#endif
			
	        // check if we can teleport now
	        bReady = CalculateMovement(Vector3.Dot(maxVector, reference), Vector3.Dot(minVector, reference), r, Vector3.Dot(o, reference), out offset, reference);
	
	        if(!bReady)
	        {
	            bool bTest;
				o += r * (BlendVectors(normalM, vOpose) + BlendVectors(normalm, vOpose)).normalized;
	            bTest = o.x > m.x && o.x < M.x;
	            bTest = bTest && o.y > m.y && o.y < M.y;
	            bTest = bTest && o.z > m.z && o.z < M.z;
	
	            if (!bTest)
	            {
	                result = Vector3.zero;
	                return false; // The instance is imposible
	            }
	
	            if (Vector3.Dot(normalM, -reference) < 1)
	               CalculateMovement(Vector3.Dot(M, reference), Vector3.Dot(minVector, reference), r, Vector3.Dot(o, reference), out offset, reference);
	            else
	               CalculateMovement(Vector3.Dot(maxVector, reference), Vector3.Dot(m, reference), r, Vector3.Dot(o, reference), out offset, reference);
	        }
			
			attempts--;
		} while (!bReady && attempts > 0);
		result = o + offset;
        return bReady;
    }

    // Can I activate the event
    protected override bool SpecialCaseActivate()
    {
        // Capsule cast elements
        LayerMask mask = ~(m_ExcludedLayers | 1 << LayerMask.NameToLayer("Ignore Raycast"));
        Vector3 newCenter;

        Vector3 vDirection;
        RaycastHit hit;

        // Reset test values
        ResetTestValues();

        // Go through the kernel
        for(int conZ = 0; conZ < 3; conZ++)
        {
            for(int conX = 0; conX < 3; conX++)
            {
                for (int conY = 0; conY < 3; conY++)
                {
                   // If nothing has happened at this point then start saving values
                    vDirection = new Vector3(m_vKernel[conX], m_vKernel[conY], m_vKernel[conZ]);
                    vDirection = vDirection.normalized;
                    if (Physics.Raycast(m_pCurrentShoot.position, vDirection, out hit, Mathf.Infinity, mask))
                    {
                        m_Max.x = Mathf.Max(m_Max.x, hit.point.x);
                        m_Max.y = Mathf.Max(m_Max.y, hit.point.y);
                        m_Max.z = Mathf.Max(m_Max.z, hit.point.z);

                        m_Min.x = Mathf.Min(m_Min.x, hit.point.x);
                        m_Min.y = Mathf.Min(m_Min.y, hit.point.y);
                        m_Min.z = Mathf.Min(m_Min.z, hit.point.z);
                        
#if USE_DEBUG
                        Debug.DrawLine(m_pCurrentShoot.position, hit.point, Color.red);
#endif
                    }
                }
            }
        }
#if USE_DEBUG
        Debug.Break();
#endif

        // Now we need to do the calculation of a new position based on the relationship of the gathered points
        newCenter = m_pCurrentShoot.position;
        bool retVal = true;

        // X
        retVal = retVal && CalculateMovement(m_Max, m_Min, m_ObjectOffsets.x * m_ObjectOffsetsWeight.x, newCenter, out newCenter, Vector3.right);
        //Debug.Log(retVal);
		
		//Debug.Log(retVal);
        // Y
        retVal = retVal && CalculateMovement(m_Max, m_Min, m_ObjectOffsets.y * m_ObjectOffsetsWeight.y, newCenter, out newCenter, Vector3.up);
        //Debug.Log(retVal);
        
        //Debug.Log(retVal + " --> " + m_Max.y + " && " + m_Min.y + " && " + (m_ObjectOffsets.y * m_ObjectOffsetsWeight.y) + " ++ " + m_pCurrentShoot.position.y);
        // Z
        retVal = retVal && CalculateMovement(m_Max, m_Min, m_ObjectOffsets.z * m_ObjectOffsetsWeight.z, newCenter, out newCenter, Vector3.forward);
        //Debug.Log(retVal);
        
        //Debug.Log(retVal + " --> " + m_Max.z + " && " + m_Min.z + " && " + (m_ObjectOffsets.z * m_ObjectOffsetsWeight.z) + " ++ " + m_pCurrentShoot.position.z);
		
#if USE_DEBUG		
        Debug.DrawLine(m_pCurrentShoot.position, new Vector3(m_Max.x  + m_pCurrentShoot.position.x * 0, m_Max.y + m_pCurrentShoot.position.y * 0, m_Max.z), Color.blue);
        Debug.DrawLine(m_pCurrentShoot.position, new Vector3(m_Min.x  + m_pCurrentShoot.position.x * 0, m_Min.y + m_pCurrentShoot.position.y * 0, m_Min.z), Color.blue);
#endif

        //if (!retVal)
        //    Debug.Break();

        if(retVal)
            m_vNewCenter = newCenter;

        return retVal;
    }

    // This is used to setup the offsets
    protected abstract bool InternalSetup();
}