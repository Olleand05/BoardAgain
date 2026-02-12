using UnityEngine;
using BoardAgain.Characters;

namespace BoardAgain.Abilites.Examples
{

    public class ExampleAbility1 : Abilities.Ability
    {
        public int damageAmount = 20;
        public override void ActivateAbility(Character caster, Character target)
        {
            Debug.Log("Example Ability 1 Activated!");
        }
    }

}