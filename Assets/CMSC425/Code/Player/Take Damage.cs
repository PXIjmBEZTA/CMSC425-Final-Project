using UnityEngine;
using System.Collections;
public class TakeDamage : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    public float respawnDelay = 1.5f; //seconds
    public float invincibilityDuration = 2.0f;
    private bool isRespawning = false;
    private bool isInvincible = false;

    private void Start()
    {
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
            projectile.OnHit(gameObject);
            StartCoroutine(Respawn());
        }
        
    }

    IEnumerator Respawn()
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

    IEnumerator InvincibilityPeriod()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
}
