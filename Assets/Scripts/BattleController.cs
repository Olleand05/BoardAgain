using UnityEngine;
using BoardAgain.Characters;
using BoardAgain.Abilities;

public class BattleController : MonoBehaviour
{
    public Character player;
    public Character enemy;

    public void PlayerUseAbility(int index)
    {
        Ability ability = player.GetAbility(index);

        if (ability != null)
            return;

        ability.ActivateAbility(player, enemy);

        EndPlayerTurn();
    }

    // Update is called once per frame
    void EndPlayerTurn()
    {
        StartEnemyTurn();
    }

    void StartEnemyTurn()
    {
        StartPlayerTurn(); //TODO: Implement enemy AI to choose an ability and activate it
    }

    void StartPlayerTurn()
    {
        //TODO: Implement any start of turn effects, such as regenerating health or applying buffs/debuffs
    }
}
