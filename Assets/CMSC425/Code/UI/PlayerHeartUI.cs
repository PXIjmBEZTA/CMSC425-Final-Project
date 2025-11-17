using UnityEngine;
using System.Collections.Generic;

public class PlayerHeartUI : MonoBehaviour
{
    
    public void DestroyHeart()
    {
        Destroy(gameObject);
    }
}
