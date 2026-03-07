using UnityEngine;
using BoardAgain.Characters;

namespace BoardAgain.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/BlockAbility")]
    public class BlockAbility : Ability
    {
        [Header("Block Settings")]
        public int baseShield = 10;

        public override void ActivateAbility(Character caster, Character target)
        {

            int amount = Mathf.RoundToInt(baseShield + caster.defense);
            caster.GainShield(amount);

            Debug.Log($"{caster.name} used {name} and gained {amount} shield! (Total shield: {caster.shield})");
        }

        public string GetRuntimeDescription(Character caster)
        {
            int amount = Mathf.RoundToInt(baseShield + caster.defense);
            return $"{abilityDescription}\nGain {amount} shield.\nSynergy: {abilityTag}";
        }
    }
}
