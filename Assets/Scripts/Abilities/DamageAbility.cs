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

        public override void ActivateAbility(Character caster, Character target)
        {
            int finalDamage = CalculateDamage(caster);
            target.TakeDamage(finalDamage);
            Debug.Log($"{caster.name} used {name} on {target.name} for {finalDamage} damage! (Base: {baseDamage}, Attack: {caster.attack}, Multiplier: {attackMultiplier})");
        }

        private int CalculateDamage(Character caster)
        {
            return Mathf.RoundToInt(baseDamage + caster.attack * attackMultiplier);
        }

        public string GetRuntimeDescription(Character caster)
        {
            int finalDamage = CalculateDamage(caster);
            return $"{abilityDescription}\nDeals {finalDamage} damage.";
        }
    }
}