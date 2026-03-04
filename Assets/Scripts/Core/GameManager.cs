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

            if (playerCharacter == null)
            {
                playerCharacter = Instantiate(playerPrefab);
                DontDestroyOnLoad(playerCharacter.gameObject);
            }
        }

        public void RegisterPlayer(Character character)
        {
            playerCharacter = character;
        }

        public void RegisterBattle(BattleController battle)
        {
            battleController = battle;
        }
    }
}
