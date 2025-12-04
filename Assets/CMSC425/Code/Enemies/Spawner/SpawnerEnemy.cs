using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour, IEnemy
{
    [Header("Spawn Settings")]
    public List<GameObject> enemyPrefabs;
    public float spawnCooldown = 12f;
    public int maxSpawnedEnemies = 2;
    public float spawnedScale = 0.7f;
    public float minDistanceBetweenEnemies = 2.5f;
    public float initialDelay = 5f;

    public int HP { get; set; } = 150;
    public EnemyRole role { get; set; } = EnemyRole.Support;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool canAct = false;
    private GameObject lastSpawnedPrefab = null;

    void Start()
    {
        StartCoroutine(InitialStall());
    }

    private IEnumerator InitialStall()
    {
        yield return new WaitForSeconds(initialDelay);
        canAct = true;
    }

    void Update()
    {
        if (canAct)
        {
            int behavior = Random.Range(1, 4);
            if (behavior == 1)
                StartCoroutine(Behavior1());
            else if (behavior == 2)
                StartCoroutine(Behavior2());
            else if (behavior == 3)
                StartCoroutine(Behavior3());
        }
    }

    public IEnumerator Behavior1()
    {
        canAct = false;
        SpawnEnemy();
        yield return new WaitForSeconds(spawnCooldown);
        canAct = true;
    }

    public IEnumerator Behavior2()
    {
        canAct = false;
        SpawnEnemy();
        yield return new WaitForSeconds(1f);
        SpawnEnemy();
        yield return new WaitForSeconds(spawnCooldown);
        canAct = true;
    }

    public IEnumerator Behavior3()
    {
        canAct = false;
        yield return new WaitForSeconds(2f);
        SpawnEnemy();
        yield return new WaitForSeconds(spawnCooldown);
        canAct = true;
    }

    private void SpawnEnemy()
{
    spawnedEnemies.RemoveAll(e => e == null);

    if (spawnedEnemies.Count >= maxSpawnedEnemies || enemyPrefabs.Count == 0)
        return;

    GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

    Vector3 spawnPos = Vector3.zero;
    bool validPosition = false;

    for (int attempt = 0; attempt < 10; attempt++)
    {
        spawnPos = transform.position + new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-1.5f, 1.5f));

        validPosition = true;
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null && Vector3.Distance(spawnPos, enemy.transform.position) < minDistanceBetweenEnemies)
            {
                validPosition = false;
                break;
            }
        }

        if (validPosition) break;
    }

    if (validPosition)
    {
        GameObject enemyObj = Instantiate(prefab, spawnPos, prefab.transform.rotation);
        enemyObj.transform.localScale = prefab.transform.localScale * spawnedScale;
        spawnedEnemies.Add(enemyObj);

        IEnemy enemyComponent = enemyObj.GetComponent<IEnemy>();
        if (enemyComponent != null && GameManager.Instance != null)
        {
            GameManager.Instance.RegisterSpawnedEnemy(enemyComponent);
        }

        Debug.Log($"[SPAWNER] Spawned {prefab.name} at {spawnPos}");
    }
}
}