using BoardAgain.Abilities;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ability Library")]
public class AbilityLibrary : ScriptableObject
{
    public Ability[] allAbilities;

    public Ability[] GetAbilitiesByTag(AbilityTag tag)
    {
        return allAbilities.Where(a => a.abilityTag == tag).ToArray();
    }

    public Ability GetAbilityByName(string name)
    {
        return allAbilities.FirstOrDefault(a => a.abilityName == name);
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(AbilityLibrary))]
    public class AbilityLibraryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AbilityLibrary lib = (AbilityLibrary)target;

            if (GUILayout.Button("Add Missing Abilities"))
            {
                lib.AddMissingAbilities();
            }
        }
    }

    public void AddMissingAbilities()
    {
        string[] guids = AssetDatabase.FindAssets("t:Ability");
        bool dirty = false;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Ability ability = AssetDatabase.LoadAssetAtPath<Ability>(path);

            if (!allAbilities.Contains(ability))
            {
                ArrayUtility.Add(ref allAbilities, ability);
                dirty = true;
                Debug.Log($"Added missing ability '{ability.name}' to library '{name}'");
            }
        }

        if (dirty)
            EditorUtility.SetDirty(this);
    }

    private void OnValidate()
    {
        AddMissingAbilities();
    }
#endif
}
