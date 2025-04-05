using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEditor.Experimental.GraphView;

public class SkillUI : MonoBehaviour
{
    public enum SlotNumber
    {
        SLOT1 = 0,
        SLOT2,
        SLOT3
    }

    public SlotNumber currentSlot;
    public TextMeshProUGUI cooldownLabel, usesLabel;

    public Image normalSkillImageUI, cooldownSkillImageUI;

    void Start()
    {
        normalSkillImageUI.type = Image.Type.Filled;
        normalSkillImageUI.fillMethod = Image.FillMethod.Radial360;
        normalSkillImageUI.fillOrigin = (int)Image.Origin360.Top;
        normalSkillImageUI.fillClockwise = true;

        usesLabel.gameObject.SetActive(true);
        cooldownLabel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        SkillManager.OnSlotImageUpdated += UploadSlotImageUI;
        SkillManager.OnSlotUsesUpdated += UploadSlotUsesUI;
        SkillManager.OnSlotCooldownUpdated += UploadSlotCooldownUI;
        SkillManager.OnSlotCooldownActivatedUpdated += ActivateCooldownText;
        SkillManager.OnSlotImageColorWhiteUpdated += SetColorImage;
    }

    private void OnDisable()
    {
        SkillManager.OnSlotImageUpdated -= UploadSlotImageUI;
        SkillManager.OnSlotUsesUpdated -= UploadSlotUsesUI;
        SkillManager.OnSlotCooldownUpdated -= UploadSlotCooldownUI;
        SkillManager.OnSlotCooldownActivatedUpdated -= ActivateCooldownText;
        SkillManager.OnSlotImageColorWhiteUpdated -= SetColorImage;
    }

    private void UploadSlotImageUI(Sprite image, GameObject skill, float cooldown, SlotNumber slot)
    {
        if (currentSlot == slot)
        {
            if (image == skill.GetComponent<ISkill>().GetSkill().gameplayImageBW) // Si la imagen es la blanca (cooldown)
            {
                normalSkillImageUI.sprite = image;
                normalSkillImageUI.fillAmount = Mathf.Clamp01(1 - (cooldown / skill.GetComponent<ISkill>().GetSkill().cooldown));

                cooldownSkillImageUI.gameObject.SetActive(true);
                cooldownSkillImageUI.sprite = skill.GetComponent<ISkill>().GetSkill().gameplayImageWhite;
                cooldownSkillImageUI.fillAmount = 1f;
            }
            else // Si la imagen no es gris (es la imagen a color)
            {
                normalSkillImageUI.sprite = image;
                normalSkillImageUI.fillAmount = 1f;

                cooldownSkillImageUI.gameObject.SetActive(false);
            }
        }
    }

    private void UploadSlotUsesUI(uint numSkillUses, SlotNumber slot)
    {
        if (currentSlot == slot)
            usesLabel.text = numSkillUses.ToString();
    }

    private void UploadSlotCooldownUI(float cooldown, SlotNumber slot)
    {
        if (currentSlot == slot)
            cooldownLabel.text = cooldown.ToString("F0");
    }

    private void ActivateCooldownText(bool activate, SlotNumber slot)
    {
        if (currentSlot == slot)
        {
            cooldownLabel.gameObject.SetActive(activate);
        }
    }

    private void SetColorImage(bool isWhite, SlotNumber slot)
    {
        if (currentSlot == slot)
            if (isWhite)
                normalSkillImageUI.color = Color.white;
            else
                normalSkillImageUI.color = Color.yellow; //Se puede poner un shades :)
    }
}