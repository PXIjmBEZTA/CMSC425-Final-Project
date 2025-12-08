using System.Collections;
using UnityEngine;

public class TutorialEnemy : MonoBehaviour, IEnemy
{
    public GameObject bulletPrefab;
    private float shootCooldown = 2;
    private bool canShoot = true;
    public int HP { get; set; } = 150;
    public EnemyRole role { get; set; } = EnemyRole.Vanguard;
    private int behavior = 1;
    public bool isBoss { get; set; } = false; //

    public Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (canShoot)
        {
            anim.Play("turret attack");
            if (behavior == 1)
                StartCoroutine(Behavior1());
            else if (behavior == 2)
                StartCoroutine(Behavior2());
            else if (behavior == 3)
                StartCoroutine(Behavior3());


            behavior++;
            if (behavior > 3) behavior = 1;
        }
    }

    public IEnumerator Behavior1() 
    {
        canShoot = false;
        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 180, 0));
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }


    public IEnumerator Behavior2()
    {
        canShoot=false;
        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 180, 0));
        yield return new WaitForSeconds(shootCooldown/2);
        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 180, 0));
        yield return new WaitForSeconds(shootCooldown * 1.5f);
        canShoot = true;
    }

    public IEnumerator Behavior3()
    {
        canShoot = false;
        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 180, 0));
        yield return new WaitForSeconds(shootCooldown / 2);
        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 180, 0));
        yield return new WaitForSeconds(shootCooldown / 2);
        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 180, 0));
        yield return new WaitForSeconds(shootCooldown * 2f);
        canShoot = true;
    }
}
