using UnityEngine;

public class SwordHitsSomething : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        IEnemyProjectile projectile = other.GetComponent<IEnemyProjectile>(); //only works against Enemy projectiles

        if (projectile != null)
        {
            projectile.OnHit(gameObject);
        }
    }
}
