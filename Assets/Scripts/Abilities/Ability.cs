using BoardAgain.Characters;
using UnityEngine;

namespace BoardAgain.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        private string abilityName;
        private string abilityDescription;
        private AbilityTag tag;

        public AbilityTag GetAbilityTag => tag;

        public abstract void ActivateAbility(Character caster, Character target);
    }
}
