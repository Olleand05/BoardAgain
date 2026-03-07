using BoardAgain.Abilities;
using BoardAgain.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BoardAgain.Core;

namespace BoardAgain.Battle
{
    public class BattleController : MonoBehaviour
    {
        [HideInInspector]
        public Character player;

        [SerializeField] private Character playerVisual; // This will hold the instantiated player character for visual purposes
        private Character playerVisualInstance;

        public Character enemy;
        public TooltipManager tooltipManager;
        public GameObject victoryPopUp;
        public GameObject defeatPopUp;
        public BattleRewardManager rewardManager;

        [Header("Audio")]
        public AudioSource sfxSource;

        [Header("UI Elements")]
        // This list should now contain 4 TextMeshProUGUI elements in the Inspector
        public List<TextMeshProUGUI> abilityButtonTexts;
        public TextMeshProUGUI bonusAbilityButtonText;

        [Header("Battle Log")]
        public GameObject logTextPrefab;
        public Transform logContentParent;

        public bool isPlayerTurn = true;
        private bool hasUsedBonusAbilityThisTurn = false;

        private void Awake()
        {

            player = GameManager.Instance.playerCharacter;
            //Reset player stats to base values
            player.attack=player.data.attack;
            player.defense = player.data.defense;

            if (playerVisual != null)
            {
                playerVisualInstance = Instantiate(playerVisual, transform);
                playerVisualInstance.InitializeFrom(player);
            }
        }

        void Start()
        {
            GameManager.Instance.RegisterBattle(this);


            UpdateAbilityButtonNames();
            LogMessage("<color=green>Your turn!</color>");

        }

        public void PlayerUseAbility(int index)
        {
            if (!isPlayerTurn || player == null || enemy == null) return;

            Ability ability = player.GetMainAbility(index);

            if (ability == null) return;

            if (sfxSource != null && ability.castSound != null)
            {
                sfxSource.PlayOneShot(ability.castSound);
            }


            LogMessage($"Player used <b>{ability.abilityName}</b>!");
            player.PlayAttackAnimation();
            ability.ActivateAbility(player, enemy);

            if (enemy.IsCharacterDead())
            {
                HandleVictory();
                return;
            }
            EndPlayerTurn();
        }

        public void PlayerUseBonusAbility()
        {
            if (!isPlayerTurn || player == null || enemy == null) return;

            Ability ability = player.bonusAbility;

            if (ability == null) return;

            if (hasUsedBonusAbilityThisTurn)
            {
                LogMessage("<color=yellow>You already used your bonus ability this turn!</color>");
                return;
            }

            LogMessage($"Player used <b>{ability.abilityName}</b>!");
            player.PlayAttackAnimation();
            ability.ActivateAbility(player, enemy);

            if (enemy.IsCharacterDead())
            {
                HandleVictory();
                return;
            }

            hasUsedBonusAbilityThisTurn = true;
            LogMessage("Bonus action complete. You can still use a regular ability.");
        }

        public void LogMessage(string message)
        {
            if (logTextPrefab == null || logContentParent == null) return;

            

            GameObject newLog = Instantiate(logTextPrefab, logContentParent);
            newLog.transform.localScale = Vector3.one;
            TextMeshProUGUI text = newLog.GetComponent<TextMeshProUGUI>();
            text.text = message;
            text.color = Color.black;

            text.outlineWidth = 0.3f;
            text.outlineColor = Color.black;

            Canvas.ForceUpdateCanvases();
            StartCoroutine(ScrollToBottom());
        }

