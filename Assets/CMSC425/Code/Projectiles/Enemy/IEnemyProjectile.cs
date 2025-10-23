using UnityEngine;
//This is an interface for all enemy projectiles since there may be multiple types.
public interface IEnemyProjectile
{
    int Damage { get; } //Damage dealt by projectile. Usually 1
    float Speed { get; } //Speed of projectile
    float LifeTime { get; } //Maximum time the projectile lasts on screen
    float TimeLeft { get; } //Time until lifetime expires
    void OnHit(GameObject target); //What to do when hit
}
