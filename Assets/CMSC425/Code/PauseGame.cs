using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    float previousTimeScale = 1;
    public TMP_Text pauseLabel;
    public static bool isPaused = false;

    public KeyCode pauseKeyCode;


    private void Start()
    {
        pauseLabel.enabled = false;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (Time.timeScale > 0)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            AudioListener.pause = true;
            pauseLabel.enabled = true;
            isPaused = true;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = previousTimeScale;
            AudioListener.pause = false;
            pauseLabel.enabled = false;
            isPaused = false;
        }
    }
}
