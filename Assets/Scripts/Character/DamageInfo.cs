using JetBrains.Annotations;
using NUnit.Framework;

public struct DamageInfo
{
    public int TeamID;
    public int Damage;
    public float Force;

    public DamageInfo(int teamID,int damge, float force)
    {
        TeamID = teamID;
        Damage = damge;
        Force = force;
    }
}