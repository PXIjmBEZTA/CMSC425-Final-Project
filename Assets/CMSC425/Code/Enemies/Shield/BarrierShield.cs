using UnityEngine;

public class BarrierShield : MonoBehaviour
{
    public int health = 3;
    public ShieldSupportEnemy owner;
    private GameObject visual;

    public void SetVisual(GameObject vis)
    {
        visual = vis;
    }

    public void TakeDamage()
    {
        health--;
        Debug.Log($"[BARRIER] {gameObject.name} barrier hit! Health remaining: {health}");
        AudioManager.Instance.Play(AudioManager.SoundType.ShieldHit);

        if (health <= 0)
        {
            Debug.Log($"[BARRIER] {gameObject.name} barrier destroyed!");
            DestroyShield();
        }
    }

    public void DestroyShield()
    {
        if (visual != null)
        {
            Destroy(visual);
        }
        Destroy(this);
    }

    void OnDestroy()
    {
        if (visual != null)
        {
            Destroy(visual);
        }
    }
}