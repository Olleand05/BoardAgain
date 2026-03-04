using UnityEngine;
using UnityEngine.EventSystems;
using BoardAgain.Abilities;
using BoardAgain.Characters;

namespace BoardAgain.Battle
{
    public class AbilityTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Ability ability;
        private Character caster;
        private TooltipManager manager;

        public void Setup(Ability newAbility, Character newCaster, TooltipManager newManager)
        {
            ability = newAbility;
            caster = newCaster;
            manager = newManager;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (ability != null && caster != null)
            {
                // Use the runtime description you already wrote!
                string description = "";
                if (ability is DamageAbility dmg) description = dmg.GetRuntimeDescription(caster);
                else if (ability is HealingAbility heal) description = heal.GetRuntimeDescription(caster);
                else description = ability.abilityDescription;

                manager.ShowTooltip(description, transform.position);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            manager.HideTooltip();
        }
    }
}