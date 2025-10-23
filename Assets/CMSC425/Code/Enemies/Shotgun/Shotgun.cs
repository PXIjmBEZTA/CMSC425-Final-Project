using System.Collections;
using UnityEngine;

public class Shotgun : MonoBehaviour, IEnemy
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;      
    private float shootCooldown;     // Seconds between shots
    private int bulletCount;              // Number of bullets per spray
    private float spreadAngle;      // Cone angle of shotgun
    private bool canShoot = true;

    public int HP { get; set; } = 200; //change HP later if needed
    private void Start()
    {
        shootCooldown = 2f;
        spreadAngle = 65f;
    }
    void Update()
    {
        //Later, each enemy will have 3 behaviors
        if (canShoot)
        {
            StartCoroutine(Behavior1());
        }
    }

    public IEnumerator Behavior1() //Basic shotgun shot
    {
        canShoot = false;
        bulletCount = Random.Range(12, 16);
        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = -spreadAngle / 2f;

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
}