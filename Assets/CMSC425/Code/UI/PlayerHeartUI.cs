using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI; // <- for Images

public class PlayerHeartUI : MonoBehaviour
{

    public Sprite emptyHeartSprite; //to assign rthe blank heart container sprite
    public Sprite fullHeartSprite;
    public void SetEmpty()
    {
        Debug.Log("Heart SetEmpty called!");
        Image img = GetComponent<Image>();

        if (img != null && emptyHeartSprite != null)
        {
            img.sprite = emptyHeartSprite;
            Debug.Log($"Sprite changed to: {img.sprite}");
        }
        else
        {
            Debug.LogError("Failed to change sprite! Image is null: " + (img == null) + ", emptyHeartSprite is null: " + (emptyHeartSprite == null));
        }
    }


    public void SetFull()
    {
        Debug.Log("Heart SetFull called!");
        Image img = GetComponent<Image>();

        if (img != null && fullHeartSprite != null)
        {
            img.sprite = fullHeartSprite;
            Debug.Log($"Sprite changed to: {img.sprite}");
        }
        else
        {
            Debug.LogError("Failed to change sprite! Image is null: " + (img == null) + ", emptyHeartSprite is null: " + (emptyHeartSprite == null));
        }
    }

    public void DestroyHeart()
    {
        Destroy(gameObject);
    }
    
}
