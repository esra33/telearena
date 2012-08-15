using UnityEngine;
using System.Collections;

/*
 * Simple gui system to go from scenario to scenario based on an achieved position
*/
public class SceneChangeManager : MonoBehaviour {

    bool m_bChangeScene = false;

    void OnGUI()
    {
        if (!m_bChangeScene)
            return;

        GUILayout.BeginArea(new Rect(Screen.width * 0.5f - Screen.width * 0.1f, Screen.height * 0.5f - Screen.height * 0.05f, Screen.width * 0.2f, Screen.height * 0.1f));

        if (Application.levelCount - 1 > Application.loadedLevel)
        {
            if (GUILayout.Button("Congratulations/nMove to the next level"))
                Application.LoadLevel(Application.loadedLevel + 1);
        }
        else
        {
            GUILayout.Label("GameOver");
        }
        
        GUILayout.EndArea();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        m_bChangeScene = true;
        MouseLook[] mouseLook = other.gameObject.GetComponentsInChildren<MouseLook>();
        foreach (MouseLook ml in mouseLook)
        {
            ml.enabled = false;
        }
        other.gameObject.active = false; // Deactivate Movement
        Screen.lockCursor = false;
        Screen.showCursor = true;
        gameObject.SendMessage("EndStateReached", SendMessageOptions.DontRequireReceiver);
    }
}
