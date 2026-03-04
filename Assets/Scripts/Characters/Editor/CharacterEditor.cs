#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;
using BoardAgain.Abilities;

[CustomEditor(typeof(CharacterData))]
public class CharacterDataEditor : Editor
{
    private AbilityTag[] tagSelections;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CharacterData characterData = (CharacterData)target;

        // Draw all serialized fields except "abilities"
        SerializedProperty prop = serializedObject.GetIterator();
        bool enterChildren = true;

        while (prop.NextVisible(enterChildren))
        {
            enterChildren = false;

            // Skip abilities, handle them manually
            if (prop.name == "abilities" || prop.name == "bonusAbility") continue;

            // Draw AbilityLibrary explicitly if needed
            EditorGUILayout.PropertyField(prop, true);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Equipped Abilities", EditorStyles.boldLabel);

        // Draw a warning if AbilityLibrary is missing
        if (characterData.abilityLibrary == null)
        {
            EditorGUILayout.HelpBox("Assign an AbilityLibrary to use filtered ability slots.", MessageType.Warning);
        }
        else
        {
            // --- Main Abilities ---
            if (tagSelections == null || tagSelections.Length != characterData.abilities.Length)
                tagSelections = new AbilityTag[characterData.abilities.Length];

            SerializedProperty abilitiesProp = serializedObject.FindProperty("abilities");
            SerializedProperty tagsProp = serializedObject.FindProperty("abilityTags");

            while (tagsProp.arraySize < abilitiesProp.arraySize)
                tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);

            // Draw each ability slot
            for (int i = 0; i < abilitiesProp.arraySize; i++)
            {
                EditorGUILayout.BeginVertical("box");

                SerializedProperty tagProp = tagsProp.GetArrayElementAtIndex(i);
                SerializedProperty abilityProp = abilitiesProp.GetArrayElementAtIndex(i);

                // Tag selector
                EditorGUILayout.PropertyField(tagProp, new GUIContent("Tag Filter"));
                AbilityTag selectedTag = (AbilityTag)tagProp.enumValueIndex;

                Ability currentAbility = abilityProp.objectReferenceValue as Ability;

                if (currentAbility != null && currentAbility.abilityTag != selectedTag)
                {
                    abilityProp.objectReferenceValue = null;
                }

                // Filter from library
                Ability[] filteredAbilities = characterData.abilityLibrary.allAbilities
                    .Where(a => a.abilityTag == selectedTag)
                    .ToArray();

                string[] abilityNames = new string[filteredAbilities.Length + 1];
                abilityNames[0] = "None";

                for (int j = 0; j < filteredAbilities.Length; j++)
                    abilityNames[j + 1] = filteredAbilities[j].name;


                
                int currentIndex = System.Array.IndexOf(filteredAbilities, currentAbility);

                int offsetIndex = currentIndex + 1; // +1 for "None" option

                int newIndex = EditorGUILayout.Popup("Ability", offsetIndex, abilityNames);
                
                if (newIndex == 0)
                {
                    abilityProp.objectReferenceValue = null;
                }
                else
                {
                    abilityProp.objectReferenceValue = filteredAbilities[newIndex - 1];
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            // --- Bonus Ability ---
            EditorGUILayout.LabelField("Bonus Ability (Synergy)", EditorStyles.boldLabel);
            Ability[] synergyAbilities = characterData.abilityLibrary.allAbilities
                .Where(a => a.abilityTag == AbilityTag.Synergy)
                .ToArray();

            string[] synergyNames = new string[synergyAbilities.Length + 1];
                synergyNames[0] = "None";

                for (int j = 0; j < synergyAbilities.Length; j++)
                    synergyNames[j + 1] = synergyAbilities[j].name;

            int bonusIndex = System.Array.IndexOf(synergyAbilities, characterData.bonusAbility);

            int offsetBonusIndex = bonusIndex + 1; // +1 for "None" option

            int newBonusIndex = EditorGUILayout.Popup("Bonus Ability", offsetBonusIndex, synergyNames);

            if (newBonusIndex == 0)
            {
                characterData.bonusAbility = null;
            }
            else
            {
                characterData.bonusAbility = synergyAbilities[newBonusIndex - 1];
            }
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
        EditorUtility.SetDirty(target);
        }
    }
}
#endif