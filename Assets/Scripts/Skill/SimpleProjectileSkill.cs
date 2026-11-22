using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill",
    menuName = "Game/Skills/Simple Projectile"
)]
public class SimpleProjectileSkill: SkillBase
{
    [Header("Projectile Setting")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField] float projectileSpeed=15;

    public float ProjectileSpeed => projectileSpeed;
    override public void ExcuteStage(SkillCastData castData,int state)
    {
        GameObject projectileObj = Instantiate(projectilePrefab, castData.caster.CasterTransform.position, Quaternion.identity);
        Projectile projectile= projectileObj.GetComponent<Projectile>();
        Vector3 direction = castData.targetPosition - castData.caster.CasterTransform.position;
        projectile.Initialize(CalculateDamgeInfo(castData),direction, projectileSpeed);
    }

    protected override int CalculateDamage(int atk)
    {
        return baseDamage + atk;
    }
}
