using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
public class ShootGuyTakeDamage : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    public float invincibilityDuration = 2.0f;
    private bool isRespawning = false;
    private bool isInvincible = false;
    public int maxLives = 3;
    private int lives;
    private MoveShootGuy player;
    public float speedReductionWhenInvincible = 0.5f;

    [Header("Invincibility Flashing")]
    public Material defaultMaterial;
    public Material invincibleMaterial;
    private float timeBetweenFlashes = 0.1f;


    [Header("UI")]
    public PlayerHeartUI heart1; //leftmost heart
    public PlayerHeartUI heart2;
    public PlayerHeartUI heart3; //rightmost heart

    public UIShaker uiShaker;

    private void Start()
    {
        player = GetComponent<MoveShootGuy>();
        lives = maxLives;
        startPosition = transform.position;
        startRotation = transform.rotation;

        StartCoroutine(TemporaryInvincibility(invincibilityDuration));
        
        
        heart1.SetFull();
        heart2.SetFull();
        heart3.SetFull();
    }
    void OnTriggerEnter(Collider other)
    {

        if (isRespawning || isInvincible)
            return;
        IEnemyProjectile projectile = other.GetComponent<IEnemyProjectile>();

        if (projectile != null) //if hit by an enemy projectile
        {
            lives -= 1; //lose a life
            if (uiShaker != null) //and added shake
            {
                uiShaker.Shake();
            }

            projectile.OnHit(gameObject);
            Debug.Log($"I took damage! I have {lives} lives left!");
            StartCoroutine(UpdateHearts());
            if (lives > 0)
                StartCoroutine(InvincibilityPeriod());
            else
                StartCoroutine(Die());
        }

    }

    IEnumerator InvincibilityPeriod() //when invincible, move slower and flash rapidly
    {
        isInvincible = true;
        player.speed *= speedReductionWhenInvincible; //reduce player move speed
        GetComponent<PlayerShoot>().enabled = false; //disable shooting

        StartCoroutine(InvincibilityFlashing());

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
        player.speed /= speedReductionWhenInvincible; //reset player move speed
        GetComponent<PlayerShoot>().enabled = true;//enable shooting

        GetComponent<MeshRenderer>().material = defaultMaterial;
    }

    IEnumerator InvincibilityFlashing()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        bool toggle = false;
        while (isInvincible)
        {
            rend.material = toggle ? invincibleMaterial : defaultMaterial;
            toggle = !toggle;
            yield return new WaitForSeconds(timeBetweenFlashes);
        }
        rend.material = defaultMaterial;

    }
    IEnumerator Die()
    {
        Destroy(gameObject);
        Debug.Log("GAME OVER!!!!!");
        yield return null;
    }

    public void ActivateTemporaryInvincibility(float duration)
    {
        if (!gameObject.activeInHierarchy) return;
        if (isInvincible) return;
        StartCoroutine(TemporaryInvincibility(duration));
    }

    private IEnumerator TemporaryInvincibility(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    private IEnumerator UpdateHearts()
    {
        AudioManager.Instance.Play(AudioManager.SoundType.Damage);

        if (lives == 2 && heart3 != null)
        {
            // Debug.Log("Setting heart3 empty");
            heart3.SetEmpty();
        }
        if (lives == 1 && heart2 != null)
        {
            // Debug.Log("Setting heart2 empty");
            heart2.SetEmpty();
        }
        if (lives == 0 && heart1 != null)
        {
            // Debug.Log("Setting heart1 empty");
            heart1.SetEmpty();
            if (uiShaker != null)
                uiShaker.SetDeadSprite();
        }
        yield return null;
    }
}
