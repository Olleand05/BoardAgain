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

        public TooltipManager tooltipManager;

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
        public TextMeshProUGUI newAbilityDescriptionText;
        public TextMeshProUGUI newAbilitySynergy;
        public List<TextMeshProUGUI> swapSlotButtonTexts;

        private Ability pendingAbility;
        private BattleController battleController;

        public void StartRewardProcess()
        {
            
            Character player = GameManager.Instance.playerCharacter;
            player.attack = player.data.attack;
            player.defense = player.data.defense;
            battleController = GameManager.Instance.battleController;

            if (NodeUI.isBossNext)
            {
                battleController.ShowFinalVictoryScreen();
                return;
            }

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

            TextMeshProUGUI text = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = $"<b>{ability.abilityName}</b>";
            }

            Button button = btn.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    GameManager.Instance.playerCharacter.bonusAbility = ability;
                    Finish();
                });
            }

            var trigger = btn.GetComponent<AbilityTooltipTrigger>();
            if (trigger == null)
                trigger = btn.AddComponent<AbilityTooltipTrigger>();

            trigger.Setup(ability, player, tooltipManager);
        }

        private void CreateSkipButton()
        {
            GameObject btn = Instantiate(buttonPrefab, buttonContainer);

            TextMeshProUGUI text = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = "<color=black><b>SKIP</b></color>\n<size=70%>(Keep slot empty)</size>";
            }

            Button button = btn.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => Finish());
            }
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
            if (tooltipManager != null)
                tooltipManager.HideTooltip();

            swapPanel.SetActive(true);
            newAbilityText.text = $"You found: <b>{newAbility.abilityName}</b>!";

            if (newAbilityDescriptionText != null)
            {
                newAbilityDescriptionText.text = newAbility.abilityDescription;
            }

            if (newAbilitySynergy != null)
            {
                newAbilitySynergy.text = $"Synergy: <b>{newAbility.abilityTag.ToString()}</b>!";
            }

            Character player = GameManager.Instance.playerCharacter;
            for (int i = 0; i < swapSlotButtonTexts.Count; i++)
            {
                if (i < player.equippedAbilities.Length)
                {
                    Ability currentAbility = player.equippedAbilities[i];
                    swapSlotButtonTexts[i].text = currentAbility != null ?
                        $"{currentAbility.abilityName}" : "Fill Empty Slot";

                    Button btn = swapSlotButtonTexts[i].GetComponentInParent<Button>();

                    if (btn != null && currentAbility != null)
                    {
                        var trigger = btn.gameObject.GetComponent<AbilityTooltipTrigger>();
                        if (trigger == null) trigger = btn.gameObject.AddComponent<AbilityTooltipTrigger>();

                        trigger.Setup(currentAbility, player, tooltipManager);
                    }
                }
            }
        }

        public void ReplaceAbility(int slotIndex)
        {
            Character player = GameManager.Instance.playerCharacter;

            if (tooltipManager != null)
                tooltipManager.HideTooltip();

            if (slotIndex >= 0 && slotIndex < player.equippedAbilities.Length)
            {
                player.equippedAbilities[slotIndex] = pendingAbility;
            }

            CloseSwapUI();
        }

        public void CloseSwapUI()
        {
            if (tooltipManager != null)
                tooltipManager.HideTooltip();

            swapPanel.SetActive(false);

            if (battleController != null)
            {
                battleController.UpdateAbilityButtonNames();
            }

            battleController.ShowFinalVictoryScreen();
        }
    }
}