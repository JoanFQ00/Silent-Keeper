using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(SkillBase))]
public class SkillBaseEditor : Editor
{
    private static TextAsset csvFile;

    private void OnEnable()
    {
        LoadCSV();
    }

    public override void OnInspectorGUI()
    {
        SkillBase skillBase = (SkillBase)target;

        if (csvFile == null)
        {
            EditorGUILayout.HelpBox("CSV file not found! Make sure it's in Resources/CSV/Skills.csv", MessageType.Warning);
            return;
        }

        List<string> skillNames = GetSkillNames();
        if (skillNames.Count > 0)
        {
            int selectedIndex = skillNames.IndexOf(skillBase.selectedSkillName);
            if (selectedIndex == -1) selectedIndex = 0;

            selectedIndex = EditorGUILayout.Popup("Select Skill", selectedIndex, skillNames.ToArray());
            string newSelectedSkillName = skillNames[selectedIndex];

            if (newSelectedSkillName != skillBase.selectedSkillName)
            {
                skillBase.selectedSkillName = newSelectedSkillName;
                skillBase.LoadFromCSV(csvFile);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No skills found in CSV!", MessageType.Warning);
        }

        if (GUILayout.Button("Refresh CSV"))
        {
            RefreshAllSkillBases();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(skillBase);
        }

        base.OnInspectorGUI();
    }

    private static void LoadCSV()
    {
        csvFile = Resources.Load<TextAsset>("CSV/Skills");
    }

    private static List<string> GetSkillNames()
    {
        List<string> skillNames = new List<string>();
        if (csvFile == null) return skillNames;

        string[] lines = csvFile.text.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            if (values.Length < 6) continue;
            skillNames.Add(values[0].Trim());
        }
        return skillNames;
    }

    private static void RefreshAllSkillBases()
    {
        LoadCSV();
        if (csvFile == null)
        {
            Debug.LogError("CSV file not found in Resources/CSV!");
            return;
        }

        string[] skillBaseGuids = AssetDatabase.FindAssets("t:SkillBase");
        foreach (string guid in skillBaseGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            SkillBase skillBase = AssetDatabase.LoadAssetAtPath<SkillBase>(path);
            if (skillBase != null)
            {
                skillBase.LoadFromCSV(csvFile);
                EditorUtility.SetDirty(skillBase);
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("All SkillBase instances updated from CSV.");
    }
}