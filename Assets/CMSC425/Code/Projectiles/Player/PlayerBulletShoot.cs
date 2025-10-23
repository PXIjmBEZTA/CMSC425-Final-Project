using UnityEngine;

public class PlayerBulletShoot : MonoBehaviour, IPlayerBullet
{
    public float speed;
    public float lifeTime;
    public float timeLeft;
    [SerializeField] private int damage;
    public int Damage => damage;

     public float acceleration = 0.00076f;

    private PlayerShoot shot;
    void Start()
    {
        speed = 3.0f;
        lifeTime = 5f;
        damage = 2;
        timeLeft = lifeTime;
        shot = FindFirstObjectByType<PlayerShoot>();
    }

    
    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            //no it serves no functional purpose other than looking cool
            //(and visual feedback of a fast and powerful attack was shot just now)
            if (shot.isBig) 
                speed += acceleration*120 * Time.deltaTime*300;
            //if false (regular small bullet) then it will move linearly without the accelerated speed

            transform.Translate(Vector3.forward * speed * Time.deltaTime);


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IEnemy enemy = other.GetComponent<IEnemy>();
        var takeDamage = other.GetComponent<EnemyTakeDamage>();
        if (enemy != null)
        {
            Destroy(gameObject);
        }


    }
}
