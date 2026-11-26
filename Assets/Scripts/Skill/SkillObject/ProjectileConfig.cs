using UnityEngine;

[System.Serializable]
public struct ProjectileConfig
{
    public bool penetrateObjects;
    public float explodeRadius;
    public int explodeDamage;
    public float speed;
    public float lifeTime;
}