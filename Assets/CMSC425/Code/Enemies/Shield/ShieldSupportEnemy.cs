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
    public EnemyRole role { get; set; } = EnemyRole.Support;

    private List<GameObject> shieldedEnemies = new List<GameObject>();

    void Start()
    {
        int behavior = Random.Range(1, 4);
        if (behavior == 1)
            StartCoroutine(Behavior1());
        else if (behavior == 2)
            StartCoroutine(Behavior2());
        else if (behavior == 3)
            StartCoroutine(Behavior3());
    }

    // Behavior1: Give shields to other enemies (loops forever)
    public IEnumerator Behavior1()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                if (enemy == gameObject) continue;
                if (enemy.GetComponent<BarrierShield>() != null) continue;

                BarrierShield barrier = enemy.AddComponent<BarrierShield>();
                barrier.health = 3;

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
                Debug.Log($"Gave barrier to {enemy.name}");
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
                Debug.Log($"[SPAWN] Absorbing barrier at {spawnPos}");
            }

            yield return new WaitForSeconds(reflectiveBarrierCooldown);
        }
    }

    void OnDestroy()
    {
        // Remove shields from enemies
        foreach (GameObject enemy in shieldedEnemies)
        {
            if (enemy != null)
            {
                BarrierShield barrier = enemy.GetComponent<BarrierShield>();
                if (barrier != null)
                {
                    barrier.DestroyShield();
                }
            }
        }

        // Remove all battlefield barriers
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("ShieldBarrier");
        foreach (GameObject b in barriers)
        {
            Destroy(b);
        }
    }
}