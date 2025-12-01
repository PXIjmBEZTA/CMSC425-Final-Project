using UnityEngine;

public class SwordHitsSomething : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        IEnemyProjectile projectile = other.GetComponent<IEnemyProjectile>();

        if (projectile != null)
        {
            projectile.OnHit(gameObject);
        }
    }
}
