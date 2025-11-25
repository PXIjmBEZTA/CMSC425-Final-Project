using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIShaker : MonoBehaviour
{
    private float shakeDuration = .30f;
    private float shakeMagnitude = 7f; // pixels

    public Image iconImage;          // Assign your profile icon Image here
    public Sprite defaultSprite;     // Normal
    public Sprite damageSprite;      // Hurt
    public Sprite deadSprite;        // **fugckinbg dies**

    private RectTransform rectTransform;
    private Vector2 originalAnchoredPos;
    private Coroutine shakeRoutine;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalAnchoredPos = rectTransform.anchoredPosition;
    }

    public void Shake()
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        if (iconImage != null && damageSprite != null)
            iconImage.sprite = damageSprite; //Show damage sprite

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;
            rectTransform.anchoredPosition = originalAnchoredPos + new Vector2(offsetX, offsetY);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalAnchoredPos;

        // If not dead, return to default sprite
        if (iconImage != null && defaultSprite != null && (iconImage.sprite != deadSprite))
            iconImage.sprite = defaultSprite;

        shakeRoutine = null;
    }

    public void SetDeadSprite()
    {
        if (iconImage != null && deadSprite != null)
            iconImage.sprite = deadSprite;
    }

    // Optional: call this when resetting the UI for respawn
    public void ResetToDefaultSprite()
    {
        if (iconImage != null && defaultSprite != null)
            iconImage.sprite = defaultSprite;
    }
}