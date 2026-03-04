using UnityEngine;
using BoardAgain.Characters;
using BoardAgain.Battle;


namespace BoardAgain.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Runtime References")]
        public Character playerPrefab;
        [HideInInspector] public Character playerCharacter;

        public BattleController battleController;

        private void Awake()
        {
            // Singleton pattern
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

        public void InitalizePlayer()
        {
            if (playerCharacter != null) return;

            playerCharacter = Instantiate(playerPrefab);
            DontDestroyOnLoad(playerCharacter.gameObject);
        }

        public void ResetGame()
        {
            // Clean up player character
            if (playerCharacter != null)
            {
                Destroy(playerCharacter.gameObject);
                playerCharacter = null;
            }
            // Clean up battle controller
            if (battleController != null)
            {
                Destroy(battleController.gameObject);
                battleController = null;
            }
        }
    }
}
