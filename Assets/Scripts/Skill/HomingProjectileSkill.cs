using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill",
    menuName = "Game/Skills/Homing Projectile"
)]
public class HomingProjectileSkill : ProjectileSkill
{
    [SerializeField]private GameObject projectilePrefab;
    [SerializeField] private float rotationSpeed=10;

    override public void ExcuteStage(SkillCastData castData, int state)
    {
        GameObject projectileObj = ProjectileManager.Instance.GetProjectile(projectilePrefab);
        HomingProjectile projectile = projectileObj.GetComponent<HomingProjectile>();
        projectileObj.transform.position = castData.projectileOrigin;
        Vector3 direction = castData.targetPosition - castData.projectileOrigin;

        projectile.Initialize(config,CalculateDamgeInfo(castData),castData.target,rotationSpeed);
    }

    protected override int CalculateDamage(int atk)
    {
        return baseDamage + atk;
    }

    public override bool RequireTarget()
    {
        return true;
    }
}
