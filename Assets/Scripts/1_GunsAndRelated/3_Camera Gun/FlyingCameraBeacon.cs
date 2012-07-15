using UnityEngine;
using System.Collections;

/*
 * This object can stay during the whole game as it won't affect the development
 * and will be easier to implement that way.
 */ 

public class FlyingCameraBeacon : MonoBehaviour {

    // This is a direct link to the camera gun manager
    CameraTeleportGun m_pCammeraGun;
    public CameraTeleportGun cameraGun
    {
        set { m_pCammeraGun = value; }
        get { return m_pCammeraGun; }
    }

    // Duration of the camera in seconds
    public float m_CammeraDuration = 3;
    float m_internalDuration;

    // Game controllers
    MouseLook m_pMouseLook;
//     CharacterMotor m_pMotor;
//     FPSInputController m_inputController;
    FreeFPSMovement m_pFPSController;

    // Player connection
    CharacterMotor m_pPlayerMotor;
    MouseLook m_pPlayerMouseLook;
    FPSInputController m_pPlayerinputController;

    // Camera Access
    Camera m_pCamera;

    // Active state
    bool m_bActive = false;
    bool m_bWaitingSD = false;
    
    // Use this for initialization
	void Start () {

        // Get the components of the beacon
        m_pMouseLook = GetComponent<MouseLook>();
        m_pCamera = GetComponent<Camera>();
        m_pFPSController = GetComponent<FreeFPSMovement>();

        m_pMouseLook.enabled = false;
        m_pFPSController.enabled = false;
        m_pCamera.enabled = false;

        // Start the cooldown in 0
        m_internalDuration = 0;

        // Get a connection to the player
        GameObject pPlayer = GameObject.FindGameObjectWithTag("Player");
        m_pPlayerMotor = pPlayer.GetComponent<CharacterMotor>();
        m_pPlayerMouseLook = pPlayer.GetComponent<MouseLook>();
        m_pPlayerinputController = pPlayer.GetComponent<FPSInputController>();

        m_bWaitingSD = false;
	}

    public void ActivateCamera()
    {
        m_bActive = true;
        m_pCamera.enabled = true;
        GetComponent<NoiseEffect>().enabled = true;
        m_internalDuration = 0;
    }

    public void PauseCamera(bool bPause)
    {
        m_bActive = !bPause;

        // Shutdown all components
        m_pMouseLook.enabled = false;
        m_pFPSController.enabled = false;

        // Turn on player components
        m_pPlayerMotor.enabled = true;
        m_pPlayerMouseLook.enabled = true;
        m_pPlayerinputController.enabled = true;
    }

    public void DeactivateCamera()
    {
        m_pCamera.enabled = false;
        GetComponent<NoiseEffect>().enabled = false;

        if (m_bActive)
        {
            // Shutdown all components
            m_pMouseLook.enabled = false;
            m_pFPSController.enabled = false;

            // Turn on player components
            m_pPlayerMotor.enabled = true;
            m_pPlayerMouseLook.enabled = true;
            m_pPlayerinputController.enabled = true;
        }
        m_bActive = false;
    }

    // Follow the update of the camera
    void Update()
    {
        // Wait for the camera to get deactivated
        if (m_bWaitingSD)
        {
            if (m_pCammeraGun.ScreenClosed())
            {
                m_bWaitingSD = false;
                DeactivateCamera();
            }
            return;
        }

        if(m_pCamera.enabled)
        {
            // infinite duration
            if (m_CammeraDuration > -1)
                m_internalDuration += Time.deltaTime;
            else
                return;

            if(m_internalDuration >= m_CammeraDuration)
            {
                // Shutdown the camera system
                m_bWaitingSD = true;
                m_pCammeraGun.ShutDownCamera();
            }
        }
    }
	
    void LateUpdate()
    {
        if (!m_bActive)
            return;

        if (Input.GetButton("Fire2"))
        {
            m_pMouseLook.enabled = true;
            m_pFPSController.enabled = true;

            // turn off the player components
            m_pPlayerMotor.enabled = false;
            m_pPlayerMouseLook.enabled = false;
            m_pPlayerinputController.enabled = false;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            m_pMouseLook.enabled = false;
            m_pFPSController.enabled = false;

            // turn off the player components
            m_pPlayerMotor.enabled = true;
            m_pPlayerMouseLook.enabled = true;
            m_pPlayerinputController.enabled = true;
        }

    }
}
