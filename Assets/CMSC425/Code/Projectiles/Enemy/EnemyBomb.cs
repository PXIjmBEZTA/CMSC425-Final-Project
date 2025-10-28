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
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Quaternion rotation = transform.rotation * Quaternion.Euler(0, angle, 0);

            GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position, rotation);
        }
        Destroy(gameObject);
    }
}
