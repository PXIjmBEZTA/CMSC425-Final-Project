using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;

    public GameObject bigBulletPrefab; //alternative for big Shoot
    private Transform firePoint; //the exact place where the bullets originate from
    public float fireRate = 1; //seconds between shots
    private float nextTimeFire = 0f;
    public ButtonControl shootButton;
    public KeyControl secondShootButton;

    public float counter = 0f;

    public UnityEvent onShoot; //add unity events and later invoke them in the methods
    //(they will allow us to drag sounds to its field in the inspector view)
    //(make sure to import `using UnityEngine.Events;`)
    public UnityEvent onBigShoot;
    private AudioInputManager audioManager;


    private void Start()
    {
        shootButton = Mouse.current.leftButton;
        secondShootButton = Keyboard.current[Key.J];
        audioManager = AudioInputManager.Instance;
        firePoint = GetComponentInChildren<Transform>().Find("PlayerFirePoint");
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
            if (counter >= 1.0f) //this should be a variable. Oh well, too late to change it now
            {
                BigShoot();
                audioManager.PlayBigShootSound();
            }

            else
            {
                Shoot();
                audioManager.PlayShootSound();
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

    private bool Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        return true;
    }

    private bool BigShoot()
    {
        if (bigBulletPrefab != null && firePoint != null)
        {
            Instantiate(bigBulletPrefab, firePoint.position, firePoint.rotation);
        }

        return true;
    }

    
}
