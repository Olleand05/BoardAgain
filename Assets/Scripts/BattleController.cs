using UnityEngine;
using BoardAgain.Characters;
using BoardAgain.Abilities;
using System.Collections;

public class BattleController : MonoBehaviour
{
    public Character player;
    public Character enemy;

    public GameObject victoryPopUp;
    public GameObject defeatPopUp;

    public bool isPlayerTurn = true;
    private bool hasUsedBonusAbilityThisTurn = false;
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