        IEnumerator ScrollToBottom()
        {
            yield return new WaitForEndOfFrame();
            ScrollRect scrollRect = logContentParent.GetComponentInParent<ScrollRect>();
            if (scrollRect != null)
            {
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }

        public void UpdateAbilityButtonNames()
        {
            if (player == null) return;

            for (int i = 0; i < abilityButtonTexts.Count; i++)
            {
                if (abilityButtonTexts[i] == null) continue;

                UnityEngine.UI.Button btn = abilityButtonTexts[i].GetComponentInParent<UnityEngine.UI.Button>();

                if (i < player.equippedAbilities.Length)
                {
                    Ability ab = player.GetMainAbility(i);
                    abilityButtonTexts[i].text = (ab != null) ? ab.abilityName : "Empty Slot";

                    ConfigureAbilityButton(abilityButtonTexts[i], ab, btn);
                }
                else
                {
                    abilityButtonTexts[i].text = "---";
                    if (btn != null) btn.interactable = false;
                }
            }

            if (bonusAbilityButtonText != null)
            {
                UnityEngine.UI.Button bonusBtn = bonusAbilityButtonText.GetComponentInParent<UnityEngine.UI.Button>();
                Ability bonusAb = player.bonusAbility;

                if (bonusAb != null)
                {
                    bonusAbilityButtonText.text = bonusAb.abilityName;
                    ConfigureAbilityButton(bonusAbilityButtonText, bonusAb, bonusBtn);
                }
                else
                {
                    bonusAbilityButtonText.text = "Locked";
                    if (bonusBtn != null) bonusBtn.interactable = false;
                }
            }
        }

        private void ConfigureAbilityButton(TMPro.TextMeshProUGUI textElement, Ability ability, UnityEngine.UI.Button btn)
        {
            if (btn == null) return;

            if (ability == null)
            {
                btn.interactable = false;
                return;
            }

            btn.interactable = true;

            var trigger = btn.GetComponent<AbilityTooltipTrigger>();
            if (trigger == null) trigger = btn.gameObject.AddComponent<AbilityTooltipTrigger>();

            trigger.Setup(ability, player, tooltipManager);
        }

        void EndPlayerTurn()
        {
            isPlayerTurn = false;
            LogMessage("<color=red>Enemy turn!</color>");

            StartCoroutine(EnemyTurnRoutine());
        }

        IEnumerator EnemyTurnRoutine()
        {
            yield return new WaitForSeconds(1.5f);

            Ability enemyAbility = enemy.GetMainAbility(0);

            if (enemyAbility == null) yield break;
            
            LogMessage($"Enemy used <b>{enemyAbility.abilityName}</b>!");
                
        
            // Play Enemy Sound
            if (sfxSource != null && enemyAbility.castSound != null)
            {
                sfxSource.PlayOneShot(enemyAbility.castSound);
            }
                enemy.PlayAttackAnimation();
                enemyAbility.ActivateAbility(enemy, player);

                if (player.IsCharacterDead())
                {
                    HandleDefeat();
                }
                else
                {
                    StartPlayerTurn();
                }
            
        }

        void StartPlayerTurn()
        {
            hasUsedBonusAbilityThisTurn = false;
            isPlayerTurn = true;
            LogMessage("<color=green>Your turn!</color>");
            UpdateAbilityButtonNames();
        }

        // Change HandleVictory to call the routine
        void HandleVictory()
        {
            isPlayerTurn = false;
            LogMessage("<b>VICTORY!</b>");
            StartCoroutine(VictoryRoutine());
        }

        IEnumerator VictoryRoutine()
        {
            enemy.HandleDeath();
            yield return new WaitForSeconds(2.0f);

            // Stop here to let the player choose a reward
            if (rewardManager != null)
            {
                rewardManager.StartRewardProcess();
            }
            else
            {
                ShowFinalVictoryScreen();
            }
        }

        // Move your map/popup logic into this separate function
        public void ShowFinalVictoryScreen()
        {
            MapManager.currentNodeIndex++;

            if (NodeUI.isBossNext)
            {
                MapManager.currentNodeIndex = 0;
                UnityEngine.SceneManagement.SceneManager.LoadScene("VictoryScreen");
            }

            victoryPopUp.SetActive(true);
            // Note: Do not use Time.timeScale = 0 if you want UI buttons to work
        }

        void HandleDefeat()
        {
            isPlayerTurn = false;
            LogMessage("<color=red>DEFEAT...</color> You have fallen in battle.");

            StartCoroutine(DefeatRoutine());
        }

        IEnumerator DefeatRoutine()
        {
            player.HandleDeath();

            yield return new WaitForSeconds(2.0f);

            defeatPopUp.SetActive(true);
        }
    }
}