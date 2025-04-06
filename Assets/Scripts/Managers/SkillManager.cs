using Unity.VisualScripting;
using UnityEngine;
using System;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public GameObject player;
    public GameObject[] allSkills; // Todas las habilidades disponibles

    private GameObject skill1;
    private GameObject skill2;
    private GameObject passiveSkill;

    private bool canStartCooldown1, canStartCooldown2;
    private float cooldownSkill1, cooldownSkill2;
    private uint numUsesSkill1, numUsesSkill2;

    public static Action<Sprite, GameObject, float, SkillUI.SlotNumber> OnSlotImageUpdated;
    public static Action<uint, SkillUI.SlotNumber> OnSlotUsesUpdated;
    public static Action<float, SkillUI.SlotNumber> OnSlotCooldownUpdated; //bool para activar o no el texto e pantalla en el SkillUI
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

    public enum SkillNumber
    {
        SKILL1 = 0,
        SKILL2
    }

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
    }

    private void Update()
    {
        //ESTO BORRAR CUANDO ESTÉN LOS INPUTS BIEN IMPLEMENTADOS
        if (skill1 != null && Input.GetKeyDown(KeyCode.K) && 
            GameObject.FindGameObjectWithTag("Player").GetComponent<MonsterMove>().GetCurrentMode() == MonsterMove.Mode.MonsterCon)
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

        if (skill2 != null)
        {
            if (!skill2.GetComponent<ISkill>().HasFinished())
                skill2.GetComponent<ISkill>().UpdateSkill();

            if (skill2.GetComponent<ISkill>().HasFinished() && canStartCooldown2 && numUsesSkill2 > 1)
            {
                //OnSlotCooldownUpdated?.Invoke(cooldownSkill2);

                if (cooldownSkill2 >= 0f) cooldownSkill2 -= Time.deltaTime;
                else canStartCooldown2 = false;
            }
            else if (skill2.GetComponent<ISkill>().HasFinished() && canStartCooldown2 && numUsesSkill2 == 0)
            {
                canStartCooldown2 = false;

                //DESACTIVAR LA HABILIDAD (Mostrar en transparente)
                //skill2.Color = blalblalba;
            }
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
        }
        else //slot 3 [LIMITAR DESDE LA MÁQUINA EXPRENDEDORA ESTE VALOR!]
        {
            passiveSkill = skill;
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
            else
            {
                numUsesSkill2 = skill2.GetComponent<ISkill>().GetSkill().uses;
                OnSlotUsesUpdated?.Invoke(numUsesSkill2, SkillUI.SlotNumber.SLOT2);

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
}