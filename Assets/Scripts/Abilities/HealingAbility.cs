using UnityEngine;
using BoardAgain.Characters;

namespace BoardAgain.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/HealingAbility")]
    public class HealingAbility : Ability
    {
        [Header("Healing Settings")]
        public int minHealing = 10;
        public int maxHealing = 50;

        public override void ActivateAbility(Character caster, Character target)
        {
            int finalHealing = CalculateHealing();
            caster.Heal(finalHealing);
            Debug.Log($"{caster.name} healed for {finalHealing}!");
        }

        private int CalculateHealing()
        {
            return Random.Range(minHealing, maxHealing + 1);
        }

        public string GetRuntimeDescription(Character caster)
        {
            return $"{abilityDescription}\nHeals for {minHealing} - {maxHealing}.";
        }
    }
}
