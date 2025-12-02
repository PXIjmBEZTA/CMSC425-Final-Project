using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class Bomber : MonoBehaviour, IEnemy
{
    public GameObject bombPrefab;
    private float shootCooldown;
    private int bulletCount;
    private float spreadAngle;
    private bool canShoot = true;

    public int HP { get; set; } = 150;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public EnemyRole role { get; set; } = EnemyRole.Vanguard;
    void Start()
    {
        StartCoroutine(InitialStall());
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot)
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

    //Behavior 1:
    //Place two bombs on the map that will blow up after some time
    //When it blows up, a bunch of bullets fly out of it
    //There is a one second delay between the bomb placements
    public IEnumerator Behavior1()
    {
        canShoot = false;
        shootCooldown = 3;
        PlaceBomb(); //The bomb itself handles the explosion
        yield return new WaitForSeconds(1);
        PlaceBomb(); //P
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    //Behavior2:
    //Place a row of bombs (top or bottom)
    public IEnumerator Behavior2()
    {
        canShoot = false;
        shootCooldown = 6;

        int numBombs = 3;
        float minX = -5.5f;
        float maxX = 5.5f;
        float minZ = -10.5f;
        float maxZ = -7.5f;
        float lengthOfField = maxX - minX;
        float deltaBomb = lengthOfField / (numBombs - 1); // Distance between one bomb and another
        float y = 0.125f;
        float z = (Random.Range(0, 2) == 0) ? minZ: maxZ;

        for (int i = 0; i < numBombs; i++)
        {
            PlaceBomb(minX + deltaBomb * i, y, z);
        }

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }


    //Behavior3:
    //Place an entire column of bombs (left, middle, or right)
    public IEnumerator Behavior3()
    {
        canShoot = false;
        shootCooldown = 6;

        int numBombs = 3;
        float minX = -5.5f;
        float maxX = 5.5f;
        float minZ = -10.5f;
        float maxZ = -7.5f;
        float heightOfField = maxZ - minZ;
        float deltaBomb = heightOfField / (numBombs - 1); // Distance between one bomb and another
        float y = 0.125f;

        float delta = 1;
        float x = minX + delta;
        float xPosition = Random.Range(0, 3); //left, middle, or right
        if (xPosition == 1)
            x = (maxX + minX) / 2;
        else if (xPosition == 2)
            x = maxX - delta;

        for (int i = 0; i < numBombs; i++)
        {
            PlaceBomb(x, y, minZ + deltaBomb * i);
        }

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    //Places a bomb at random coordinates
    private void PlaceBomb()
    {
        float minX = -5.5f;
        float maxX = 5.5f;
        float minZ = -10.5f;
        float maxZ = -7.5f;

        //These are coordinates on the Square of Movement
        float x = Random.Range(minX, maxX);
        float y = 0.125f;
        float z = Random.Range(minZ, maxZ);

        Vector3 spawnPosition = new Vector3(x, y, z);
        Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
    }

    //Places a bomb at specific coordinates
    private void PlaceBomb(float x, float y, float z)
    {
        Vector3 spawnPosition = new Vector3(x, y, z);
        Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
    }

    private IEnumerator InitialStall()
    {
        yield return new WaitForSeconds(2);
    }
}
