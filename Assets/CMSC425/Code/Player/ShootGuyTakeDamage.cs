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
    private MovePlayer player;
    public float speedReductionWhenInvincible = 0.5f;

    [Header("Invincibility Flashing")]
    public Material defaultMaterial;
    public Material invincibleMaterial;
    private float timeBetweenFlashes = 0.1f;


    [Header("UI")]
    public GameObject heartsParent;
    public GameObject heartPrefab;
    private PlayerHeartUI heart1;
    private PlayerHeartUI heart2;

    private void Start()
    {
        player = GetComponent<MovePlayer>();
        lives = maxLives;
        startPosition = transform.position;
        startRotation = transform.rotation;

        GameObject h1 = Instantiate(heartPrefab, heartsParent.transform);
        GameObject h2 = Instantiate(heartPrefab, heartsParent.transform);

        heart1 = h1.GetComponent<PlayerHeartUI>();
        heart2 = h2.GetComponent<PlayerHeartUI>();

    }
    void OnTriggerEnter(Collider other)
    {

        if (isRespawning || isInvincible)
            return;
        IEnemyProjectile projectile = other.GetComponent<IEnemyProjectile>();

        if (projectile != null) //if hit by an enemy projectile
        {
            lives -= 1; //lose a life
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
        if (lives <= 1)
            heart2.DestroyHeart();
        if (lives == 0)
            heart1.DestroyHeart();
        yield return null;
    }
}
