using RangerRPG.Core;
using UnityEngine;

namespace NumbersMunchers.Scripts {
    public class GameManager : SingletonBehaviour<GameManager> {
        [SerializeField] private int playerLives = 3;

        public override void Awake() {
            base.Awake();
            if (Instance == null || Instance == this) {
                DontDestroyOnLoad(gameObject);	
            }
        }
        
        public static void LoadGame() {
            CustomSceneLoader.LoadScene("GameScene");
        }
    }
}