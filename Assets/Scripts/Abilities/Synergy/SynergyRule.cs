using UnityEngine;

namespace BoardAgain.Abilities.Synergy
{
    [CreateAssetMenu(menuName = "Abilities/Synergy Rule")]
    public class SynergyRule : ScriptableObject
    {
        public AbilityTag requiredTag;
        public int requiredCount = 4;

        public Ability unlockedAbility;
    }
}