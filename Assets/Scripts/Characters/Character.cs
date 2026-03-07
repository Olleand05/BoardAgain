using BoardAgain.Abilities;
using BoardAgain.Abilities.Synergy;
using UnityEngine;
using BoardAgain.Core;
using System;

namespace BoardAgain.Characters
{
    public class Character : MonoBehaviour
    {
        [Header("Identity & Stats")]
        [HideInInspector] public string characterName;
        [HideInInspector] public int maxHealth;
        [HideInInspector] public int currentHealth;
        [HideInInspector] public int attack;
        [HideInInspector] public int defense;
        [HideInInspector] public int shield;

        [Header("Data Sheets")]
        [Tooltip("Standard stats for this character.")]
        public CharacterData data;
        [Tooltip("Stats used only if this character spawns as a Boss.")]
        public CharacterData bossData;

        [Header("References")]
        public GameObject plateArmor;
        public Animator animator;
        public HealthBarSlider healthBarSlider;

        [HideInInspector] public Ability[] equippedAbilities;
        [HideInInspector] public Ability bonusAbility;

        public int CurrentHealth => currentHealth;


        [HideInInspector] public bool needReset;

        void Awake()
        {
            InitializeStats();
        }

        private void InitializeStats()
        {

            CharacterData activeData = data;

            if (gameObject.CompareTag("Enemy") && NodeUI.isBossNext)
            {
                if (bossData != null)
                {
                    activeData = bossData;
                    if (plateArmor != null) plateArmor.SetActive(true);
                }
                else
                {
                    Debug.LogWarning($"{gameObject.name} flagged as Boss but no BossData assigned! Using standard data.");
                }
            }

            if (activeData != null)
            {
                characterName = activeData.characterName;
                maxHealth = activeData.maxHealth;
                attack = activeData.attack;
                defense = activeData.defense;
                equippedAbilities = activeData.abilities;
                bonusAbility = activeData.bonusAbility;
            }

      
            currentHealth = maxHealth;

         
            if (healthBarSlider != null)
            {
                healthBarSlider.SetMaxHealth(maxHealth);
                healthBarSlider.SetHealth(currentHealth);
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

        public void TakeDamage(int damageAmount)
        {
            int damageTaken = Mathf.Max(damageAmount - defense, 0);
            int absorbed = Mathf.Min(shield, damageTaken);
            shield -= absorbed;

            int remaining = damageTaken - absorbed;
            currentHealth -= remaining;
            currentHealth = Mathf.Max(currentHealth, 0);

            UpdateHealthBar();
        }

        public void Heal(int healAmount)
        {
            currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            if (healthBarSlider != null)
            {
                healthBarSlider.SetHealth(currentHealth);
            }
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
            if (index >= 0 && index < equippedAbilities.Length && equippedAbilities[index] != null)
            {
                equippedAbilities[index].ActivateAbility(this, target);
            }
        }

        public Ability GetMainAbility(int index)
        {
            if (index >= 0 && index < equippedAbilities.Length)
                return equippedAbilities[index];
            return null;
        }

        public bool IsCharacterDead() => currentHealth <= 0;

        public void PlayAttackAnimation() => animator.SetTrigger("MeleeAttack");

        public void PlayAnimation(string triggerName)
        {
            if (animator != null) animator.SetTrigger(triggerName);
        }

        public void HandleDeath()
        {
            if (healthBarSlider != null) healthBarSlider.gameObject.SetActive(false);
            animator.SetTrigger("Die");
        }
    }
}