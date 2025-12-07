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

    public UIShaker uiShaker;

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
        //future notes for myself: ^this block was accidentally duplicated, causing 2 lives to be taken instead of one (deleted now)
    }

    //This function kills the player, and respawns them as the shootGuy
    //Instantiating the shootGuy is difficult because he is a prefab, so that explains the complex code here
    IEnumerator Respawn()
    {
        isRespawning = true;
        GetComponent<Collider>().enabled = false;        // disable collisions
        GetComponent<MeshRenderer>().enabled = false;    // hide player visuals
        GetComponent<MovePlayer>().enabled = false;
        GetComponent<PlayerShoot>().enabled = false;
        heart4.DestroyHeart(); //because ShootGuy has 3 lives
        GameManager.Instance.showShootGuyControls(); //This function will automatically check if playing tutorial
        yield return new WaitForSeconds(respawnDelay); //this is a delay before continuing code

        Vector3 shootGuyStartPos = startPosition;
        //shootGuyStartPos.z = -11 + 0.1f; //the +0.1f is accounting for the player's size

        GameObject go = Instantiate(shootGuyPrefab, shootGuyStartPos, startRotation);

        ShootGuyTakeDamage takeDamageScript = go.GetComponent<ShootGuyTakeDamage>();
        takeDamageScript.Init(heart1, heart2, heart3, uiShaker);

        MoveShootGuy shootGuyMovement = go.GetComponent<MoveShootGuy>();
        shootGuyMovement.Init(shootGuyAnimation);


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


    //This exists because other classes may want to make the player invincible
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
        // Debug.Log($"UpdateHearts called. Lives: {lives}");

        AudioManager.Instance.Play(AudioManager.SoundType.Damage);
        
        if (lives == 3 && heart4 != null)
        {
            // Debug.Log("Setting heart4 empty");
            heart4.SetEmpty();
        }
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
