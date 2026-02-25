using UnityEngine;

namespace BoardAgain.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Synergy Requirement")]
    public class SynergyRequirement : ScriptableObject
    {
        public AbilityTag requiredTag;

        [Min(1)]
        public int requiredCount = 4;

        public bool IsRequirementMet(Ability[] equippedAbilities)
        {
            int count = 0;
            foreach (var ability in equippedAbilities)
            {
                if (ability.abilityTag == requiredTag)
                {
                    count++;
                }
            }
            return count >= requiredCount;
        }

        #if UNITY_EDITOR
                private void OnValidate()
                {
                    if (requiredCount <= 0)
                        requiredCount = 4;
                }
        #endif
    }
}