using System.Collections;
using UnityEngine;

public class Shotgun : MonoBehaviour, IEnemy
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;      
    private float shootCooldown = 2;     // Seconds between shots
    private int bulletCount;              // Number of bullets per spray
    private float spreadAngle = 65;      // Cone angle of shotgun
    private bool canShoot = true;

    public int HP { get; set; } = 200; //change HP later if needed
    public EnemyRole role { get; set; } = EnemyRole.Vanguard;

    public bool isBoss { get; set; } = false;//

    void Update()
    {
        if (canShoot)
        {
            int behavior = Random.Range(1, 4); //either 1, 2, or 3
            if (behavior == 1)
                StartCoroutine(Behavior1());
            else if (behavior == 2)
                StartCoroutine(Behavior2());
            else if (behavior == 3)
                StartCoroutine(Behavior3());
        }
    }

    public IEnumerator Behavior1() //Basic shotgun shot
    {
        canShoot = false;
        bulletCount = Random.Range(12, 15);//12, 13, or 14 bullets
        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = -spreadAngle / 2f;
        shootCooldown = 2f;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;

            // Rotate around Y axis for horizontal spread
            Quaternion rotation = transform.rotation * Quaternion.Euler(0, angle, 0);

            Instantiate(bulletPrefab, transform.position, rotation);
        }

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    public IEnumerator Behavior2() //Shoots left to right instead of default shotgun
    {
        canShoot = false;
        bulletCount = Random.Range(12, 16);
        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = -spreadAngle / 2f;
        float delta_shot = Random.Range(0.05f, 0.1f); ; //How much time between each bullet
        shootCooldown = 0.8f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;

            // Rotate around Y axis for horizontal spread
            Quaternion rotation = transform.rotation * Quaternion.Euler(0, angle, 0);

            GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
            bullet.transform.localScale *= 1.2f;
            yield return new WaitForSeconds(delta_shot);
        }

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }


    public IEnumerator Behavior3()
    {
        canShoot = false;
        bulletCount = Random.Range(12, 16);
        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = -spreadAngle / 2f;
        float delta_shot = Random.Range(0.05f, 0.1f); ; //How much time between each bullet
        shootCooldown = 0.8f;

        for (int i = bulletCount; i > 0; i--)
        {
            float angle = startAngle + angleStep * i;

            // Rotate around Y axis for horizontal spread
            Quaternion rotation = transform.rotation * Quaternion.Euler(0, angle, 0);

            GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
            bullet.transform.localScale *= 1.2f;
            yield return new WaitForSeconds(delta_shot);
        }

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}