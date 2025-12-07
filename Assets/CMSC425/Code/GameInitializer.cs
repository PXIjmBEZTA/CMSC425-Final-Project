using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject[] enemyPrefabs;

    [Header("Enemy Numbers")]
    public int numVanguards = 2;
    public int numSupports = 1;
    public bool playingTutorial = false;
    void Start()
    {
        IEnemy[] enemies = new IEnemy[enemyPrefabs.Length];
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemies[i] = enemyPrefabs[i].GetComponent<IEnemy>();
        }
        GameManager.Instance.InitiateCombat(enemies, numVanguards, numSupports);
        GameManager.Instance.TurnOnTurorial();
    }

}
