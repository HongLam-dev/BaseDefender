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
        GameObject fireballObj = Instantiate(projectilePrefab, castData.caster.transform.position, Quaternion.identity);
        Projectile fireball= fireballObj.GetComponent<Projectile>();
        fireball.Initialize(new DamageInfo(castData.caster.TeamID, CalculateDamage(castData.caster.Atk),force), castData.caster.transform.position, castData.targetPosition);
    }

    protected override int CalculateDamage(int atk)
    {
        return baseDamage + atk;
    }
}
