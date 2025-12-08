using UnityEngine;

public class ReflectiveBarrier : MonoBehaviour
{
    public int health = 3;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IEnemyProjectile>() != null)
        {
            return;
        }

        IPlayerBullet bullet = other.GetComponent<IPlayerBullet>();
        if (bullet != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 directionToPlayer = (player.transform.position - other.transform.position).normalized;
                other.transform.rotation = Quaternion.LookRotation(directionToPlayer);

                Destroy(other.GetComponent<PlayerBulletShoot>());
                Destroy(other.GetComponent<BigPlayerBulletShoot>());

                other.gameObject.AddComponent<ReflectedBullet>();

                Renderer renderer = other.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.red;
                }
            }

            health--;
            AudioManager.Instance.Play(AudioManager.SoundType.Reflect);

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}