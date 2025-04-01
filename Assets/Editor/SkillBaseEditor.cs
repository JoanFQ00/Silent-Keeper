using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(SkillBase))]
public class SkillBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SkillBase skillBase = (SkillBase)target;
        TextAsset csvFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/CSV/Skills.csv");

        if (csvFile == null)
        {
            EditorGUILayout.HelpBox("CSV file not found! Make sure it's in Assets/CSV/Skills.csv", MessageType.Warning);
            return;
        }

        // Cargar habilidades del CSV solo para esta instancia
        List<string> skillNames = new List<string>();
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // Saltar la cabecera
        {
            string[] values = lines[i].Split(',');
            if (values.Length < 6) continue;
            skillNames.Add(values[0].Trim());
        }

        if (skillNames.Count > 0)
        {
            // Buscar la habilidad seleccionada en la lista, si no está, usar el primer elemento
            int selectedIndex = skillNames.IndexOf(skillBase.selectedSkillName);
            if (selectedIndex == -1) selectedIndex = 0;

            selectedIndex = EditorGUILayout.Popup("Select Skill", selectedIndex, skillNames.ToArray());
            skillBase.selectedSkillName = skillNames[selectedIndex];
        }
        else
        {
            EditorGUILayout.HelpBox("No skills found in CSV!", MessageType.Warning);
        }

        // Guardar cambios en la instancia actual
        if (GUI.changed)
        {
            EditorUtility.SetDirty(skillBase);
        }

        base.OnInspectorGUI();
    }
}