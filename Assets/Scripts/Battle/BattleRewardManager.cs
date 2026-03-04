using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using BoardAgain.Abilities;
using BoardAgain.Characters;
using BoardAgain.Core;

namespace BoardAgain.Battle
{
    public class BattleRewardManager : MonoBehaviour
    {
        [Header("UI References")]
        public GameObject rewardPanel;
        public Transform buttonContainer;
        public GameObject buttonPrefab;

        [Header("Settings")]
        public Ability[] allPotentialBonusAbilities;

        private BattleController battleController;

        public void StartRewardProcess()
        {
            Character player = GameManager.Instance.playerCharacter;
            battleController = GameManager.Instance.battleController;

            if (player.bonusAbility != null)
            {
                battleController.ShowFinalVictoryScreen();
                return;
            }
            rewardPanel.SetActive(true);

            foreach (Transform child in buttonContainer) Destroy(child.gameObject);

            List<Ability> available = new List<Ability>();
            foreach (Ability ab in allPotentialBonusAbilities)
            {
                if (ab.IsUnlocked(player.equippedAbilities))
                {
                    available.Add(ab);
                }
            }

            foreach (Ability ability in available)
            {
                CreateChoiceButton(ability, player);
            }

            CreateSkipButton();
        }

        private void CreateChoiceButton(Ability ability, Character player)
        {
            GameObject btn = Instantiate(buttonPrefab, buttonContainer);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"<b>{ability.abilityName}</b>\n{ability.abilityDescription}";

            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                player.bonusAbility = ability;
                Finish();
            });
        }

        private void CreateSkipButton()
        {
            GameObject btn = Instantiate(buttonPrefab, buttonContainer);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = "<color=yellow>SKIP</color>\n(Keep the slot empty)";

            btn.GetComponent<Button>().onClick.AddListener(() => Finish());
        }

        private void Finish()
        {
            rewardPanel.SetActive(false);

            // Force the BattleController to find the player data AGAIN
            if (battleController != null)
            {
                // Debug here to see if it actually assigned

                battleController.UpdateAbilityButtonNames();
            }

            battleController.ShowFinalVictoryScreen();
        }
    }
}