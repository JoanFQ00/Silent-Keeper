using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public GameObject children1, children2, children3, children4;

    void Start()
    {
        DisableChildern();

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
        SkillManager.OnSlotDisable += DisableChildern;
    }

    private void OnDisable()
    {
        SkillManager.OnSlotImageUpdated -= UploadSlotImageUI;
        SkillManager.OnSlotUsesUpdated -= UploadSlotUsesUI;
        SkillManager.OnSlotCooldownUpdated -= UploadSlotCooldownUI;
        SkillManager.OnSlotCooldownActivatedUpdated -= ActivateCooldownText;
        SkillManager.OnSlotImageColorWhiteUpdated -= SetColorImage;
        SkillManager.OnSlotDisable -= DisableChildern;
    }

    private void Update()
    {
        if (BalloonBox.balloonsInInventory > 0)
            EnableChidren();
    }

    private void UploadSlotImageUI(Sprite image, GameObject skill, float cooldown, SlotNumber slot)
    {
        if (currentSlot != SlotNumber.SLOT3)
        {
            if (currentSlot == slot)
            {
                if (image == skill.GetComponent<ISkill>().GetSkill().gameplayImageBW)
                {
                    normalSkillImageUI.sprite = image;
                    //normalSkillImageUI.fillAmount = Mathf.Clamp01(1 - (cooldown / skill.GetComponent<ISkill>().GetSkill().cooldown));
                    normalSkillImageUI.fillAmount = Mathf.Clamp01(1 - (cooldown / SkillManager.Instance.ballonCooldown));

                    cooldownSkillImageUI.gameObject.SetActive(true);
                    cooldownSkillImageUI.sprite = skill.GetComponent<ISkill>().GetSkill().gameplayImageWhite;
                    cooldownSkillImageUI.fillAmount = 1f;
                }
                else
                {
                    normalSkillImageUI.sprite = image;
                    normalSkillImageUI.fillAmount = 1f;

                    cooldownSkillImageUI.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (currentSlot == slot)
                normalSkillImageUI.sprite = image;
        }
    }

    private void UploadSlotUsesUI(uint numSkillUses, SlotNumber slot)
    {
        if (currentSlot == slot)
        {
            if (BalloonBox.balloonsInInventory == 3)
                usesLabel.text = "*FULL*";
            else
                usesLabel.text = numSkillUses.ToString();

        }
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
                normalSkillImageUI.color = Color.yellow;
    }

    private void EnableChidren()
    {
        children1.SetActive(true);
        children2.SetActive(true);
        children3.SetActive(true);
        children4.SetActive(true);
    }

    private void DisableChildern()
    {
        children1.SetActive(false);
        children2.SetActive(false);
        children3.SetActive(false);
        children4.SetActive(false);
    }
}