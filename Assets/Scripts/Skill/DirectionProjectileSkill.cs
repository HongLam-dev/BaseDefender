using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill",
    menuName = "Game/Skills/Direction Projectile"
)]
public class DirectionProjectileSkill: SkillBase
{
    [Header("Projectile Setting")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField] float projectileSpeed=15;

    public float ProjectileSpeed => projectileSpeed;
    override public void ExcuteStage(SkillCastData castData,int state)
    {
        GameObject projectileObj = ProjectileManager.Instance.GetProjectile(projectilePrefab);
        DirectionProjectile projectile= projectileObj.GetComponent<DirectionProjectile>();
        projectileObj.transform.position = castData.projectileOrigin;
        Vector3 direction = castData.targetPosition - castData.projectileOrigin;

        projectile.Initialize(CalculateDamgeInfo(castData),direction, projectileSpeed);
    }

    protected override int CalculateDamage(int atk)
    {
        return baseDamage + atk;
    }
}
