using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Image cooldown;

    public void UpdateCooldownUI(float fill)
    {
        cooldown.fillAmount = fill;
    }

    public void SetSkill(Sprite icon)
    {
        this.icon.sprite = icon;
        UpdateCooldownUI(0);
    }
}
