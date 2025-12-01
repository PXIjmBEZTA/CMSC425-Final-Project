using System.Collections;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    public GameObject bulletPrefab;
    void Start()
    {
        StartCoroutine(BlowUp());
    }


    private IEnumerator BlowUp()
    {
        float bombTimer = 3f;
        int bulletCount = Random.Range(30, 34);
        float spreadAngle = 360; //All around the bomb
        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = 0;

        yield return new WaitForSeconds(bombTimer);

        // Play ONE explosion sound here
        AudioManager.Instance.Play(AudioManager.SoundType.BombExplode);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Quaternion rotation = transform.rotation * Quaternion.Euler(0, angle, 0);

            GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position, rotation);

            //Telling the bullet not to play its own sound (bullet sfx would stack otherwise, sounding loud and sharp)
            EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
            if (bulletScript != null)
                bulletScript.SuppressSound = true;
        }
        Destroy(gameObject);
    }
}
