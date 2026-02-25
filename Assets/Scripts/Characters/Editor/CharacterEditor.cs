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

            // Draw each ability slot
            for (int i = 0; i < characterData.abilities.Length; i++)
            {
                EditorGUILayout.BeginVertical("box");

                // Tag filter
                tagSelections[i] = (AbilityTag)EditorGUILayout.EnumPopup("Tag Filter", tagSelections[i]);

                // Filter abilities from the library
                Ability[] filteredAbilities = characterData.abilityLibrary.allAbilities
                    .Where(a => a.abilityTag == tagSelections[i])
                    .ToArray();

                string[] abilityNames = filteredAbilities.Select(a => a.name).ToArray();
                int currentIndex = System.Array.IndexOf(filteredAbilities, characterData.abilities[i]);

                int newIndex = EditorGUILayout.Popup("Ability", currentIndex, abilityNames);
                if (newIndex >= 0 && newIndex < filteredAbilities.Length)
                    characterData.abilities[i] = filteredAbilities[newIndex];

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            // --- Bonus Ability ---
            EditorGUILayout.LabelField("Bonus Ability (Synergy)", EditorStyles.boldLabel);
            Ability[] synergyAbilities = characterData.abilityLibrary.allAbilities
                .Where(a => a.abilityTag == AbilityTag.Synergy)
                .ToArray();

            string[] synergyNames = synergyAbilities.Select(a => a.name).ToArray();
            int bonusIndex = System.Array.IndexOf(synergyAbilities, characterData.bonusAbility);

            int newBonusIndex = EditorGUILayout.Popup("Bonus Ability", bonusIndex, synergyNames);
            if (newBonusIndex >= 0 && newBonusIndex < synergyAbilities.Length)
                characterData.bonusAbility = synergyAbilities[newBonusIndex];
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif