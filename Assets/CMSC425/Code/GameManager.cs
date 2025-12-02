using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


//This script is responsible for initiating battles and win/loss conditions.

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

   


    private bool gameOver = false;

    private List<IEnemy> enemies = new List<IEnemy>(0); //list of all enemies currently in combat.

    private Vector3 vanguardCenterSpawnPoint = new Vector3(0, 0.125f, -4);
    private Vector3 vanguardLeftSpawnPoint = new Vector3(-3.5f, 0.125f, -4);
    private Vector3 vanguardRightSpawnPoint = new Vector3(3.5f, 0.125f, -4);
    private Vector3 supportCenterSpawnPoint = new Vector3(0, 0.125f, -6);
    private Vector3 supportLeftSpawnPoint = new Vector3(-3.5f, 0.125f, -6);
    private Vector3 supportRightSpawnPoint = new Vector3(3.5f, 0.125f, -6);
    private Quaternion startRotation = Quaternion.Euler(0, 180, 0);
    private IEnemy[] vanguardEnemies = new IEnemy[3];
    private IEnemy[] supportEnemies = new IEnemy[3];

    public bool vanguardIsFull = false;
    public bool supportIsFull = false;

    private int numVanguardEnemies = 0;
    private int numSupportEnemies = 0;
    private int maxEnemiesPerRow = 0; //will be either 1, 2, or 3.
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    

    //Called once when the combat begins
    public void InitiateCombat(IEnemy[] initialEnemies, int enemiesPerRow)
    {
        
        maxEnemiesPerRow = enemiesPerRow;
        enemies.Clear();
        numVanguardEnemies = 0;
        numSupportEnemies = 0;
        for (int i = 0; i < initialEnemies.Length; i++)
        {
            StartCoroutine(SpawnEnemy(initialEnemies[i]));
        }
    }

    //This will spawn the enemy at full health, and will give them correct starting locations
    private IEnumerator SpawnEnemy(IEnemy enemy)
    {
        yield return new WaitForSeconds(0.25f);

        int index = (enemy.role == EnemyRole.Vanguard) ? numVanguardEnemies : numSupportEnemies;
        (Vector3 spawnPoint, int slotIndex) = GetBalancedSpawnPoint(enemy.role, index);


        GameObject instance = GameObject.Instantiate(((MonoBehaviour)enemy).gameObject, spawnPoint, startRotation);
        IEnemy enemyInstance = instance.GetComponent<IEnemy>();
        enemies.Add(enemyInstance);

        // Update the row array
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


    }


    //When an enemy dies, the enemy list is updated
    public void OnEnemyDeath(IEnemy enemy)
    {
        enemies.Remove(enemy);
        if (enemy.role == EnemyRole.Vanguard)
        {
            RemoveEnemyFromRow(vanguardEnemies, enemy);
            vanguardIsFull = false;
        }
        else //suport enemy
        {
            RemoveEnemyFromRow(supportEnemies, enemy);
            supportIsFull = false;
        }



        if (enemies.Count == 0 && !gameOver)
        {
            Debug.Log("Yippee! You win!");
            //Win();
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

        switch(maxEnemiesPerRow)
        {
            case 1:
                order = new int[] { 1 }; //spawn only center!
                break;
            case 2:
                order = new int[] { 0, 2 }; //spawn left and right!
                break;
            case 3:
                order = new int[] { 1, 0, 2 }; //spawn center, then left, then right
                break;
            default:
                Debug.Log("There cannot be more than 3 max-enemies per row!");
                order = new int[] { 1, 0, 2 }; //spawn center, then left, then right
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
        for (int i = 0;i < row.Length;i++)
        {
            if (row[i] == null)
                return false;
        }
        return true;
    }
    //Loss condition. When Player dies, Game Over Screen!
    public void OnPlayerFinalDeath()
    {
        if (gameOver) return;

        gameOver = true;
        Debug.Log("GAME OVER! Player has no lives remaining.");
            //Lose();
        // You can add UI logic here later:
        // ShowGameOverPanel();
        // Stop enemy spawns
        // Freeze game
    }
}
