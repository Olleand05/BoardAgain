using BoardAgain.Characters;
using UnityEngine;

namespace BoardAgain.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        [Header("General Info")]
        public string abilityName;
        [TextArea]
        public string abilityDescription;

        [Header("Audio")]
        public AudioClip castSound;

        [Header("Ability Tag (Used as category)")]
        public AbilityTag abilityTag = AbilityTag.None;

        [Header("Unlock Condition")]
        public SynergyRequirement synergyRequirement;

        public bool IsUnlocked(Ability[] equippedAbilities)
        {
            if (synergyRequirement == null)
                return true;

            return synergyRequirement.IsRequirementMet(equippedAbilities);
        }

        public abstract void ActivateAbility(Character caster, Character target);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (abilityTag == 0)
                Debug.LogWarning($"Ability '{abilityName}' has no AbilityTag assigned!", this);
        }
#endif
    }
}