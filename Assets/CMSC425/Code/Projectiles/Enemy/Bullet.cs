using UnityEngine;

public class EnemyBullet : MonoBehaviour, IEnemyProjectile
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float lifeTime = 5f;
    private float timeLeft;

    public int Damage => damage;
    public float Speed => speed;
    public float LifeTime => lifeTime;
    public float TimeLeft => timeLeft;

    public void Start()
    {
        timeLeft = lifeTime;
    }
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            Destroy(gameObject);        
        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        OnHit(collision.gameObject);
    }

    public void OnHit(GameObject target)
    {

        Destroy(gameObject);
    }
}
