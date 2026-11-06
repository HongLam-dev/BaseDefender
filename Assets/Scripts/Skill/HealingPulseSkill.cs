using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill",
    menuName = "Echoes/Skills/Healing Pulse"
)]
public class HealingPulseSkill : SkillBase
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private int healTime = 1;

    [SerializeField]
    private float healInterval = 1;

    override public void ExcuteState(SkillCastData castData, int state)
    {
        GameObject healObject=Instantiate(prefab, castData.target.position, Quaternion.identity);
        healObject.transform.SetParent(castData.target);

        HealingPulse healingPulse = healObject.GetComponent<HealingPulse>();
        Health target = castData.target.GetComponent<Health>();
        healingPulse.Initialize(CalculateDamage(castData.caster.Atk),healInterval,healTime,target);
    }

    protected override int CalculateDamage(int atk)
    {
        return baseDamage;
    }
}
