using UnityEngine;
public interface ISkillCaster
{
    Transform CasterTransform { get; }
    Vector3 Forward { get; }
    int TeamID { get; }
    int Atk { get; }
    int KnockBackForce { get; }
}

