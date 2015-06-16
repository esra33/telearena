using UnityEngine;
using System.Collections;

public class TargetAlineation : MonoBehaviour {

    public int[] m_Position = { 0, 0 };
    public float[] m_Size = { 7, 4 };

    public Color m_initialColor = Color.red;
    public Color m_finalColor = Color.yellow;

    Material m_MyMaterial;

    float m_timer = 0;
    int m_operation = 1;

    void Start()
    {
        m_MyMaterial = (Material)Instantiate(GetComponent<Renderer>().material);

        Vector2 texScale = new Vector2(1 / m_Size[0], 1 / m_Size[1]);
        Vector2 texOffset = new Vector2(m_Position[0] * (1 / m_Size[0]), m_Position[1] * (1 / m_Size[1]));

        m_MyMaterial.mainTextureScale = texScale;
        m_MyMaterial.mainTextureOffset = texOffset;

        m_MyMaterial.SetTextureScale("_BumpMap", texScale);
        m_MyMaterial.SetTextureOffset("_BumpMap", texOffset);

        GetComponent<Renderer>().material = m_MyMaterial;
    }

	// Update is called once per frame
	void Update () {

        m_MyMaterial.color = Color.Lerp(m_initialColor, m_finalColor, m_timer);

        m_timer = m_timer + m_operation * Time.deltaTime;

        if(m_timer < 0)
        {
            m_timer = 0;
            m_operation = 1;
        }

        if(m_timer > 1)
        {
            m_timer = 1;
            m_operation = -1;
        }

	}
}
