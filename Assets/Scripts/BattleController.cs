using BoardAgain.Abilities;
using BoardAgain.Characters;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BattleController : MonoBehaviour
{
    public Character player;
    public Character enemy;

    public GameObject victoryPopUp;
    public GameObject defeatPopUp;

    [Header("UI Elements")]
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
    }
    public void PlayerUseAbility(int index)
    {
        if (!isPlayerTurn || player == null || enemy == null) return;

        bool isBonusSlot = (index == player.data.abilites.Length-1);

        if (isBonusSlot && hasUsedBonusAbilityThisTurn)
        {
            LogMessage("<color=yellow>Bonus ability already used this turn!</color>");
            return;
        }
        Ability ability = player.GetAbility(index);

        if (ability != null)
        {
            LogMessage($"Player used <b>{ability.name}</b>!");
            ability.ActivateAbility(player, enemy);

            if (enemy.IsCharacterDead())
            {
                HandleVictory();
                return;
            }

            bool isBonusAbility = (index == player.data.abilites.Length);
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

            if (i < player.data.abilites.Length)
            {
                var ability = player.data.abilites[i];
                abilityButtonTexts[i].text = (ability != null) ? ability.name : "Empty";
            }
            else
            {
                abilityButtonTexts[i].text = "---";
            }
        }

        if (bonusAbilityButtonText != null)
        {
            bonusAbilityButtonText.text = (player.ActiveSynergyAbility != null) ?
                player.ActiveSynergyAbility.name : "Locked";
        }
    }

    // Update is called once per frame
    void EndPlayerTurn()
    {
        isPlayerTurn = false;
        StartCoroutine(EnemyTurnRoutine());
    }

    IEnumerator EnemyTurnRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        Ability enemyAbility = enemy.GetAbility(0);

        if (enemyAbility != null)
        {
            LogMessage($"Enemy used <b>{enemyAbility.name}</b>!");
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
        victoryPopUp.SetActive(true);
        Time.timeScale = 0f;
    }
    void HandleDefeat()
    {
        isPlayerTurn = false;
        LogMessage("<color=red>DEFEAT...</color> You have fallen in battle.");
        defeatPopUp.SetActive(true);

    }
}
