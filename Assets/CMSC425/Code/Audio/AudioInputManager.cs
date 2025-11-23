using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class AudioInputManager : MonoBehaviour
{
    public ButtonControl shootButton;

    void Start()
    {
        
        shootButton = Mouse.current.leftButton;
    }
    void Update()
    {
        // if (shootButton.wasPressedThisFrame)
        // {
        //     AudioManager.Instance.Play(AudioManager.SoundType.Shoot);
        // }

        // else if (PlayerShoot)
        // {
        //     AudioManager.Instance.Play(AudioManager.SoundType.BigShoot);

        // }
    }

    public void PlayShootSound()
    {
        Debug.Log("ShootSound played");
        AudioManager.Instance.Play(AudioManager.SoundType.Shoot);
    }

    public void PlayBigShootSound()
    {
        Debug.Log("BIG shoot playeed");
        AudioManager.Instance.Play(AudioManager.SoundType.BigShoot);
    }

    public void PlaySwordSwingSound()
    {
        Debug.Log("SHINGGGG");
        AudioManager.Instance.Play(AudioManager.SoundType.SwingSword);
    }

    public void PlayDashSound()
    {
        Debug.Log("WOOOSHH");
        AudioManager.Instance.Play(AudioManager.SoundType.Dash);
    }
}
