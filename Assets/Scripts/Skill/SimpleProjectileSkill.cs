using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill",
    menuName = "Game/Skills/Simple Projectile"
)]
public class SimpleProjectileSkill: SkillBase
{
    [SerializeField]
    private GameObject projectilePrefab;

    override public void ExcuteStage(SkillCastData castData,int state)
    {
        GameObject fireballObj = Instantiate(projectilePrefab, castData.caster.CasterTransform.position, Quaternion.identity);
        Projectile fireball= fireballObj.GetComponent<Projectile>();
        fireball.Initialize(CalculateDamgeInfo(castData), castData.caster.CasterTransform.position, castData.targetPosition);
    }

    protected override int CalculateDamage(int atk)
    {
        return baseDamage + atk;
    }
}
