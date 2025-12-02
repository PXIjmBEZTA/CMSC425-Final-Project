using UnityEngine;

public class BarrierShield : MonoBehaviour
{
    public int health = 3;
    private GameObject visual;

    public void SetVisual(GameObject vis)
    {
        visual = vis;
    }

    public void TakeDamage()
    {
        health--;
        Debug.Log($"[BARRIER] {gameObject.name} barrier hit! Health remaining: {health}");

        if (health <= 0)
        {
            Debug.Log($"[BARRIER] {gameObject.name} barrier destroyed!");
            if (visual != null)
            {
                Destroy(visual);
            }
            Destroy(this);
        }
    }

    void OnDestroy()
    {
        if (visual != null)
        {
            Destroy(visual);
        }
    }
}