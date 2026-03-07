using BoardAgain.Abilities;
using UnityEngine;
using BoardAgain.Characters;

namespace BoardAgain.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/HealingAbility")]
    public class HealingAbility : Ability
    {

        [Header("Healing Settings")]
        public int baseHealing;

        [Tooltip("Multiplier applied to the caster's current health to calculate additional healing.")]
        public float healMultiplier;

        public override void ActivateAbility(Character caster, Character target)
        {
            int finalHealing = CalculateHealing(caster);
            caster.Heal(finalHealing);
            Debug.Log($"{caster.name} used {name} on {target.name} for {finalHealing} damage! (Base: {baseHealing}, Attack: {caster.data.attack}, Multiplier: {healMultiplier})");
        }

        private int CalculateHealing(Character caster)
        {
            return Mathf.RoundToInt(baseHealing + caster.CurrentHealth * healMultiplier);
        }

        public string GetRuntimeDescription(Character caster)
        {
            int finalHealing = CalculateHealing(caster);
            return $"{abilityDescription}\nRestores {finalHealing} damage.";
        }
    }
}
