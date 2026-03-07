using BoardAgain.Abilities;
using BoardAgain.Characters;
using BoardAgain.Core;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        public Ability[] allStandardAbilities;

        [Header("Swap UI References")]
        public GameObject swapPanel;
        public TextMeshProUGUI newAbilityText;
        public List<TextMeshProUGUI> swapSlotButtonTexts;

        private Ability pendingAbility;
        private BattleController battleController;

        public void StartRewardProcess()
        {
            Character player = GameManager.Instance.playerCharacter;
            battleController = GameManager.Instance.battleController;

            if (player.bonusAbility != null)
            {
                StartRandomSwapProcess();
                return;
            }

            rewardPanel.SetActive(true);
            foreach (Transform child in buttonContainer) Destroy(child.gameObject);

            Ability[] activeAbilities = player.equippedAbilities.Where(a => a != null).ToArray();

            List<Ability> available = new List<Ability>();
            foreach (Ability ab in allPotentialBonusAbilities)
            {
                if (ab != null && ab.IsUnlocked(activeAbilities))
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
                GameManager.Instance.playerCharacter.bonusAbility = ability;
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

            if (battleController != null)
            {
                battleController.UpdateAbilityButtonNames();
            }

            StartRandomSwapProcess();
        }

        public void StartRandomSwapProcess()
        {
            Character player = GameManager.Instance.playerCharacter;

            HashSet<string> equippedNames = new HashSet<string>(
                player.equippedAbilities.Where(a => a != null).Select(a => a.abilityName)
            );

            List<Ability> swapPool = allStandardAbilities
                .Where(ab => ab != null && !equippedNames.Contains(ab.abilityName))
                .ToList();

            if (swapPool.Count == 0)
            {
                battleController.ShowFinalVictoryScreen();
                return;
            }

            pendingAbility = swapPool[Random.Range(0, swapPool.Count)];
            OpenSwapUI(pendingAbility);
        }

        public void OpenSwapUI(Ability newAbility)
        {
            swapPanel.SetActive(true);
            newAbilityText.text = $"You found: <b>{newAbility.abilityName}</b>!";

            Character player = GameManager.Instance.playerCharacter;
            for (int i = 0; i < swapSlotButtonTexts.Count; i++)
            {
                if (i < player.equippedAbilities.Length)
                {
                    Ability currentAbility = player.equippedAbilities[i];
                    swapSlotButtonTexts[i].text = currentAbility != null ?
                        $"Replace {currentAbility.abilityName}" : "Fill Empty Slot";
                }
            }
        }

        public void ReplaceAbility(int slotIndex)
        {
            Character player = GameManager.Instance.playerCharacter;

            if (slotIndex >= 0 && slotIndex < player.equippedAbilities.Length)
            {
                player.equippedAbilities[slotIndex] = pendingAbility;
            }

            CloseSwapUI();
        }

        public void CloseSwapUI()
        {
            swapPanel.SetActive(false);

            if (battleController != null)
            {
                battleController.UpdateAbilityButtonNames();
            }

            battleController.ShowFinalVictoryScreen();
        }
    }
}