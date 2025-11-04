using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;

    public GameObject bigBulletPrefab; //alternative for big Shoot
    public Transform firePoint; //the exact place where the bullets originate from
    public float fireRate = 1; //seconds between shots
    private float nextTimeFire = 0f;
    public ButtonControl shootButton;
    public KeyControl secondShootButton;

    public float counter = 0f;

    private void Start()
    {
        fireRate = 1;
        shootButton = Mouse.current.leftButton;
        secondShootButton = Keyboard.current[Key.J];
        //shootButton = Keyboard.current.mKey;
        //shootButton = Keyboard.current.spaceKey;
    }
    // Update is called once per frame
    void Update()
    {
        if (nextTimeFire > 0)
            nextTimeFire -= Time.deltaTime;


        if (shootButton.isPressed || secondShootButton.isPressed)
            counter += Time.deltaTime;

        if (shootButton.wasReleasedThisFrame || secondShootButton.wasReleasedThisFrame)
        {
            if (counter >= 1.0f)
            {
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

    public void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        
    }

    public void BigShoot()
    {
        if (bigBulletPrefab != null && firePoint != null)
        {
            Instantiate(bigBulletPrefab, firePoint.position, firePoint.rotation);
        }

        
    }
}
