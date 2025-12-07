using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MovePlayer : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 4;
    public Key forwardMove = Key.W;
    public Key backwardMove = Key.S;
    public Key leftMove = Key.A;
    public Key rightMove = Key.D;

    [Header("Dash")]
    public Key dashKey = Key.Space;
    public float dashMultiplier = 2.75f;
    public float dashDuration = 0.20f;
    public float dashCooldown = 1.5f;
    public Color dashTrailColor = Color.cyan;

    private KeyControl forwardKey, backwardKey, leftKey, rightKey, dashKeyCtrl;
    private bool isDashing = false;
    private float baseSpeed;
    private float dashEndTime = 0f;
    private float nextDashReadyTime = 0f;
    private TrailRenderer dashTrail;

    private TakeDamage takeDamageScript;
    public UnityEvent onDash; //<-- unity event for audio


    void Start()
    {
        baseSpeed = speed;

        var kb = Keyboard.current;
        forwardKey = kb[forwardMove];
        backwardKey = kb[backwardMove];
        leftKey = kb[leftMove];
        rightKey = kb[rightMove];
        dashKeyCtrl = kb[dashKey];


        // --- Create the Trail Renderer at runtime ---
        dashTrail = gameObject.AddComponent<TrailRenderer>();
        dashTrail.time = 0.2f;                        // how long trail stays visible
        dashTrail.startWidth = 0.4f;
        dashTrail.endWidth = 0f;
        dashTrail.material = new Material(Shader.Find("Sprites/Default"));
        dashTrail.startColor = dashTrail.endColor = dashTrailColor;
        dashTrail.emitting = false;                   // only active during dash


        takeDamageScript = GetComponent<TakeDamage>();
    }

    void Update()
    {
        if (PauseGame.isPaused) return;

        if (!isDashing && Time.time >= nextDashReadyTime && dashKeyCtrl.wasPressedThisFrame)
        {
            HandleDashA();
            onDash.Invoke();
        }

        if (isDashing && Time.time >= dashEndTime)
        {
            HandleDashB();
        }

        HandleMovement();
    }

    void HandleDashA()
    {


        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        speed *= dashMultiplier;
        dashTrail.emitting = true; // turn on trail

        takeDamageScript.ActivateTemporaryInvincibility(dashDuration);
    }





    void HandleDashB()
    {

        isDashing = false;
        speed /= dashMultiplier;
        dashTrail.emitting = false; // turn off trail
        nextDashReadyTime = Time.time + dashCooldown;

    }

    void HandleMovement()
    {
        Vector3 move = Vector3.zero;
        if (forwardKey.isPressed) move += Vector3.forward;
        if (backwardKey.isPressed) move += Vector3.back;
        if (leftKey.isPressed) move += Vector3.left;
        if (rightKey.isPressed) move += Vector3.right;

        if (move.sqrMagnitude > 0f)
        {
            move.Normalize();
            transform.Translate(move * speed * Time.deltaTime, Space.World);
        }

        //delta = playerWidth / 2. playerWidth = 0.2 (currently)
        float delta = 0.1f; //Space considered because of player size
        float minX = -6f;
        float maxX = 6f;
        float minZ = -11f;
        float maxZ = -7f;
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX + delta, maxX - delta);
        pos.z = Mathf.Clamp(pos.z, minZ + delta, maxZ - delta);
        transform.position = pos;
    }
}