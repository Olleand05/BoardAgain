using UnityEngine;
using BoardAgain.Characters;
using BoardAgain.Battle;


namespace BoardAgain.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public AbilityLibrary abilityLibrary;

        [Header("Runtime References")]
        public Character playerPrefab;
        [HideInInspector] public Character playerCharacter;

        public BattleController battleController;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void RegisterPlayer(Character character)
        {
            playerCharacter = character;
        }

        public void RegisterBattle(BattleController battle)
        {
            battleController = battle;
        }

        public void SetAbilities()
        {
            playerCharacter.equippedAbilities[0] = abilityLibrary.GetAbilityByName("Overpower");
            playerCharacter.equippedAbilities[1] = abilityLibrary.GetAbilityByName("Bash");
            playerCharacter.equippedAbilities[2] = null;
            playerCharacter.equippedAbilities[3] = null;
        }

        public void InitalizePlayer()
        {
            if (playerCharacter != null)
            {
                SetAbilities();
                return;
            }

            playerCharacter = Instantiate(playerPrefab);
            SetAbilities();
            DontDestroyOnLoad(playerCharacter.gameObject);
        }

        public void ResetGame()
        {
            if (playerCharacter != null)
            {
                Destroy(playerCharacter.gameObject);
                playerCharacter = null;
            }
            if (battleController != null)
            {
                Destroy(battleController.gameObject);
                battleController = null;
            }
        }
    }
}
