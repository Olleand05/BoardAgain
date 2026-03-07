using UnityEngine;
using BoardAgain.Characters;

namespace BoardAgain.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/DamageAbility")]
    public class DamageAbility : Ability
    {

        [Header("Damage Settings")]
        public int baseDamage = 10;
        public float attackMultiplier = 1f;

        [Header("Randomness")]
        public int randomVariance = 3;

        public override void ActivateAbility(Character caster, Character target)
        {
            int finalDamage = CalculateDamage(caster);
            target.TakeDamage(finalDamage);

            Debug.Log($"{caster.name} used {name} on {target.name} for {finalDamage} damage!");
        }

        private int CalculateDamage(Character caster)
        {
            int baseValue = Mathf.RoundToInt(baseDamage + caster.attack * attackMultiplier);
            int randomOffset = Random.Range(-randomVariance, randomVariance + 1);
            return baseValue + randomOffset;
        }

        public string GetRuntimeDescription(Character caster)
        {
            return $"{abilityDescription}\nBase damage: {baseDamage+attackMultiplier*caster.attack}.";
        }
    }
}