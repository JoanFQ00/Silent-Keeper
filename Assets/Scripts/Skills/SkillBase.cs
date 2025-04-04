using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Element/Skill")]
public class SkillBase : ScriptableObject
{
    public enum SkillType
    {
        ACTIVE = 0,
        PASSIVE
    }

    public enum ReloadType
    {
        ROOM = 0,
        ZONE
    }

    public Sprite bigImage, gameplayImage; //Se usará en SkillManager o en la máquiana exprendedora para hacer el observer con la UI | No hace falta ponerse en el update ;)

    [HideInInspector]
    public string selectedSkillName; //Se usará en SkillManager o en la máquiana exprendedora para hacer el observer con la UI | No hace falta ponerse en el update ;)

    public SkillType currentSkillType { get; private set; } //Se usará en la máquina exprendedora a la hora de asignar a qué stak va usando la función de SkillManager "AssignSkillToSlot"
    public ReloadType currentReloadType { get; private set; } //Se ha de aplicar un observer externamente y que afecte a las condiciones del SkillManager (va en relación a "uses") (por ejemplo al cruzar purtas, por lo tanto 2 tipos, puertas de sala (de 1 solo uso) y la puerta final de zona)
    public string skillDescription { get; private set; } //Se usará en el SkillManager (gameplayImage) y/o en la máquiana exprendedora (bigImage) para hacer el observer con la UI | No hace falta ponerse en el update ;)
    public uint uses { get; private set; } //Se usa en SkillManager (si se gastan que se queden transparentes hasta que se recarguen)
    public float cooldown { get; private set; } //Se usa en SkillManager para recargar la habilidad

    private const string CSV_PATH = "CSV/Skills";  // Ruta a la carpeta Resources

    // Método para cargar datos desde el CSV
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
            }
        }
    }

    // Este método se ejecutará al cargar el ScriptableObject (cuando se reinicie el proyecto o ejecute la build)
    public void Awake()
    {
        // Si ya se tiene un selectedSkillName, cargar desde el CSV
        TextAsset csvFile = Resources.Load<TextAsset>(CSV_PATH);  // Cargar desde Resources
        if (!string.IsNullOrEmpty(selectedSkillName))
        {
            LoadFromCSV(csvFile);
        }
    }
}