using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerDash: MonoBehaviour
{
    [Header("Dash Settings")]
    public Key dashKey = Key.Space;
    public float dashMultiplier = 2.75f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.6f;
    public Color dashTrailColor = Color.cyan;

    private float baseSpeed;
    private float currentCooldown = 0f;
    private float currentDuration = 0f;

    private bool isDashing = false;
    private TrailRenderer dashTrail;
    private MovePlayer player;

    public void Initialize(MovePlayer player)
    {
        player = GetComponent<MovePlayer>();
        baseSpeed = player.speed;

        // Create Trail Renderer dynamically
        dashTrail = player.gameObject.AddComponent<TrailRenderer>();
        dashTrail.time = 0.2f;
        dashTrail.startWidth = 0.4f;
        dashTrail.endWidth = 0f;
        dashTrail.material = new Material(Shader.Find("Sprites/Default"));
        dashTrail.startColor = dashTrail.endColor = dashTrailColor;
        dashTrail.emitting = false;
    }

    public void Update()
    {
        float dt = Time.deltaTime;

        // --- Handle cooldown ---
        if (currentCooldown > 0f)
            currentCooldown -= dt;

        // --- Input check ---
        if (!isDashing && currentCooldown <= 0f)
        {
            if (Keyboard.current != null && Keyboard.current[dashKey].wasPressedThisFrame)
                StartDash();
        }

        // --- Handle active dash ---
        if (isDashing)
        {
            currentDuration -= dt;
            if (currentDuration <= 0f)
                EndDash();
        }
    }

    private void StartDash()
    {
        if (player == null)
        {
            Debug.LogError("PlayerDash.StartDash: 'player' not set! Did you call Initialize()?");
            return;
        }

        isDashing = true;
        currentDuration = dashDuration;
        currentCooldown = dashCooldown;

        player.speed = baseSpeed * dashMultiplier;
        dashTrail.emitting = true;
    }

    private void EndDash()
    {
        if (player == null) return;

        isDashing = false;
        player.speed = baseSpeed;
        dashTrail.emitting = false;
    }
}
