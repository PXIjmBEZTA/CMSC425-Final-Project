using UnityEngine;

public class BigPlayerBulletShoot : MonoBehaviour, IPlayerBullet 
{  //to be attatched to the bigBullet Prefab in particular
    public float speed;
    public float lifeTime;
    public float timeLeft;
    [SerializeField] private int damage;
    public int Damage => damage;
    public float acceleration = 0.00076f;
    void Start()
    {
        speed = 1.2f;
        lifeTime = 5f;
        damage = 5;
        timeLeft = lifeTime;
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
            //since this whole script is particularly for bigBullets, no need to see the condition "isBig"
            speed += acceleration*120 * Time.deltaTime*300;
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
