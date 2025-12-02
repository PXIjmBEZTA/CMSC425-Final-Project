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
    public float reflectiveBarrierCooldown = 9f;

    public int HP { get; set; } = 100;
    public EnemyRole role { get; set; } = EnemyRole.Support;

    private List<GameObject> shieldedEnemies = new List<GameObject>();

    void Start()
    {
        StartCoroutine(GiveBarriers());
        StartCoroutine(SpawnReflectiveBarriers());
    }

    IEnumerator GiveBarriers()
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

    IEnumerator SpawnReflectiveBarriers()
    {
        yield return new WaitForSeconds(5f);

        while (true)
        {
            if (reflectiveBarrierPrefab == null)
            {
                yield return new WaitForSeconds(reflectiveBarrierCooldown);
                continue;
            }

            // Player movement area
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

            if (Random.value < 0.5f)
            {
                barrier.AddComponent<SimpleBarrier>();
                barrier.AddComponent<BarrierPulse>().SetColors(new Color(1f, 0.6f, 0f), new Color(1f, 0.8f, 0.2f));
            }
            else
            {
                barrier.AddComponent<ReflectiveBarrier>();
                barrier.AddComponent<BarrierPulse>().SetColors(new Color(0.6f, 0f, 1f), new Color(1f, 0f, 1f));
            }

            yield return new WaitForSeconds(reflectiveBarrierCooldown);
        }
    }

    public IEnumerator Behavior1()
    {
        yield return null;
    }

    public IEnumerator Behavior2()
    {
        yield return null;
    }

    public IEnumerator Behavior3()
    {
        yield return null;
    }

    void OnDestroy()
    {
        foreach (GameObject enemy in shieldedEnemies)
        {
            if (enemy != null)
            {
                BarrierShield barrier = enemy.GetComponent<BarrierShield>();
                if (barrier != null)
                {
                    Destroy(barrier);
                }
            }
        }

        GameObject[] barriers = GameObject.FindGameObjectsWithTag("ShieldBarrier");
        foreach (GameObject b in barriers)
        {
            Destroy(b);
        }
    }
}