using UnityEngine;
using BoardAgain.Characters;
using BoardAgain.Abilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class BattleController : MonoBehaviour
{
    public Character player;
    public Character enemy;

    public GameObject victoryPopUp;
    public GameObject defeatPopUp;

    [Header("UI Elements")]
    public List<TextMeshProUGUI> abilityButtonTexts; 
    public TextMeshProUGUI bonusAbilityButtonText;

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
            Debug.Log("You have already used your bonus ability this round!");
            return;
        }
        Ability ability = player.GetAbility(index);

        if (ability != null)
        {
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
                Debug.Log("Bonus Ability used! You can still use a regular ability.");
            }
            else
            {
                EndPlayerTurn();
            }
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
        UpdateAbilityButtonNames();
    }

    void HandleVictory()
    {
        isPlayerTurn = false;
        Debug.Log("The Enemy has died! Battle Over.");
        victoryPopUp.SetActive(true);
        Time.timeScale = 0f;
    }
    void HandleDefeat()
    {
        isPlayerTurn = false;
        Debug.Log("You died! Battle Over.");
        defeatPopUp.SetActive(true);

    }
}
