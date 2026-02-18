using UnityEngine;
using BoardAgain.Characters;
using BoardAgain.Abilities;
using System.Collections;

public class BattleController : MonoBehaviour
{
    public Character player;
    public Character enemy;

    public GameObject victoryPopUp;
    public bool isPlayerTurn = true;


    public void PlayerUseAbility(int index)
    {
        if (!isPlayerTurn || player == null || enemy == null) return;

        Ability ability = player.GetAbility(index);

        if (ability != null)
        {
            ability.ActivateAbility(player, enemy);

            if (enemy.IsCharacterDead())
            {
                HandleVictory();
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
        isPlayerTurn = true;
    }

    void HandleVictory()
    {
        Debug.Log("The Enemy has died! Battle Over.");
        victoryPopUp.SetActive(true);
        Time.timeScale = 0f;
    }
    void HandleDefeat()
    {
        isPlayerTurn = false;
        Debug.Log("You died! Battle Over.");
    }
}
