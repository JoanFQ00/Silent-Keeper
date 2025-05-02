using UnityEngine;
using System;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public GameObject player;
    public GameObject[] allSkills;

    private GameObject skill1;
    private GameObject skill2;
    private GameObject passiveSkill;

    private bool canStartCooldown1, canStartCooldown2;
    private float cooldownSkill1, cooldownSkill2;
    private uint numUsesSkill1, numUsesSkill2;

    public static Action<Sprite, GameObject, float, SkillUI.SlotNumber> OnSlotImageUpdated;
    public static Action<uint, SkillUI.SlotNumber> OnSlotUsesUpdated;
    public static Action<float, SkillUI.SlotNumber> OnSlotCooldownUpdated;
    public static Action<bool, SkillUI.SlotNumber> OnSlotCooldownActivatedUpdated;
    public static Action<bool, SkillUI.SlotNumber> OnSlotImageColorWhiteUpdated;
    public static Action<string> OnSlotTextUpdated;

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

    //public enum SkillNumber
    //{
    //    SKILL1 = 0,
    //    SKILL2,
    //    SKILL3
    //}

    private void Start()
    {
        canStartCooldown1 = false;
        canStartCooldown2 = false;

        foreach (GameObject g in allSkills)
        {
            g.GetComponent<ISkill>().SetInitialHasFinished();
        }

        //Esto es solo para el prototipo
        AssignSkillToSlot(allSkills[0], 1);
        AssignSkillToSlot(allSkills[1], 2);
        AssignSkillToSlot(allSkills[2], 3);
    }

    private void Update()
    {
        //ESTO BORRAR CUANDO ESTÉN LOS INPUTS BIEN IMPLEMENTADOS
        //if (skill1 != null && Input.GetKeyDown(KeyCode.Q) && 
        //    GameObject.FindGameObjectWithTag("Player").GetComponent<MonsterMove>().GetCurrentMode() == MonsterMove.Mode.MonsterCon)

        if (skill1 != null && Input.GetKeyDown(KeyCode.LeftShift))
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

        if (skill1 != null)
        {
            if (!skill1.GetComponent<ISkill>().HasFinished())
                skill1.GetComponent<ISkill>().UpdateSkill();

            if (skill1.GetComponent<ISkill>().HasFinished() && canStartCooldown1 && numUsesSkill1 > 0)
            {
                if (cooldownSkill1 >= 0f)
                {
                    OnSlotCooldownActivatedUpdated?.Invoke(true, SkillUI.SlotNumber.SLOT1);
                    OnSlotCooldownUpdated?.Invoke(cooldownSkill1, SkillUI.SlotNumber.SLOT1);
                    OnSlotImageUpdated?.Invoke(skill1.GetComponent<ISkill>().GetSkill().gameplayImageBW, skill1, cooldownSkill1, SkillUI.SlotNumber.SLOT1);
                    OnSlotImageColorWhiteUpdated?.Invoke(true, SkillUI.SlotNumber.SLOT1);

                    cooldownSkill1 -= Time.deltaTime;
                }
                else
                {
                    canStartCooldown1 = false;
                    OnSlotCooldownActivatedUpdated?.Invoke(false, SkillUI.SlotNumber.SLOT1);
                    OnSlotImageUpdated?.Invoke(skill1.GetComponent<ISkill>().GetSkill().gameplayImage, skill1, cooldownSkill1, SkillUI.SlotNumber.SLOT1);
                }
            }
            else if (skill1.GetComponent<ISkill>().HasFinished() && canStartCooldown1 && numUsesSkill1 == 0)
            {
                canStartCooldown1 = false;
                OnSlotImageColorWhiteUpdated?.Invoke(true, SkillUI.SlotNumber.SLOT1);
                OnSlotImageUpdated?.Invoke(skill1.GetComponent<ISkill>().GetSkill().gameplayImageBW, skill1, cooldownSkill1, SkillUI.SlotNumber.SLOT1);

                //DESACTIVAR LA HABILIDAD (Mostrar en transparente)
                //skill1.Color = blalblalba;
            }
        }

        //ESTO BORRAR CUANDO ESTÉN LOS INPUTS BIEN IMPLEMENTADOS
        //if (skill2 != null && Input.GetKeyDown(KeyCode.E) &&
        //GameObject.FindGameObjectWithTag("Player").GetComponent<MonsterMove>().GetCurrentMode() == MonsterMove.Mode.MonsterCon)

        if (skill2 != null && Input.GetMouseButtonDown(0))
        {
            if (skill2.GetComponent<ISkill>().HasFinished() && !canStartCooldown2 && numUsesSkill2 > 0)
            {
                numUsesSkill2--;
                canStartCooldown2 = true;
                cooldownSkill2 = skill2.GetComponent<ISkill>().GetSkill().cooldown;

                skill2.GetComponent<ISkill>().Execute(player);

                OnSlotCooldownActivatedUpdated?.Invoke(false, SkillUI.SlotNumber.SLOT2);
                OnSlotCooldownUpdated?.Invoke(cooldownSkill2, SkillUI.SlotNumber.SLOT2);
                OnSlotUsesUpdated?.Invoke(numUsesSkill2, SkillUI.SlotNumber.SLOT2);
                OnSlotImageColorWhiteUpdated?.Invoke(false, SkillUI.SlotNumber.SLOT2);
            }
        }

        if (skill2 != null)
        {
            if (!skill2.GetComponent<ISkill>().HasFinished())
                skill2.GetComponent<ISkill>().UpdateSkill();

            if (skill2.GetComponent<ISkill>().HasFinished() && canStartCooldown2 && numUsesSkill2 > 0)
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
            else if (skill2.GetComponent<ISkill>().HasFinished() && canStartCooldown2 && numUsesSkill2 == 0)
            {
                canStartCooldown2 = false;
                OnSlotImageColorWhiteUpdated?.Invoke(true, SkillUI.SlotNumber.SLOT2);
                OnSlotImageUpdated?.Invoke(skill2.GetComponent<ISkill>().GetSkill().gameplayImageBW, skill2, cooldownSkill2, SkillUI.SlotNumber.SLOT2);

                //DESACTIVAR LA HABILIDAD (Mostrar en transparente)
                //skill1.Color = blalblalba;
            }
        }

        //if (skill2 != null)
        //{
        //    if (!skill2.GetComponent<ISkill>().HasFinished())
        //        skill2.GetComponent<ISkill>().UpdateSkill();

        //    if (skill2.GetComponent<ISkill>().HasFinished() && canStartCooldown2 && numUsesSkill2 > 1)
        //    {
        //        //OnSlotCooldownUpdated?.Invoke(cooldownSkill2);

        //        if (cooldownSkill2 >= 0f) cooldownSkill2 -= Time.deltaTime;
        //        else canStartCooldown2 = false;
        //    }
        //    else if (skill2.GetComponent<ISkill>().HasFinished() && canStartCooldown2 && numUsesSkill2 == 0)
        //    {
        //        canStartCooldown2 = false;

        //        //DESACTIVAR LA HABILIDAD (Mostrar en transparente)
        //        //skill2.Color = blalblalba;
        //    }
        //}

        if (passiveSkill != null) //if (passiveSkill != null && Input.GetKeyDown(KeyCode.P))
        {
            passiveSkill.GetComponent<ISkill>().Execute(player);
        }
    }

    // Método para ser llamado desde la máquina exprendedora
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
            numUsesSkill2 = skill2.GetComponent<ISkill>().GetSkill().uses;
            OnSlotImageUpdated?.Invoke(skill2.GetComponent<ISkill>().GetSkill().gameplayImage, skill2, cooldownSkill2, SkillUI.SlotNumber.SLOT2);
            OnSlotImageColorWhiteUpdated?.Invoke(true, SkillUI.SlotNumber.SLOT2);
            OnSlotUsesUpdated?.Invoke(numUsesSkill2, SkillUI.SlotNumber.SLOT2);
        }
        else //slot 3 [LIMITAR DESDE LA MÁQUINA EXPRENDEDORA ESTE VALOR!]
        {
            passiveSkill = skill;
            OnSlotImageUpdated?.Invoke(passiveSkill.GetComponent<ISkill>().GetSkill().gameplayImage, passiveSkill, 0, SkillUI.SlotNumber.SLOT3);
        }
    }

    //YA MIRARÉ SI SE HA DE AÑADIR UNA COLUMNA MÁS AL EXCXEL
    public void SetNumUsesSkill(ISkill reloadSkill)
    {
        foreach (GameObject g in allSkills)
        {
            if (skill1.GetComponent<ISkill>() == reloadSkill)
            {
                numUsesSkill1 = skill1.GetComponent<ISkill>().GetSkill().uses;
                OnSlotUsesUpdated?.Invoke(numUsesSkill1, SkillUI.SlotNumber.SLOT1);
                OnSlotImageUpdated?.Invoke(skill1.GetComponent<ISkill>().GetSkill().gameplayImage, skill1, cooldownSkill1, SkillUI.SlotNumber.SLOT1);

                //DESACTIVAR LA HABILIDAD QUE HA HECHO ESTO
                //skill2.Color = blalblalba;
            }
            else if (skill2.GetComponent<ISkill>() == reloadSkill)
            {
                numUsesSkill2 = skill2.GetComponent<ISkill>().GetSkill().uses;
                OnSlotUsesUpdated?.Invoke(numUsesSkill2, SkillUI.SlotNumber.SLOT2);
                OnSlotImageUpdated?.Invoke(skill2.GetComponent<ISkill>().GetSkill().gameplayImage, skill2, cooldownSkill2, SkillUI.SlotNumber.SLOT2);

                //DESACTIVAR LA HABILIDAD QUE HA HECHO ESTO
                //skill1.Color = blalblalba;
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
                cooldownSkill2 = skill2.GetComponent<ISkill>().GetSkill().cooldown;

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
}