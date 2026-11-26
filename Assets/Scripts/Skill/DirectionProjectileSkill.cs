using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill",
    menuName = "Game/Skills/Direction Projectile"
)]
public class DirectionProjectileSkill: ProjectileSkill
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private bool destroyOnHit=true;

    public float ProjectileSpeed => config.speed;
    override public void ExcuteStage(SkillCastData castData,int state)
    {
        GameObject projectileObj = ProjectileManager.Instance.GetProjectile(projectilePrefab);
        DirectionProjectile projectile= projectileObj.GetComponent<DirectionProjectile>();
        projectileObj.transform.position = castData.projectileOrigin;
        Vector3 direction = castData.targetPosition - castData.projectileOrigin;

        projectile.Initialize(config,CalculateDamgeInfo(castData),direction,destroyOnHit);
    }

    protected override int CalculateDamage(int atk)
    {
        return baseDamage + atk;
    }
}
