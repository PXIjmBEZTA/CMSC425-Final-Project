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
    public MovePlayer flyGuy;



    [Header("Invincibility Flashing")]
    public Material defaultMaterial;
    public Material invincibleMaterial;
    private float timeBetweenFlashes = 0.1f;


    [Header("UI")]
    public PlayerHeartUI heart1; //leftmost heart
    public PlayerHeartUI heart2;
    public PlayerHeartUI heart3; //rightmost heart

    public UIShaker uiShaker;

    private Renderer[] modelRenderers;


    private void Start()
    {
        player = GetComponent<MoveShootGuy>();
        lives = maxLives;
        startPosition = transform.position;
        startRotation = transform.rotation;

        StartCoroutine(TemporaryInvincibility(invincibilityDuration));


        //Get a reference to the old hearts from the flyGuy



        heart1.SetFull();
        heart2.SetFull();
        heart3.SetFull();

        modelRenderers = GetComponentsInChildren<Renderer>();
        if (modelRenderers == null)
            Debug.Log("No renderers found!");
    }

    //This is necessary for the GameManager to manually set the UI elements.
    public void Init(PlayerHeartUI h1, PlayerHeartUI h2, PlayerHeartUI h3, UIShaker shaker)
    {
        heart1 = h1;
        heart2 = h2;
        heart3 = h3;
        uiShaker = shaker;
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

        SetRenderersMaterial(defaultMaterial);
    }

    IEnumerator InvincibilityFlashing()
    {
        bool toggle = false;
        while (isInvincible)
        {
            SetRenderersMaterial(toggle ? invincibleMaterial : defaultMaterial);
            toggle = !toggle;
            yield return new WaitForSeconds(timeBetweenFlashes);
        }
        SetRenderersMaterial(defaultMaterial);

    }
    IEnumerator Die()
    {
        Destroy(gameObject);
        GameManager.Instance.OnPlayerFinalDeath();
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


    private void SetRenderersEnabled(bool enabled)
    {
        if (modelRenderers == null) return;
        foreach (var r in modelRenderers) r.enabled = enabled;
    }

    // Helper to set material(s) to the same material (simple flashing)
    private void SetRenderersMaterial(Material mat)
    {
        if (modelRenderers == null) return;
        foreach (var r in modelRenderers)
        {
            // This creates per-instance materials — that's fine for flashing.
            // If you want to avoid instancing, use sharedMaterial (but it affects all instances).
            r.material = mat;
        }
    }
}