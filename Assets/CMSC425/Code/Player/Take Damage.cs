using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class TakeDamage : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    public float respawnDelay = 1.5f; //seconds
    public float invincibilityDuration = 2.0f;
    private bool isRespawning = false;
    private bool isInvincible = false;
    public int maxLives = 4;
    private int lives;
    private MovePlayer player;
    public float speedReductionWhenInvincible = 0.5f;

    [Header ("Invincibility Flashing")]
    public Material defaultMaterial;
    public Material invincibleMaterial;
    private float timeBetweenFlashes = 0.1f;

    public GameObject shootGuyPrefab;

    [Header("UI")]
    public PlayerHeartUI heart1; //leftmost heart
    public PlayerHeartUI heart2;
    public PlayerHeartUI heart3;
    public PlayerHeartUI heart4; //rightmost heart

    private void Start()
    {
        player = GetComponent<MovePlayer>();
        lives = maxLives;
        startPosition = transform.position;
        startRotation = transform.rotation;
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

    IEnumerator Respawn() //This is not used anymore. 
    {
        isRespawning = true;
        GetComponent<Collider>().enabled = false;        // disable collisions
        GetComponent<MeshRenderer>().enabled = false;    // hide player visuals
        GetComponent<MovePlayer>().enabled = false;
        GetComponent<PlayerShoot>().enabled = false;

        yield return new WaitForSeconds(respawnDelay); //this is a delay before continuing code

        Vector3 shootGuyStartPos = startPosition;
        shootGuyStartPos.z = -11 + 0.1f; //the +0.1f is accounting for the player's size

        Instantiate(shootGuyPrefab, shootGuyStartPos, startRotation);
        Destroy(gameObject);
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
        Debug.Log("Oh no! I am dead! Now spawning ShootGuy"); //Later, we swap to Gunner instead of flyer
        yield return StartCoroutine(Respawn());
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
        if (lives == 3)
            heart4.DestroyHeart();
        if (lives == 2)
            heart3.DestroyHeart();
        if (lives == 1)
            heart2.DestroyHeart();
        if (lives == 0)
            heart1.DestroyHeart();
        yield return null;
    }
}
