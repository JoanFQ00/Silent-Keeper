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

    public Sprite bigImage, gameplayImage, gameplayImageBW, gameplayImageWhite; // Will be used in SkillManager or in the vending machine to set up the observer with the UI | No need to put it in the update ;)

    [HideInInspector]
    public string selectedSkillName; // Will be used in SkillManager or in the vending machine to set up the observer with the UI | No need to put it in the update ;)

    public SkillType currentSkillType { get; private set; } // Will be used in the vending machine when assigning which stack it goes to using the SkillManager function "AssignSkillToSlot"
    public ReloadType currentReloadType { get; private set; } // An external observer must be applied and it should affect the SkillManager conditions (related to "uses") (e.g., when crossing doors, therefore two types: room doors (single use) and the final zone door)
    public string skillDescription { get; private set; } // Will be used in SkillManager (gameplayImage) and/or in the vending machine (bigImage) to set up the observer with the UI | No need to put it in the update ;)
    public uint uses { get; private set; } // Used in SkillManager (if used up, make them transparent until reloaded)
    public float cooldown { get; private set; } // Used in SkillManager to reload the ability

    private const string CSV_PATH = "CSV/Skills";  // Path to the Resources folder

    // Method to load data from the CSV
    public void LoadFromCSV(TextAsset csvFile)
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV file not assigned!");
            return;
        }

        string[] lines = csvFile.text.Split('\n');
        for (int i = 1; i < lines.Length; i++) // Skip the header
        {
            string[] values = lines[i].Split(',');
            if (values.Length < 6) continue;

            // If the name matches the selected one, load the skill
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

    // This method will execute when the ScriptableObject loads (when the project restarts or the build is run)
    public void Awake()
    {
        // If a selectedSkillName already exists, load from CSV
        TextAsset csvFile = Resources.Load<TextAsset>(CSV_PATH);  // Load from Resources
        if (!string.IsNullOrEmpty(selectedSkillName))
        {
            LoadFromCSV(csvFile);
        }
    }
}