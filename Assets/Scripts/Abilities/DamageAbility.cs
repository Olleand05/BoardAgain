using UnityEngine;
using BoardAgain.Characters;


namespace BoardAgain.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/DamageAbility")]
    public class DamageAbility : Ability
    {
        public int baseDamage;
        public float attackMultiplier = 1f;

        public override void ActivateAbility(Character caster, Character target)
        {
            int finalDamage = Mathf.RoundToInt(
                baseDamage + caster.data.attack * attackMultiplier);
            target.TakeDamage(finalDamage);
        }
    }
}