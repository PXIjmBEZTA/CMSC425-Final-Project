using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    
    private IEnemy enemy;
    private int maxHP;
    private float x_scale;
    private float y_scale;
    private float z_scale;

    public Transform hpBarFill;

    void Start()
    {
        enemy = GetComponent<IEnemy>();
        if (enemy == null)
            Debug.LogWarning("EnemyTakeDamage requires an IEnemy component on this game object!");

        maxHP = enemy.HP;
        x_scale = hpBarFill.transform.localScale.x;
        y_scale = hpBarFill.transform.localScale.y;
        z_scale = hpBarFill.transform.localScale.z;
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
        
        if (hpBarFill != null && maxHP > 0)
        {
            float hpPercent = Mathf.Clamp01((float)enemy.HP / maxHP);
            hpBarFill.localScale = new Vector3(hpPercent*x_scale, y_scale, z_scale);
        }
        
        
        if (enemy.HP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.OnEnemyDeath(gameObject.GetComponent<IEnemy>());
        Destroy(gameObject);
        //potentially we will want it to explode 
    }
}
