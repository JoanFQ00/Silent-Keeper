using UnityEngine;
using System;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public GameObject player;
    public GameObject[] allSkills;
    public float ballonCooldown = 2.0f;

    private GameObject skill1;
    private GameObject skill2;
    private GameObject passiveSkill;

    private bool canStartCooldown1, canStartCooldown2;
    private uint numUsesSkill1, numUsesSkill2;
    private float cooldownSkill1, cooldownSkill2;

    public static Action<Sprite, GameObject, float, SkillUI.SlotNumber> OnSlotImageUpdated;
    public static Action<uint, SkillUI.SlotNumber> OnSlotUsesUpdated;
    public static Action<float, SkillUI.SlotNumber> OnSlotCooldownUpdated;
    public static Action<bool, SkillUI.SlotNumber> OnSlotCooldownActivatedUpdated;
    public static Action<bool, SkillUI.SlotNumber> OnSlotImageColorWhiteUpdated;
    public static Action<string> OnSlotTextUpdated;
    public static Action OnSlotDisable;

    [SerializeField] private SpriteRenderer arrowRenderer;
    [SerializeField] private float arrowDistance = 1f;

    private bool isAimingBalloon = false;

    [SerializeField] public float maxArrowTime = 5f;
    private float arrowTimer = 0f;
    private bool isArrowTimerRunning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        canStartCooldown1 = false;
        canStartCooldown2 = false;

        foreach (GameObject g in allSkills)
        {
            g.GetComponent<ISkill>().SetInitialHasFinished();
        }

        AssignSkillToSlot(allSkills[0], 1);
        AssignSkillToSlot(allSkills[1], 2);
        AssignSkillToSlot(allSkills[2], 3);

        arrowRenderer.enabled = false;
    }

    public void OnBalloonTaked()
    {
        numUsesSkill2 = BalloonBox.balloonsInInventory;
        OnSlotUsesUpdated?.Invoke(numUsesSkill2, SkillUI.SlotNumber.SLOT2);
        OnSlotImageUpdated?.Invoke(skill2.GetComponent<ISkill>().GetSkill().gameplayImage, skill2, cooldownSkill2, SkillUI.SlotNumber.SLOT2);
    }

    private void Update()
    {
        if (skill2 != null)
        {
            if (arrowRenderer == true)
            {
                AimArrow();
                if (arrowRenderer.enabled && isArrowTimerRunning)
                {
                    arrowTimer += Time.deltaTime;
                    if (arrowTimer >= maxArrowTime)
                    {
                        arrowRenderer.enabled = false;
                        isAimingBalloon = false;
                        isArrowTimerRunning = false;
                        arrowTimer = 0f;
                    }
                }
            }
            if (!GameManager.Instance.isPaused && !IntroRoomsManager.Instance.roomInAnimation)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {

                    if (!isAimingBalloon && skill2.GetComponent<ISkill>().HasFinished() && !canStartCooldown2 && BalloonBox.balloonsInInventory > 0)
                    {
                        isAimingBalloon = true;
                        arrowRenderer.enabled = true;

                        isArrowTimerRunning = true;
                        arrowTimer = 0f;
                    }
                    else if (isAimingBalloon)
                    {
                        arrowRenderer.enabled = false;
                        isAimingBalloon = false;

                        isArrowTimerRunning = false;
                        arrowTimer = 0f;

                        if (skill2.GetComponent<ISkill>().HasFinished() && !canStartCooldown2 && BalloonBox.balloonsInInventory > 0)
                        {
                            BalloonBox.balloonsInInventory--;
                            canStartCooldown2 = true;
                            cooldownSkill2 = ballonCooldown;

                            skill2.GetComponent<ISkill>().Execute(player);

                            OnSlotCooldownActivatedUpdated?.Invoke(false, SkillUI.SlotNumber.SLOT2);
                            OnSlotCooldownUpdated?.Invoke(cooldownSkill2, SkillUI.SlotNumber.SLOT2);
                            OnSlotUsesUpdated?.Invoke(BalloonBox.balloonsInInventory, SkillUI.SlotNumber.SLOT2);
                            OnSlotImageColorWhiteUpdated?.Invoke(false, SkillUI.SlotNumber.SLOT2);
                        }
                    }
                }
            }
        }

        if (skill2 != null)
        {
            if (!skill2.GetComponent<ISkill>().HasFinished())
                skill2.GetComponent<ISkill>().UpdateSkill();

            if (skill2.GetComponent<ISkill>().HasFinished() && canStartCooldown2 && BalloonBox.balloonsInInventory > 0)
            {
                if (cooldownSkill2 >= 0f)
                {
                    OnSlotCooldownActivatedUpdated?.Invoke(true, SkillUI.SlotNumber.SLOT2);
                    OnSlotCooldownUpdated?.Invoke(cooldownSkill2, SkillUI.SlotNumber.SLOT2);
                    OnSlotImageUpdated?.Invoke(skill2.GetComponent<ISkill>().GetSkill().gameplayImageBW, skill2, cooldownSkill2, SkillUI.SlotNumber.SLOT2);
                    OnSlotImageColorWhiteUpdated?.Invoke(true, SkillUI.SlotNumber.SLOT2);

                    cooldownSkill2 -= Time.deltaTime;
                }
                else
                {
                    canStartCooldown2 = false;
                    OnSlotCooldownActivatedUpdated?.Invoke(false, SkillUI.SlotNumber.SLOT2);
                    OnSlotImageUpdated?.Invoke(skill2.GetComponent<ISkill>().GetSkill().gameplayImage, skill2, cooldownSkill2, SkillUI.SlotNumber.SLOT2);
                }
            }
            else if (skill2.GetComponent<ISkill>().HasFinished() && canStartCooldown2 && BalloonBox.balloonsInInventory == 0)
            {
                canStartCooldown2 = false;
                OnSlotImageColorWhiteUpdated?.Invoke(true, SkillUI.SlotNumber.SLOT2);
                OnSlotImageUpdated?.Invoke(skill2.GetComponent<ISkill>().GetSkill().gameplayImageBW, skill2, cooldownSkill2, SkillUI.SlotNumber.SLOT2);
                OnSlotDisable?.Invoke();
            }
        }
    }


    // Method to be called from the vending machine
    public void AssignSkillToSlot(GameObject skill, int slot)
    {
        if (slot == 1)
        {
            skill1 = skill;
            numUsesSkill1 = skill1.GetComponent<ISkill>().GetSkill().uses;
            OnSlotImageUpdated?.Invoke(skill1.GetComponent<ISkill>().GetSkill().gameplayImage, skill1, cooldownSkill1, SkillUI.SlotNumber.SLOT1);
            OnSlotImageColorWhiteUpdated?.Invoke(true, SkillUI.SlotNumber.SLOT1);
            OnSlotUsesUpdated?.Invoke(numUsesSkill1, SkillUI.SlotNumber.SLOT1);
        }
        else if (slot == 2)
        {
            skill2 = skill;
            OnSlotImageUpdated?.Invoke(skill2.GetComponent<ISkill>().GetSkill().gameplayImage, skill2, cooldownSkill2, SkillUI.SlotNumber.SLOT2);
            OnSlotImageColorWhiteUpdated?.Invoke(true, SkillUI.SlotNumber.SLOT2);
            OnSlotUsesUpdated?.Invoke(BalloonBox.balloonsInInventory, SkillUI.SlotNumber.SLOT2);
        }
        else //slot 3 [LIMIT THIS VALUE FROM THE VENDING MACHINE!]
        {
            passiveSkill = skill;
            OnSlotImageUpdated?.Invoke(passiveSkill.GetComponent<ISkill>().GetSkill().gameplayImage, passiveSkill, 0, SkillUI.SlotNumber.SLOT3);
        }
    }

    // I’LL SEE LATER IF ANOTHER COLUMN NEEDS TO BE ADDED TO THE EXCEL
    public void SetNumUsesSkill(ISkill reloadSkill)
    {
        foreach (GameObject g in allSkills)
        {
            if (skill1.GetComponent<ISkill>() == reloadSkill)
            {
                numUsesSkill1 = skill1.GetComponent<ISkill>().GetSkill().uses;
                OnSlotUsesUpdated?.Invoke(numUsesSkill1, SkillUI.SlotNumber.SLOT1);
                OnSlotImageUpdated?.Invoke(skill1.GetComponent<ISkill>().GetSkill().gameplayImage, skill1, cooldownSkill1, SkillUI.SlotNumber.SLOT1);
            }
            else if (skill2.GetComponent<ISkill>() == reloadSkill)
            {
                numUsesSkill2 = skill2.GetComponent<ISkill>().GetSkill().uses;
                OnSlotUsesUpdated?.Invoke(numUsesSkill2, SkillUI.SlotNumber.SLOT2);
                OnSlotImageUpdated?.Invoke(skill2.GetComponent<ISkill>().GetSkill().gameplayImage, skill2, cooldownSkill2, SkillUI.SlotNumber.SLOT2);
            }
        }
    }

    //INPUTS
    public void OnSkillOneActivated()
    {
        if (skill1 != null)
        {
            if (skill1.GetComponent<ISkill>().HasFinished() && !canStartCooldown1 && numUsesSkill1 > 0)
            {
                numUsesSkill1--;
                canStartCooldown1 = true;
                cooldownSkill1 = skill1.GetComponent<ISkill>().GetSkill().cooldown;

                skill1.GetComponent<ISkill>().Execute(player);

                OnSlotCooldownActivatedUpdated?.Invoke(false, SkillUI.SlotNumber.SLOT1);
                OnSlotCooldownUpdated?.Invoke(cooldownSkill1, SkillUI.SlotNumber.SLOT1);
                OnSlotUsesUpdated?.Invoke(numUsesSkill1, SkillUI.SlotNumber.SLOT1);
                OnSlotImageColorWhiteUpdated?.Invoke(false, SkillUI.SlotNumber.SLOT1);
            }
        }
    }

    public void OnSkillTwoActivated()
    {
        if (skill2 != null)
        {
            if (skill2.GetComponent<ISkill>().HasFinished() && !canStartCooldown2 && numUsesSkill2 > 0)
            {
                numUsesSkill2--;
                canStartCooldown2 = true;
                cooldownSkill2 = ballonCooldown;

                skill2.GetComponent<ISkill>().Execute(player);

                OnSlotCooldownActivatedUpdated?.Invoke(false, SkillUI.SlotNumber.SLOT2);
                OnSlotCooldownUpdated?.Invoke(cooldownSkill2, SkillUI.SlotNumber.SLOT2);
                OnSlotUsesUpdated?.Invoke(numUsesSkill2, SkillUI.SlotNumber.SLOT2);
                OnSlotImageColorWhiteUpdated?.Invoke(false, SkillUI.SlotNumber.SLOT2);
            }
        }
    }

    public GameObject GetSlot(int slot)
    {
        switch (slot)
        {
            case 1:
                return skill1;

            case 2:
                return skill2;

            case 3:
                return passiveSkill;

            default:
                return null;
        }
    }


    private void AimArrow()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDirection = (mousePos - (Vector2)player.transform.position).normalized;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        arrowRenderer.transform.position = (Vector2)player.transform.position + aimDirection * arrowDistance;
        arrowRenderer.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}