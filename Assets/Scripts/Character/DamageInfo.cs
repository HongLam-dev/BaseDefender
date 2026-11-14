using UnityEngine;
public struct DamageInfo
{
    public int TeamID;
    public int Damage;
    public float Force;

    public Vector3 direction;
    public DamageInfo(int teamID,int damge, float force,Vector3 direction)
    {
        TeamID = teamID;
        Damage = damge;
        Force = force;
        this.direction = direction;
    }
}