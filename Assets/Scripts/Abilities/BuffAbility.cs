using UnityEngine;
using BoardAgain.Characters;


namespace BoardAgain.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/BuffAbility")]
    public class BuffAbility : Ability
    {

        [Header("Buff Settings")]
        public int statMultiplier = 2;
        public StatType stat = StatType.Attack;

        public enum StatType
        {
            Attack,
            Defense
        }

        public override void ActivateAbility(Character caster, Character target)
        {
            int statChange = statMultiplier;
            caster.ApplyBuff(stat, statChange);
            Debug.Log($"{caster.name} used {name} and changed {stat} by {statChange}! (Old: {caster.attack}, New: {caster.attack*statChange}");
        }

        public string GetRuntimeDescription(Character caster)
        {
            int statChange = statMultiplier;
            return $"{abilityDescription}\nDeals {statChange} damage.";
        }
    }
}
