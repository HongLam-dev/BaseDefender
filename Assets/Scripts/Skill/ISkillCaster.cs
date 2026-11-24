using UnityEngine;
public interface ISkillCaster
{
    Transform CasterTransform { get; }
    int TeamID { get; }
    int Atk { get; }
    int KnockBackForce { get; }
}

