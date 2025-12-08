using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MoveShootGuy : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 4;
    public Key forwardMove = Key.W;
    public Key backwardMove = Key.S;
    public Key leftMove = Key.A;
    public Key rightMove = Key.D;

    [Header("Jump")]
    public Key jumpKey = Key.Space;
    public float jumpHeight = 2;
    public float jumpDuration = 2.5f;

    private KeyControl forwardKey, backwardKey, leftKey, rightKey, jumpKeyCtrl;
    private bool isJumping = false;
    private float jumpTimer = 0;
    private float startZ;

    public Animator animator;

    void Start()
    {
        Keyboard kb = Keyboard.current;
        forwardKey = kb[forwardMove];
        backwardKey = kb[backwardMove];
        leftKey = kb[leftMove];
        rightKey = kb[rightMove];
        jumpKeyCtrl = kb[jumpKey];
        animator = GetComponentInChildren<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (PauseGame.isPaused) return;

        HandleMovement();
        HandleJump();
    }


    void HandleMovement()
    {
        Vector3 move = Vector3.zero;
        if (leftKey.isPressed)
        {
            move += Vector3.left;
            animator.SetBool("isMoving", true);
            animator.SetFloat("Direction", 1);
        }
        else if (rightKey.isPressed)
        {
            move += Vector3.right;
            animator.SetBool("isMoving", true);
            animator.SetFloat("Direction", -1);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (move.sqrMagnitude > 0f)
        {
            move.Normalize();
            transform.Translate(move * speed * Time.deltaTime, Space.World);
        }

        //Force player within the square of movement:
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

    void HandleJump()
    {
        if (jumpKeyCtrl.wasPressedThisFrame && !isJumping)
        {
            isJumping = true;
            jumpTimer = 0;
            startZ = transform.position.z;
        }

        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            float t = jumpTimer / jumpDuration;

            float newZ = startZ + 4 * jumpHeight * t * (1 - t);
            Vector3 pos = transform.position;
            pos.z = newZ;
            transform.position = pos;

            if (t >= 1)
            {
                //End of jump
                isJumping = false;
                pos.z = startZ;
                transform.position = pos;

            }
        }
    }
}