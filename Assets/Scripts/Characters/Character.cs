using BoardAgain.Abilities;
using BoardAgain.Abilities.Synergy;
using UnityEditor.Build.Content;
using UnityEngine;
using BoardAgain.Core;
using static UnityEngine.Rendering.DebugUI;

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
        [HideInInspector]
        public int shield;

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

        public void HandleDeath()
        {
            healthBarSlider.gameObject.SetActive(false);
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

            //Added for shield mechanic, shield absorbs damage before health

            int absorbed = Mathf.Min(shield, damageTaken);
            shield -= absorbed;

            int remaining = damageTaken - absorbed;

            currentHealth -= remaining;

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

        public void GainShield(int amount)
        {
            shield += Mathf.Max(0, amount);
        }

        public void ApplyBuff(BuffAbility.StatType stat, int multiplier)
        {
            switch (stat)
            {
                case BuffAbility.StatType.Attack:
                    attack *= multiplier;
                    break;

                case BuffAbility.StatType.Defense:
                    defense *= multiplier;
                    break;
            }
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