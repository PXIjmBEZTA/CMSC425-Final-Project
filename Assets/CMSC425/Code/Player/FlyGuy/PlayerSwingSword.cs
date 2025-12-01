using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerSwingSword : MonoBehaviour
{
    [Header("Sword Settings")]
    public GameObject sword;
    public float swingDuration = 0.4f;
    public float swingCooldown = 1f;
    public float swordDistance = 1;

    [Header("Controls")]
    public ButtonControl swordButton;
    public KeyControl secondSwordButton;


    private bool isSwinging = false;
    private float nextSwingTime = 0f;
    private Quaternion startRotation;
    private TrailRenderer trail;
    private int swordDirection = 1; //1 means CW, -1 means CCW
    void Start()
    {
        startRotation = sword.transform.localRotation;
        swordButton = Mouse.current.rightButton;
        secondSwordButton = Keyboard.current[Key.K];
        if (sword == null)
            Debug.LogWarning("Sword is not assigned! - PlayerSwingSword");
        sword.transform.localPosition = new Vector3(0, 0, -swordDistance); //swing from back to front clockwise
        sword.transform.localRotation = Quaternion.identity;
        sword.SetActive(false);

        trail = sword.GetComponent<TrailRenderer>();
        if (trail)
        {
            trail.enabled = false; //initially disabled
            trail.startWidth = 0.3f;
            trail.endWidth = 0;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (swordButton == null) return;

        if ((swordButton.isPressed || secondSwordButton.isPressed) && !isSwinging && Time.time >= nextSwingTime)
        {
            StartCoroutine(SwingSword());
        }
    }

    private IEnumerator SwingSword()
    {
        isSwinging = true;
        nextSwingTime = Time.time + swingCooldown;
        sword.SetActive(true);

        trail.Clear();
        trail.enabled = true;

        float elapsed = 0f;
        while (elapsed < swingDuration)
        {
            elapsed += Time.deltaTime;
            float anglePerSecond = 360 * swordDirection / swingDuration;
            sword.transform.RotateAround(transform.position, Vector3.up, anglePerSecond * Time.deltaTime);
            yield return null;
        }
        swordDirection = -swordDirection; //swap directions
        trail.enabled = false;
        sword.transform.localPosition = new Vector3(0, 0, -swordDistance);
        sword.transform.localRotation = startRotation;
        sword.SetActive(false);
        isSwinging = false;
    }
}
