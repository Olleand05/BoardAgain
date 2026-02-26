using BoardAgain.Abilities;
using BoardAgain.Characters;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    public Character player;
    public Character enemy;

    public GameObject victoryPopUp;
    public GameObject defeatPopUp;

    [Header("UI Elements")]
    // This list should now contain 4 TextMeshProUGUI elements in the Inspector
    public List<TextMeshProUGUI> abilityButtonTexts;
    public TextMeshProUGUI bonusAbilityButtonText;

    [Header("Battle Log")]
    public GameObject logTextPrefab;
    public Transform logContentParent;

    public bool isPlayerTurn = true;
    private bool hasUsedBonusAbilityThisTurn = false;

    void Start()
    {
        UpdateAbilityButtonNames();
        LogMessage("<color=green>Your turn!</color>");

    }

    public void PlayerUseAbility(int index)
    {
        if (!isPlayerTurn || player == null || enemy == null) return;

        
        bool isBonusSlot = (index == 99);

        Ability ability;
        if (isBonusSlot)
        {
            ability = player.data.bonusAbility;
        }
        else
        {
            ability = player.GetAbility(index);
        }

        if (ability != null)
        {
            if (isBonusSlot && hasUsedBonusAbilityThisTurn)
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

            if (isBonusSlot)
            {
                hasUsedBonusAbilityThisTurn = true;
                LogMessage("Bonus action complete. You can still use a regular ability.");
            }
            else
            {
                EndPlayerTurn();
            }
        }
    }


    public void LogMessage(string message)
    {
        if (logTextPrefab == null || logContentParent == null) return;

        GameObject newLog = Instantiate(logTextPrefab, logContentParent);
        newLog.transform.localScale = Vector3.one;
        newLog.GetComponent<TextMeshProUGUI>().text = message;

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
        if (player == null || player.data == null) return;

        for (int i = 0; i < abilityButtonTexts.Count; i++)
        {
            if (abilityButtonTexts[i] == null) continue;

            if (i < player.data.abilities.Length)
            {
                Ability ab = player.data.abilities[i];
                abilityButtonTexts[i].text = (ab != null) ? ab.abilityName : "Empty Slot";
            }
            else
            {
                abilityButtonTexts[i].text = "---";
            }
        }

        if (bonusAbilityButtonText != null)
        {
            if (player.data.bonusAbility != null)
            {
                bonusAbilityButtonText.text = player.data.bonusAbility.abilityName;
            }
            else
            {
                bonusAbilityButtonText.text = "Locked";
            }
        }
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

        Ability enemyAbility = enemy.GetAbility(0);

        if (enemyAbility != null)
        {
            LogMessage($"Enemy used <b>{enemyAbility.abilityName}</b>!");
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
    }

    void StartPlayerTurn()
    {
        hasUsedBonusAbilityThisTurn = false;
        isPlayerTurn = true;
        LogMessage("<color=green>Your turn!</color>");
        UpdateAbilityButtonNames();
    }

    void HandleVictory()
    {
        isPlayerTurn = false;
        LogMessage("<b>VICTORY!</b> The enemy was defeated.");

        StartCoroutine(VictoryRoutine());
    }

    IEnumerator VictoryRoutine()
    {
        enemy.PlayDeathAnimation();

        yield return new WaitForSeconds(2.0f);
        MapManager.currentNodeIndex++;

        if (NodeUI.isBossNext)
        {
            MapManager.currentNodeIndex = 0;
            SceneManager.LoadScene("VictoryScreen");
        }

        victoryPopUp.SetActive(true);
        Time.timeScale = 0f; 
    }

    void HandleDefeat()
    {
        isPlayerTurn = false;
        LogMessage("<color=red>DEFEAT...</color> You have fallen in battle.");

        StartCoroutine(DefeatRoutine());
    }

    IEnumerator DefeatRoutine()
    {
        player.PlayDeathAnimation();

        yield return new WaitForSeconds(2.0f);

        defeatPopUp.SetActive(true);
    }
}