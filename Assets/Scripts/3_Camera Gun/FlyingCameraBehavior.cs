using UnityEngine;
using System.Collections;

public enum VIEWPOINT_STATE
{
    OPENING,
    OPEN,
    CLOSING,
    CLOSE,
}

public class FlyingCameraBehavior : MonoBehaviour {

    // threshold
    public float m_ThresholdPMax = 0.95f;
    public float m_ThresholdPMin = 0.01f;

    float m_maxSizeX;
    float m_maxSizeY;
    VIEWPOINT_STATE m_State = VIEWPOINT_STATE.CLOSE;
    public VIEWPOINT_STATE state
    {
        get { return m_State; }
    }

    Vector3 m_LerpOrigen;

    void Start()
    {
        m_maxSizeX = transform.localScale.x;
        m_maxSizeY = transform.localScale.y;

        transform.localScale = Vector3.zero;
        m_State = VIEWPOINT_STATE.CLOSE;
        m_LerpOrigen = new Vector3(m_ThresholdPMin * m_maxSizeX, m_ThresholdPMin * m_maxSizeY, 1);
    }

    public void SQOpen()
    {
        guiTexture.enabled = true;
        m_State = VIEWPOINT_STATE.OPENING;

        m_LerpOrigen = transform.localScale;
    }

    public void SQClose()
    {
        if (m_State == VIEWPOINT_STATE.CLOSE)
            return;

        m_State = VIEWPOINT_STATE.CLOSING;
    }

    void Update()
    {
        // Exclusion options
        if (m_State == VIEWPOINT_STATE.OPEN || m_State == VIEWPOINT_STATE.CLOSE)
            return;

        Vector3 vecScale = transform.localScale;
        if(m_State == VIEWPOINT_STATE.OPENING)
        {
            // Open the viewPort
            if (transform.localScale.x < m_ThresholdPMax*m_maxSizeX)
            {
                vecScale.x = m_maxSizeX;
            }
            else if (transform.localScale.y < m_ThresholdPMax * m_maxSizeY)
            {
                m_LerpOrigen.x = m_maxSizeX;
                vecScale.x = m_maxSizeX;
                transform.localScale = vecScale;
                vecScale.y = m_maxSizeY;
            }
            else
            {
                m_State = VIEWPOINT_STATE.OPEN;
                m_LerpOrigen = transform.localScale;
                return;
            }
        }

        if (m_State == VIEWPOINT_STATE.CLOSING)
        {
            // close the viewPort
            if (transform.localScale.y > (1 - m_ThresholdPMax) * m_maxSizeY)
            {
                vecScale.y = m_ThresholdPMin * m_maxSizeY;
            }
            else if (transform.localScale.x > (1 - m_ThresholdPMax) * m_maxSizeX)
            {
                m_LerpOrigen.y = m_ThresholdPMin * m_maxSizeY;
                vecScale.y = m_LerpOrigen.y;
                transform.localScale = vecScale;
                vecScale.x = m_ThresholdPMin * m_maxSizeX;
            }
            else
            {
                m_State = VIEWPOINT_STATE.CLOSE;
                guiTexture.enabled = false;
                m_LerpOrigen = transform.localScale;
                return;
            }
        }

        // Update the system
        float pdOC = Vector3.Distance(transform.localScale, m_LerpOrigen);
        transform.localScale = Vector3.Lerp(m_LerpOrigen, vecScale, Time.deltaTime + pdOC / Vector3.Distance(vecScale, m_LerpOrigen));
    }
}