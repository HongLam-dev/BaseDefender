using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private SkillSlot[] skilSlots;
    [SerializeField] Transform player;
    IReadOnlyList<SkillRuntime> skills;
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text stoneText;
    [SerializeField] private TMP_Text crystalText;
    [SerializeField] private TMP_Text soulText;

    [SerializeField] private Image workDetailIcon;
    [SerializeField] private TMP_Text workDetailTxt;

    [SerializeField] private Button workButton;

    [SerializeField] private Vector3 targetIndicatorOffset;

    [SerializeField] private Transform targetIndicator;

    [SerializeField] private PlayerResourceManager resourceManager;

    private Health playerHealth;
    [SerializeField] private Image healthFill;
    [SerializeField] private TMP_Text healthText;
    private PlayerInteraction interaction;

    private void Start()
    {

        playerHealth = player.GetComponent<Health>();
        interaction = player.GetComponent<PlayerInteraction>();

        playerHealth.OnHealthChange += HandleHealthChange;
        interaction.OnTargetWorkChange += HandleTargetWorkChange;
        interaction.OnNearByWorkProgressUpdate += UpdateWorkloadUI;
        resourceManager.OnResourceAdded+=HandleResourceAdded;

        ClearSkillUI();
        ClearResourceUI();
        UpdateHealthUI();

        this.skills = player.GetComponent<CharacterCombat>().Skills;
        for (int i = 0; i < skilSlots.Length; i++)
        {
            if (i >= skills.Count-1)
            {
                skilSlots[i].gameObject.SetActive(false);
            }
            else
            {
                skilSlots[i].SetSkill(skills[i].skillData.Icon);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i < skills.Count(); i++)
        {
            skilSlots[i].UpdateCooldownUI(skills[i].CooldownRemaining, skills[i].SkillCooldown);
        }
    }

    private void ClearSkillUI()
    {
        foreach (SkillSlot slot in skilSlots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    private void ClearResourceUI()
    {
        UpdateCrystalUI(0);
        UpdateStoneUI(0);
        UpdateSoulUI(0);
        UpdateWoodUI(0);
    }

    private void HandleHealthChange(HealthChangeInfo info)
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthFill.fillAmount = (float)playerHealth.CurrentHealth / playerHealth.MaxHealth;
        healthText.text = playerHealth.CurrentHealth.ToString() + "/" + playerHealth.MaxHealth.ToString();
    }

    public void UpdateWoodUI(int amount)
    {
        woodText.text = amount.ToString();
    }
    public void UpdateCrystalUI(int amount)
    {
        crystalText.text = amount.ToString();
    }
    public void UpdateStoneUI(int amount)
    {
        stoneText.text = amount.ToString();
    }
    public void UpdateSoulUI(int amount)
    {
        soulText.text = amount.ToString();
    }

    private void DisplayWorkDetail(int remainingWorkload, Sprite icon)
    {
        workDetailTxt.text = remainingWorkload.ToString();
        workDetailIcon.sprite = icon;
        workDetailTxt.gameObject.SetActive(true);
        workDetailIcon.gameObject.SetActive(true);
        workButton.gameObject.SetActive(true);
    }

    private void UpdateWorkloadUI(WorkInfo info)
    {
        workDetailTxt.text = info.remainingWorkload.ToString();
    }

    private void DisableWorkDetail()
    {
        workDetailTxt.gameObject.SetActive(false);
        workDetailIcon.gameObject.SetActive(false);
        workButton.gameObject.SetActive(false);
    }

    private void HandleTargetWorkChange(IWork work)
    {
        if(work!=null)
        {
            WorkInfo info = work.GetWorkInfo();
            targetIndicator.GetComponent<PingPongAroundCenter>()?.SetPosition(info.position + targetIndicatorOffset);
            targetIndicator.gameObject.SetActive(true);
            DisplayWorkDetail(info.remainingWorkload,info.icon);
        }
        else
        {
            targetIndicator.gameObject.SetActive(false);
            DisableWorkDetail();
        }
    }

    private void HandleResourceAdded(ItemStack resource)
    {
        switch(resource.item.id)
        {
            case "resource_wood":
                UpdateWoodUI(resourceManager.GetResourceAmount(resource.item.id));
                break;
            case "resource_stone":
                UpdateStoneUI(resourceManager.GetResourceAmount(resource.item.id));
                break;
            case "resource_crystal":
                UpdateCrystalUI(resourceManager.GetResourceAmount(resource.item.id));
                break;
            case "resource_soul":
                UpdateSoulUI(resourceManager.GetResourceAmount(resource.item.id));
                break;
        }
    }
}
