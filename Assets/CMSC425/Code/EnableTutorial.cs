using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class EnableTutorial : MonoBehaviour
{
    public TMP_Text controlsText;
    public TMP_Text shootGuyControls;

    private void Start()
    {
        controlsText.enabled = false;
        shootGuyControls.enabled = false;
    }

    public void showControls()
    {
        controlsText.enabled = true;
    }

    public void showShootGuyControls()
    {
        controlsText.enabled = false;
        shootGuyControls.enabled = true;
    }
    public void hideControls()
    {
        controlsText.enabled = false;
        shootGuyControls.enabled = false;  
    }

}
