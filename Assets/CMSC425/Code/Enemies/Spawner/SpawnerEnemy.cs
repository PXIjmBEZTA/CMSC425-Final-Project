using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour, IEnemy
{
    [Header("Spawn Settings")]
    public GameObject shotgunPrefab;
    public GameObject shieldPrefab;
    public GameObject bomberPrefab;
    public float spawnCooldown = 12f;
    public float initialDelay = 5f;

    public int HP { get; set; } = 150;
    public EnemyRole role { get; set; } = EnemyRole.Support;

    private bool canAct = false;
    public bool isBoss { get; set; } = false;//

    void Start()
    {
        StartCoroutine(InitialStall());
    }

    private IEnumerator InitialStall()
    {
        canAct = false;
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

    // Behavior1: Spawn Shotgun
    public IEnumerator Behavior1()
    {
        canAct = false;
        IEnemy shotGun = shotgunPrefab.GetComponent<IEnemy>();
        StartCoroutine(GameManager.Instance.SpawnEnemy(shotGun));
        yield return new WaitForSeconds(spawnCooldown);
        canAct = true;
    }

    // Behavior2: Spawn Shield
    public IEnumerator Behavior2()
    {
        canAct = false;
        IEnemy shield = shieldPrefab.GetComponent<IEnemy>();
        StartCoroutine(GameManager.Instance.SpawnEnemy(shield));
        yield return new WaitForSeconds(spawnCooldown);
        canAct = true;
    }

    // Behavior3: Spawn Bomber
    public IEnumerator Behavior3()
    {
        canAct = false;
        IEnemy bomber = bomberPrefab.GetComponent<IEnemy>();
        StartCoroutine(GameManager.Instance.SpawnEnemy(bomber));
        yield return new WaitForSeconds(spawnCooldown);
        canAct = true;
    }

    private void SpawnEnemy(GameObject prefab)
    {
        if (prefab == null || GameManager.Instance == null)
            return;

        IEnemy enemy = prefab.GetComponent<IEnemy>();
        if (enemy != null)
        {
            GameManager.Instance.SpawnEnemyFromSpawner(enemy);
            Debug.Log($"[SPAWNER] Requested spawn of {prefab.name}");
        }
    }

}