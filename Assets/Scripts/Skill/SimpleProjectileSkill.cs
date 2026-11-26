using System;
using Unity.VisualScripting;
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
    [SerializeField] bool isHomming=false;

    public float ProjectileSpeed => projectileSpeed;
    override public void ExcuteStage(SkillCastData castData,int state)
    {
        GameObject projectileObj = ProjectileManager.Instance.GetProjectile(projectilePrefab);
        Projectile projectile= projectileObj.GetComponent<Projectile>();
        projectileObj.transform.position = castData.projectileOrigin;
        Vector3 direction = castData.targetPosition - castData.caster.CasterTransform.position;

        projectile.Initialize(CalculateDamgeInfo(castData),direction, projectileSpeed);
    }

    protected override int CalculateDamage(int atk)
    {
        return baseDamage + atk;
    }
}
