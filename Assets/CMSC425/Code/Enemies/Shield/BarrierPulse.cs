using UnityEngine;

public class BarrierPulse : MonoBehaviour
{
    private Color color1;
    private Color color2;
    private Renderer rend;
    private float pulseSpeed = 2f;

    public void SetColors(Color c1, Color c2)
    {
        color1 = c1;
        color2 = c2;
    }

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = new Material(rend.material);
        }
    }

    void Update()
    {
        if (rend != null)
        {
            float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            rend.material.color = Color.Lerp(color1, color2, t);
        }
    }
}