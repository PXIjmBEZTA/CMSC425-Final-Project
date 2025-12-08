using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSupportEnemy : MonoBehaviour, IEnemy
{
    [Header("Barrier Settings")]
    public GameObject barrierPrefab;
    public float barrierCooldown = 10f;

    [Header("Reflective Barrier")]
    public GameObject reflectiveBarrierPrefab;
    public float reflectiveBarrierCooldown = 15f;

    public int HP { get; set; } = 100;
    public bool isBoss { get; set; } = false;//
    public EnemyRole role { get; set; } = EnemyRole.Support;

    private List<GameObject> shieldedEnemies = new List<GameObject>();
    private List<GameObject> myBarriers = new List<GameObject>();

    void Start()
    {
        StartCoroutine(Behavior1());
        StartCoroutine(Behavior2());
        StartCoroutine(Behavior3());
    }

    // Behavior1: Give shields to other enemies (loops forever)
    public IEnumerator Behavior1()
{
    yield return new WaitForSeconds(3f);

    while (true)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log($"[SHIELD] Found {enemies.Length} enemies with Enemy tag");

        foreach (GameObject enemy in enemies)
        {
            Debug.Log($"[SHIELD] Checking {enemy.name}");
            
            if (enemy == gameObject)
            {
                Debug.Log($"[SHIELD] Skipping self");
                continue;
            }
            if (enemy.GetComponent<BarrierShield>() != null)
            {
                Debug.Log($"[SHIELD] {enemy.name} already has barrier");
                continue;
            }

            BarrierShield barrier = enemy.AddComponent<BarrierShield>();
            barrier.health = 3;
            barrier.owner = this;

            if (barrierPrefab != null)
            {
                GameObject visual = Instantiate(barrierPrefab);
                visual.transform.SetParent(enemy.transform);
                visual.transform.localPosition = Vector3.forward * 0.8f;
                visual.transform.localRotation = Quaternion.identity;
                visual.transform.localScale = Vector3.one * 0.3f;
                barrier.SetVisual(visual);
            }

            shieldedEnemies.Add(enemy);
            Debug.Log($"[SHIELD] Gave barrier to {enemy.name}");
        }

        yield return new WaitForSeconds(barrierCooldown);
    }
}

    // Behavior2: Spawn reflective (purple) barriers (loops forever)
    public IEnumerator Behavior2()
    {
        yield return new WaitForSeconds(5f);

        while (true)
        {
            if (reflectiveBarrierPrefab != null)
            {
                float minX = -5.5f;
                float maxX = 5.5f;
                float minZ = -10.5f;
                float maxZ = -7.5f;

                float randomX = Random.Range(minX + 1f, maxX - 1f);
                float randomZ = Random.Range(minZ + 1f, maxZ - 1f);
                float y = 0.4f;

                Vector3 spawnPos = new Vector3(randomX, y, randomZ);

                GameObject barrier = Instantiate(reflectiveBarrierPrefab, spawnPos, Quaternion.identity);
                barrier.tag = "ShieldBarrier";
                barrier.transform.localScale = new Vector3(1.5f, 0.8f, 0.15f);

                BoxCollider box = barrier.GetComponent<BoxCollider>();
                if (box != null)
                {
                    box.size = new Vector3(1f, 1f, 3f);
                }

                Collider col = barrier.GetComponent<Collider>();
                if (col != null)
                {
                    col.isTrigger = true;
                }

                Rigidbody rb = barrier.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;

                barrier.AddComponent<ReflectiveBarrier>();
                barrier.AddComponent<BarrierPulse>().SetColors(new Color(0.6f, 0f, 1f), new Color(1f, 0f, 1f));
                
                myBarriers.Add(barrier);
                Debug.Log($"[SPAWN] Reflective barrier at {spawnPos}");
            }

            yield return new WaitForSeconds(reflectiveBarrierCooldown);
        }
    }

    // Behavior3: Spawn absorbing (orange) barriers (loops forever)
    public IEnumerator Behavior3()
    {
        yield return new WaitForSeconds(7f);

        while (true)
        {
            if (reflectiveBarrierPrefab != null)
            {
                float minX = -5.5f;
                float maxX = 5.5f;
                float minZ = -10.5f;
                float maxZ = -7.5f;

                float randomX = Random.Range(minX + 1f, maxX - 1f);
                float randomZ = Random.Range(minZ + 1f, maxZ - 1f);
                float y = 0.4f;

                Vector3 spawnPos = new Vector3(randomX, y, randomZ);

                GameObject barrier = Instantiate(reflectiveBarrierPrefab, spawnPos, Quaternion.identity);
                barrier.tag = "ShieldBarrier";
                barrier.transform.localScale = new Vector3(1.5f, 0.8f, 0.15f);

                BoxCollider box = barrier.GetComponent<BoxCollider>();
                if (box != null)
                {
                    box.size = new Vector3(1f, 1f, 3f);
                }

                Collider col = barrier.GetComponent<Collider>();
                if (col != null)
                {
                    col.isTrigger = true;
                }

                Rigidbody rb = barrier.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;

                barrier.AddComponent<SimpleBarrier>();
                barrier.AddComponent<BarrierPulse>().SetColors(new Color(1f, 0.6f, 0f), new Color(1f, 0.8f, 0.2f));
                
                myBarriers.Add(barrier);
                Debug.Log($"[SPAWN] Absorbing barrier at {spawnPos}");
            }

            yield return new WaitForSeconds(reflectiveBarrierCooldown);
        }
    }

    void OnDestroy()
    {
        // Only remove shields that THIS ShieldEnemy created
        foreach (GameObject enemy in shieldedEnemies)
        {
            if (enemy != null)
            {
                BarrierShield barrier = enemy.GetComponent<BarrierShield>();
                if (barrier != null && barrier.owner == this)
                {
                    barrier.DestroyShield();
                }
            }
        }

        // Only remove barriers that THIS ShieldEnemy created
        foreach (GameObject b in myBarriers)
        {
            if (b != null)
            {
                Destroy(b);
            }
        }
    }
}