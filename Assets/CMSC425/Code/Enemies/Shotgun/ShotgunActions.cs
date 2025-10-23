using Unity.VisualScripting;
using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    public float cooldown;
    public float nextActionTime;
    void Start()
    {
        nextActionTime = 1.0f;
    }

    void Update()
    {
        if (Time.time >= nextActionTime)
        {
            //Perform random behavior
            nextActionTime = Time.time + cooldown; //Cooldown returned by behavior function
        }
    }

}
