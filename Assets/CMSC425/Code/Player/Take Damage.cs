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

    [Header ("Invincibility Flashing")]
    public Material defaultMaterial;
    public Material invincibleMaterial;
    private float timeBetweenFlashes = 0.1f;

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

        transform.position = startPosition;
        transform.rotation = startRotation;

        GetComponent<Collider>().enabled = true;        // disable collisions
        GetComponent<MeshRenderer>().enabled = true;    // hide player visuals
        GetComponent<MovePlayer>().enabled = true;
        GetComponent<PlayerShoot>().enabled = true;

        isRespawning = false;
        StartCoroutine(InvincibilityPeriod());
    }

    IEnumerator InvincibilityPeriod() //when invincible, move slower and flash rapidly
    {
        isInvincible = true;
        player.speed /= 2; //reduce player move speed
        GetComponent<PlayerShoot>().enabled = false; //disable shooting

        StartCoroutine(InvincibilityFlashing());

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
        player.speed *= 2; //reset player move speed
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
        Debug.Log("Oh no! I am dead! You lose :("); //Later, we swap to Gunner instead of flyer
        Destroy(gameObject);
        yield return new WaitForEndOfFrame();
    }
}
