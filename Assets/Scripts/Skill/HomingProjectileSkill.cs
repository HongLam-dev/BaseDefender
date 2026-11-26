using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill",
    menuName = "Game/Skills/Homing Projectile"
)]
public class HomingProjectileSkill : SkillBase
{
    [Header("Projectile Setting")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 15;

    public float ProjectileSpeed => projectileSpeed;
    override public void ExcuteStage(SkillCastData castData, int state)
    {
        GameObject projectileObj = ProjectileManager.Instance.GetProjectile(projectilePrefab);
        HomingProjectile projectile = projectileObj.GetComponent<HomingProjectile>();
        projectileObj.transform.position = castData.projectileOrigin;
        Vector3 direction = castData.targetPosition - castData.projectileOrigin;

        projectile.Initialize(CalculateDamgeInfo(castData),castData.target, projectileSpeed);
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
