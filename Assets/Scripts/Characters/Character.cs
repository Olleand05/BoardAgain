using BoardAgain.Abilities;
using BoardAgain.Abilities.Synergy;
using UnityEngine;

namespace BoardAgain.Characters
{

    public abstract class Character : MonoBehaviour
    {
        private string characterName;
        private int maxHealth;
        private int currentHealth;
        private int attack;
        private int defense;

        private Ability[] equippedAbilities;
        private SynergyRule[] synergyRules;

        public Ability ActiveSynergyAbility { get; private set; }
        private AbilityTag? activeSynergyTag = null;

        public CharacterData data;

        void Awake()
        {
            characterName = data.characterName;
            maxHealth = data.maxHealth;
            equippedAbilities = data.abilites;
            attack = data.attack;
            defense = data.defense;
            currentHealth = maxHealth;
        }

        public void CheckSynergy()
        {
            foreach (var rule in synergyRules)
            {
                int count = CountAbilitesWithTag(rule.requiredTag);

                bool isNewSynergy =
                    count >= rule.requiredCount &&
                    activeSynergyTag != rule.requiredTag;

                if (isNewSynergy)
                {
                    ActiveSynergyAbility = rule.unlockedAbility;
                    activeSynergyTag = rule.requiredTag;
                    return;
                }
            }
        }

        public int CountAbilitesWithTag(AbilityTag tag)
        {
            int count = 0;
            foreach (var ability in equippedAbilities)
            {
                if (ability.GetAbilityTag == tag)
                    count++;
            }
            return count;
        }


        public void TakeDamage(int damageAmount)
        {
            int damageTaken = Mathf.Max(damageAmount - defense, 0);
            currentHealth -= damageTaken;
            return;
        }

        public void Heal(int healAmount)
        {
            currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
            return;
        }

        public void UseAbility(int index, Character target)
        {
            equippedAbilities[index].ActivateAbility(this, target);
            return;
        }

        public Ability GetAbility(int index)
        {
            if (index < equippedAbilities.Length)
                return equippedAbilities[index];
            if (index == equippedAbilities.Length)
                return ActiveSynergyAbility;

            return null;
        }
    }

}