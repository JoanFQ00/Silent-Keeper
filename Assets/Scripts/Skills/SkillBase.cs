using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Element/Skill")]
public class SkillBase : ScriptableObject
{
    private enum SkillType
    {
        ACTIVE = 0,
        PASSIVE
    }

    private enum ReloadType
    {
        ROOM = 0,
        ZONE
    }

    [HideInInspector]
    public string selectedSkillName;

    private SkillType currentSkillType;
    private ReloadType currentReloadType;
    private string skillDescription;
    private uint uses;
    private float cooldown;

    private ISkill selectedSkill; // Referencia a la habilidad que se ha seleccionado

    // Cargar la habilidad en función del nombre seleccionado
    private void LoadSkill()
    {
        switch (selectedSkillName)
        {
            case "Ballooned":
                // selectedSkill = new BalloonedSkill();
                break;

            case "InumakyScream":
                // selectedSkill = new InumakyScreamSkill();
                break;

            // Añadir más habilidades aquí si es necesario
            default:
                Debug.LogWarning("Habilidad no encontrada: " + selectedSkillName);
                break;
        }
    }

    // Ejecutar la habilidad seleccionada
    public void ExecuteSkill()
    {
        selectedSkill?.Execute();  // Si la habilidad no es nula, ejecutarla
    }

    // Actualizar el estado de la habilidad (como el cooldown)
    public void UpdateSkill()
    {
        selectedSkill?.UpdateSkill();
    }

    // Cargar habilidades desde el archivo CSV
    public void LoadFromCSV(TextAsset csvFile)
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV file not assigned!");
            return;
        }

        string[] lines = csvFile.text.Split('\n');
        for (int i = 1; i < lines.Length; i++) // Saltar la cabecera
        {
            string[] values = lines[i].Split(',');
            if (values.Length < 6) continue;

            // Si el nombre coincide con el seleccionado, cargar la habilidad
            if (values[0].Trim() == selectedSkillName)
            {
                currentSkillType = values[2].Trim().ToUpper() == "ACTIVE" ? SkillType.ACTIVE : SkillType.PASSIVE;
                skillDescription = values[1].Trim();
                uses = uint.TryParse(values[3], out uint parsedUses) ? parsedUses : 0;
                cooldown = float.TryParse(values[4], out float parsedCooldown) ? parsedCooldown : 0;
                currentReloadType = values[5].Trim().ToUpper() == "ROOM" ? ReloadType.ROOM : ReloadType.ZONE;
                LoadSkill();
            }
        }
    }
}