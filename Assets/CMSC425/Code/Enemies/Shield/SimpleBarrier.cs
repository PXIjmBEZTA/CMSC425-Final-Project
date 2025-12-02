using UnityEngine;

public class SimpleBarrier : MonoBehaviour
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
            health--;
            Destroy(other.gameObject);

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}