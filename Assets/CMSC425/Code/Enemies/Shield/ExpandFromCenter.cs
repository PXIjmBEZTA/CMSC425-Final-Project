using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandFromCenter : MonoBehaviour
{
    public float targetX = 3f;     //width after expanding
    public float duration = 0.5f;  //how long the expansion lasts

    void Start()
    {
        StartExpand();
    }

    public void StartExpand()
    {
        StartCoroutine(Expand());
    }

    IEnumerator Expand()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = new Vector3(targetX, startScale.y, startScale.z);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        transform.localScale = endScale;
    }
}
