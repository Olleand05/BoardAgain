#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;
using BoardAgain.Abilities;

[CustomEditor(typeof(CharacterData))]
public class CharacterDataEditor : Editor
{
    private AbilityTag[] tagSelections;

    private void OnEnable()
    {
        CharacterData characterData = (CharacterData)target;

        if (tagSelections == null || tagSelections.Length != characterData.abilities.Length)
        {
            tagSelections = new AbilityTag[characterData.abilities.Length];

            for (int i = 0; i < characterData.abilities.Length; i++)
            {
                Ability ab = characterData.abilities[i];
                tagSelections[i] = (ab != null) ? ab.abilityTag : AbilityTag.None;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CharacterData characterData = (CharacterData)target;

        SerializedProperty prop = serializedObject.GetIterator();
        bool enterChildren = true;

        while (prop.NextVisible(enterChildren))
        {
            enterChildren = false;

            if (prop.name == "abilities" || prop.name == "bonusAbility") continue;

            EditorGUILayout.PropertyField(prop, true);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Equipped Abilities", EditorStyles.boldLabel);

        if (characterData.abilityLibrary == null)
        {
            EditorGUILayout.HelpBox("Assign an AbilityLibrary to use filtered ability slots.", MessageType.Warning);
        }
        else
        {
            if (tagSelections == null || tagSelections.Length != characterData.abilities.Length)
                tagSelections = new AbilityTag[characterData.abilities.Length];

            SerializedProperty abilitiesProp = serializedObject.FindProperty("abilities");

            for (int i = 0; i < abilitiesProp.arraySize; i++)
            {
                EditorGUILayout.BeginVertical("box");

                
                tagSelections[i] = (AbilityTag)EditorGUILayout.EnumPopup("Tag Filter", tagSelections[i]);

                Ability currentAbility = abilitiesProp.GetArrayElementAtIndex(i).objectReferenceValue as Ability;

                if (currentAbility != null && currentAbility.abilityTag != tagSelections[i])
                    abilitiesProp.GetArrayElementAtIndex(i).objectReferenceValue = null;

                Ability[] filteredAbilities = characterData.abilityLibrary.allAbilities
                    .Where(a => a.abilityTag == tagSelections[i])
                    .ToArray();         

                string[] abilityNames = new string[filteredAbilities.Length + 1];
                abilityNames[0] = "None";

                for (int j = 0; j < filteredAbilities.Length; j++)
                    abilityNames[j + 1] = filteredAbilities[j].name;



                int currentIndex = System.Array.IndexOf(filteredAbilities, currentAbility);

                int offsetIndex = currentIndex + 1; 

                int newIndex = EditorGUILayout.Popup("Ability", offsetIndex, abilityNames);

                if (newIndex == 0)
                {
                    abilitiesProp.GetArrayElementAtIndex(i).objectReferenceValue = null;
                }
                else
                {
                    abilitiesProp.GetArrayElementAtIndex(i).objectReferenceValue = filteredAbilities[newIndex - 1];
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            EditorGUILayout.LabelField("Bonus Ability (Synergy)", EditorStyles.boldLabel);
            Ability[] synergyAbilities = characterData.abilityLibrary.allAbilities
                .Where(a => a.abilityTag == AbilityTag.Synergy)
                .ToArray();

            string[] synergyNames = new string[synergyAbilities.Length + 1];
            synergyNames[0] = "None";

            for (int j = 0; j < synergyAbilities.Length; j++)
                synergyNames[j + 1] = synergyAbilities[j].name;

            int bonusIndex = System.Array.IndexOf(synergyAbilities, characterData.bonusAbility);

            int offsetBonusIndex = bonusIndex + 1; 

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
            EditorUtility.SetDirty(target);
        
    }
}
#endif