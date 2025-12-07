using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool gameOver = false;

    private List<IEnemy> enemies = new List<IEnemy>(0);

    private Vector3 vanguardCenterSpawnPoint = new Vector3(0, 0.125f, -6); 
    private Vector3 vanguardLeftSpawnPoint = new Vector3(-3.5f, 0.125f, -6);
    private Vector3 vanguardRightSpawnPoint = new Vector3(3.5f, 0.125f, -6);
    private Vector3 supportCenterSpawnPoint = new Vector3(0, 0.125f, -4);
    private Vector3 supportLeftSpawnPoint = new Vector3(-3.5f, 0.125f, -4);
    private Vector3 supportRightSpawnPoint = new Vector3(3.5f, 0.125f, -4);
    private Quaternion startRotation = Quaternion.Euler(0, 180, 0);
    private IEnemy[] vanguardEnemies = new IEnemy[3];
    private IEnemy[] supportEnemies = new IEnemy[3];

    private bool vanguardIsFull = false;
    private bool supportIsFull = false;

    private int numVanguardEnemies = 0;
    private int numSupportEnemies = 0; 
    private int maxVanguardEnemies = 0;
    private int maxSupportEnemies = 0;

    private bool playingTutorial = false;
    public TMP_Text controlsText;
    public TMP_Text shootGuyControls;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        controlsText.enabled = false;
        shootGuyControls.enabled = false;
    }

    public void InitiateCombat(IEnemy[] initialEnemies, int maxVanguard, int maxSupport)
    {
        maxVanguardEnemies = maxVanguard;
        maxSupportEnemies = maxSupport;
        enemies.Clear();
        numVanguardEnemies = 0;
        numSupportEnemies = 0;
        for (int i = 0; i < initialEnemies.Length; i++)
        {
            StartCoroutine(SpawnEnemy(initialEnemies[i]));
        }

    }

    public void TurnOnTurorial()
    {
        playingTutorial = true;
        showControls();
    }
    public IEnumerator SpawnEnemy(IEnemy enemy)
    {
        yield return new WaitForSeconds(0.25f);

        // Check if row is full before spawning
        if (enemy.role == EnemyRole.Vanguard && numVanguardEnemies >= maxVanguardEnemies)
        {
            Debug.Log("[GAMEMANAGER] Vanguard row full, can't spawn");
            yield break;
        }
        if (enemy.role == EnemyRole.Support && numSupportEnemies >= maxSupportEnemies)
        {
            Debug.Log("[GAMEMANAGER] Support row full, can't spawn");
            yield break;
        }
        
        int index = (enemy.role == EnemyRole.Vanguard) ? numVanguardEnemies : numSupportEnemies;
        (Vector3 spawnPoint, int slotIndex) = GetBalancedSpawnPoint(enemy.role, index);
        IEnemy[] row = enemy.role == EnemyRole.Vanguard ? vanguardEnemies : supportEnemies;
        if (IsRowFull(row)) yield break; //do not add if the row is full.
        GameObject instance = GameObject.Instantiate(((MonoBehaviour)enemy).gameObject, spawnPoint, startRotation);
        IEnemy enemyInstance = instance.GetComponent<IEnemy>();
        enemies.Add(enemyInstance);

        if (enemy.role == EnemyRole.Vanguard)
        {
            vanguardEnemies[slotIndex] = enemyInstance; 
            numVanguardEnemies++;
            vanguardIsFull = IsRowFull(vanguardEnemies);
        }
        else
        {
            supportEnemies[slotIndex] = enemyInstance;
            numSupportEnemies++;
            supportIsFull = IsRowFull(supportEnemies);
        }
    
        Debug.Log($"[GAMEMANAGER] Spawned {instance.name} at {spawnPoint}");
    }

    public void SpawnEnemyFromSpawner(IEnemy enemy)
    {
        StartCoroutine(SpawnEnemy(enemy));
    }

    public void OnEnemyDeath(IEnemy enemy)
    {
        enemies.Remove(enemy);
        if (enemy.role == EnemyRole.Vanguard)
        {
            RemoveEnemyFromRow(vanguardEnemies, enemy);
            vanguardIsFull = false;
            numVanguardEnemies--;
        }
        else
        {
            RemoveEnemyFromRow(supportEnemies, enemy);
            supportIsFull = false;
            numSupportEnemies--;
        }

        if (enemies.Count == 0 && !gameOver)
        {
            Debug.Log("Yippee! You win!");
        }
    }

    public void RegisterSpawnedEnemy(IEnemy enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    private (Vector3, int) GetBalancedSpawnPoint(EnemyRole role, int index)
    {
        Vector3[] positions;
        if (role == EnemyRole.Vanguard)
            positions = new Vector3[] { vanguardLeftSpawnPoint, vanguardCenterSpawnPoint, vanguardRightSpawnPoint };
        else
            positions = new Vector3[] { supportLeftSpawnPoint, supportCenterSpawnPoint, supportRightSpawnPoint };

        int[] order;
        int maxEnemiesInRow = (role == EnemyRole.Vanguard) ? maxVanguardEnemies : maxSupportEnemies;
        switch(maxEnemiesInRow)
        {
            case 1:
                order = new int[] { 1 };
                break;
            case 2:
                order = new int[] { 0, 2 };
                break;
            case 3:
                order = new int[] { 1, 0, 2 };
                break;
            default:
                Debug.Log("There cannot be more than 3 max-enemies per row!");
                order = new int[] { 1, 0, 2 };
                break;
        }
        return (positions[order[index]], order[index]);
    }

    private void RemoveEnemyFromRow(IEnemy[] row, IEnemy enemy)
    {
        for (int i = 0; i < row.Length; i++)
        {
            if (row[i] == enemy)
            {
                row[i] = null;
                break; 
            }
        }
    }

    private bool IsRowFull(IEnemy[] row)
    {
        for (int i = 0; i < row.Length; i++)
        {
            if (row[i] == null)
                return false;
        }
        return true;
    }

    public void OnPlayerFinalDeath()
    {
        if (gameOver) return;

        gameOver = true;
        Debug.Log("GAME OVER! Player has no lives remaining.");
    }

    public void showControls()
    {
        controlsText.enabled = true;
    }

    public void showShootGuyControls()
    {
        if (playingTutorial)
        {
            controlsText.enabled = false;
            shootGuyControls.enabled = true;
        }
    }
    public void hideControls()
    {
        controlsText.enabled = false;
        shootGuyControls.enabled = false;
    }
}