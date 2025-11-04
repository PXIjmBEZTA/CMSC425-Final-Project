using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerSwingSword : MonoBehaviour
{
    [Header("Sword Settings")]
    public GameObject sword;
    public float swingDuration = 0.4f;
    public float swingCooldown = 2.5f;


    [Header("Swing Arc")]
    public float swingAngle = 90f;
    public float swingSpeed = 360f; //Degrees per second

    [Header("Controls")]
    public ButtonControl shootButton;




    private bool isSwinging = false;
    private float nextSwingTime = 0f;
    private Quaternion startRotation;
    private Quaternion endRotation;


    void Start()
    {
        startRotation = Quaternion.Euler(0, 45f, 0);
        endRotation = Quaternion.Euler(0, 45f + swingAngle, 0);
        shootButton = Mouse.current.rightButton;
        if (sword == null)
            Debug.LogWarning("Sword is not assigned! - PlayerSwingSword");
        sword.transform.localRotation = startRotation;
        sword.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (shootButton == null) return;

        if (shootButton.isPressed && !isSwinging && Time.time >= nextSwingTime)
        {
            StartCoroutine(SwingSword());
        }
    }

    private IEnumerator SwingSword()
    {
        isSwinging = true;
        nextSwingTime = Time.time + swingCooldown;
        sword.SetActive(true);


        float elapsed = 0f;
        while (elapsed < swingDuration)
        {
            elapsed += Time.deltaTime; 
            float t = elapsed / swingDuration;
            sword.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        sword.SetActive(false);

        sword.transform.localRotation = startRotation;
        isSwinging = false;
    }
}
