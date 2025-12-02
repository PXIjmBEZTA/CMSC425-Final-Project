using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject[] enemyPrefabs;
    public int numEnemiesPerRow = 3;

    void Start()
    {
        IEnemy[] enemies = new IEnemy[enemyPrefabs.Length];
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemies[i] = enemyPrefabs[i].GetComponent<IEnemy>();
        }
        GameManager.Instance.InitiateCombat(enemies, numEnemiesPerRow);
    }

}
