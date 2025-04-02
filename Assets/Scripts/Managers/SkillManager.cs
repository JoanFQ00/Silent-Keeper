using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public SkillBase[] allSkills; // Todas las habilidades disponibles

    private SkillBase skill1;
    private SkillBase skill2;
    private SkillBase passiveSkill;

    [HideInInspector]
    public bool canUpdateSkill1, canUpdateSkill2;

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
        canUpdateSkill1 = false;
        canUpdateSkill2 = false;
    }

    private void Update()
    {
        // Ejecutar habilidad 1 si se presiona la tecla correspondiente
        if (Input.GetKeyDown(KeyCode.Alpha1) && !canUpdateSkill1)
        {
            skill1.ExecuteSkill(SkillNumber.SKILL1);
        }

        // Ejecutar habilidad 2 si se presiona la tecla correspondiente
        if (Input.GetKeyDown(KeyCode.Alpha2)&& !canUpdateSkill2)
        {
            skill2.ExecuteSkill(SkillNumber.SKILL2);
        }

        // Actualizar las habilidades (para manejar cooldowns)
        if (canUpdateSkill1)
            skill1.UpdateSkill();
        if (canUpdateSkill2)
            skill2.UpdateSkill();
    }

    // Método para asignar una habilidad a una ranura del jugador
    public void AssignSkillToSlot(SkillBase skill, int slot)
    {
        if (slot == 1)
        {
            skill1 = skill;
        }
        else if (slot == 2)
        {
            skill2 = skill;
        }
    }
}