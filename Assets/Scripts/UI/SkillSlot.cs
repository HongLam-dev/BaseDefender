using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Image cooldown;

    [SerializeField] TMP_Text cooldownTxt;

    public void UpdateCooldownUI(float currentCooldown,float maxCooldown)
    {
        cooldown.fillAmount = currentCooldown / maxCooldown;
        if (currentCooldown <= 0)
        { 
            cooldownTxt.gameObject.SetActive(false);
            return;

        }
        cooldownTxt.text = ((int)currentCooldown).ToString();
    }

    public void SetSkill(Sprite icon)
    {
        this.icon.sprite = icon;
        cooldownTxt.gameObject.SetActive(false);
        cooldown.fillAmount = 0;
    }
}
