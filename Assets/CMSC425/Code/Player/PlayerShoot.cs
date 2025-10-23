using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint; //the exact place where the bullets originate from
    public float fireRate = 0.2f; //seconds between shots
    private float nextTimeFire = 0f;
    public ButtonControl shootButton;

    public float counter = 0f;
    private Vector3 scaleChange = new Vector3(.6f, .6f, .6f);

    public bool isBig;
    private void Start()
    {
        shootButton = Mouse.current.leftButton;
        //shootButton = Keyboard.current.mKey;
        //shootButton = Keyboard.current.spaceKey;
    }
    // Update is called once per frame
    void Update()
    {
        if (nextTimeFire > 0)
            nextTimeFire -= Time.deltaTime;


        if (shootButton.isPressed)
            counter += Time.deltaTime;

        if (shootButton.wasReleasedThisFrame)
        {
            Debug.Log($"Counter on release: {counter}");
            if (counter >= 1.2f)
            {
                Debug.Log("My charged shot is ready!!!!");
                BigShoot();
            }


            else
            {
                Shoot();
            }


            counter = 0f; // reset after firing
            nextTimeFire = fireRate;
        }


        // if (shootButton.isPressed && nextTimeFire <= 0)
        // {
        //     Shoot();
        //     nextTimeFire = fireRate;
        // }

    }
    
    //a wrapper method that encapsulates both

    public bool Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        isBig = false;
        return true;
        
    }

    public bool BigShoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.transform.localScale += scaleChange;  //modify this instance only
            //otherwise bulletprefab.transform.localScale would modify the original prefab itself, permanently changing it
        }
        isBig = true;
        return true;

        
    }
}
