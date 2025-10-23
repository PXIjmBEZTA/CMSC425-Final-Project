using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    IEnemy enemy;
    void Start()
    {
        enemy = GetComponent<IEnemy>();
        if (enemy == null)
            Debug.LogWarning("EnemyTakeDamage requires an IEnemy component on this game object!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemy == null) return;

        IPlayerBullet bullet = other.GetComponent<IPlayerBullet>();
        
        if (bullet != null)
        {
            int damage = bullet.Damage;
            ApplyDamage(damage);
            Destroy(other.gameObject);
        }
    }

    private void ApplyDamage(int damage)
    {
        enemy.HP -= damage;
        //Update HP Bar (when implemented)
        Debug.Log($"{gameObject.name} took {damage} damage! Remaining HP: {enemy.HP}");
        if (enemy.HP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        //potentially we will want it to explode 
    }
}
