using BoardAgain.Abilities;
using BoardAgain.Abilities.Synergy;
using UnityEditor.Build.Content;
using UnityEngine;
using BoardAgain.Core;

namespace BoardAgain.Characters
{

    public class Character : MonoBehaviour
    {
        [HideInInspector]
        public string characterName;
        [HideInInspector]
        public int maxHealth;
        [HideInInspector]
        public int currentHealth;
        [HideInInspector]
        public int attack;
        [HideInInspector]
        public int defense;

        public Animator animator;

        public HealthBarSlider healthBarSlider;

        [HideInInspector]
        public Ability[] equippedAbilities;
        [HideInInspector]
        public Ability bonusAbility;

        public int CurrentHealth => currentHealth;

        public CharacterData data;

        void Awake()
        {
            characterName = data.characterName;
            maxHealth = data.maxHealth;
            equippedAbilities = data.abilities;
            attack = data.attack;
            defense = data.defense;
            bonusAbility = data.bonusAbility;

            if (gameObject.CompareTag("Enemy")&&NodeUI.isBossNext)
            {
                //TODO: Implement better boss scaling based on player progress and stats, not just a flat increase
                characterName = "BOSS "+characterName;
                maxHealth += 50;
                attack += 10;

                Debug.Log("Boss Spawned! Health and Attack increased.");
            }
            currentHealth = maxHealth;

            if (healthBarSlider != null)
            {
                healthBarSlider.SetMaxHealth(maxHealth);
            }
        }

        private void Start()
        {   
            if (gameObject.CompareTag("Player"))
                GameManager.Instance.RegisterPlayer(this);
        }
        public void InitializeFrom(Character source)
        {
            if (source == null) return;

            this.data = source.data;

            this.equippedAbilities = source.equippedAbilities;
            this.bonusAbility = source.bonusAbility;

            this.currentHealth = source.currentHealth;
        }

        public void PlayAttackAnimation()
        {
            animator.SetTrigger("MeleeAttack");
        }

        public void PlayDeathAnimation()
        {

         
                animator.SetTrigger("Die");
            
        }

        private void UpdateHealthBar()
        {
            if (healthBarSlider != null)
            {
                healthBarSlider.SetHealth(currentHealth);
            }
        }

        public void TakeDamage(int damageAmount)
        {
            int damageTaken = Mathf.Max(damageAmount - defense, 0);
            currentHealth -= damageTaken;

            currentHealth = Mathf.Max(currentHealth, 0);

            UpdateHealthBar();
            return;
        }

        public void Heal(int healAmount)
        {
            currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
            UpdateHealthBar();
            return;
        }

        public void UseAbility(int index, Character target)
        {
            equippedAbilities[index].ActivateAbility(this, target);
            return;
        }

        public Ability GetMainAbility(int index)
        {
            if (index >= 0 && index < equippedAbilities.Length)
            {
                return equippedAbilities[index];
            }

            return null;   
        }

        public bool IsCharacterDead()
        {
            return currentHealth <= 0;
        }
    }

}