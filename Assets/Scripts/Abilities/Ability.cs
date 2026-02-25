using BoardAgain.Characters;
using UnityEngine;

namespace BoardAgain.Abilities
{
    public abstract class Ability : ScriptableObject
    {   
        [Header("General Info")]
        public string abilityName;
        public string abilityDescription;
        public AbilityTag tag;

        public AbilityTag GetAbilityTag => tag;

        [Header("Unlock Condition")]
        public SynergyRequirement synergyRequirement;

        public bool IsUnlocked(Ability[] equippedAbilities)
        {
            if (synergyRequirement == null)
                return true;

            return synergyRequirement.IsRequirementMet(equippedAbilities);
        }

        public abstract void ActivateAbility(Character caster, Character target);
    }
}
