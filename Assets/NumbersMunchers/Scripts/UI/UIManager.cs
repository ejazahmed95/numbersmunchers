using NumbersMunchers.Scripts.Questions;
using RangerRPG.Core;
using TMPro;

namespace NumbersMunchers.Scripts.UI {
    public class UIManager : SingletonBehaviour<UIManager> {

        public TMP_Text questionText;
        public TMP_Text scoreText;
        public TMP_Text livesText;

        private int currentScore = 0;

        public void InitQuestion(QuestionInfo question) {
            questionText.text = question.Question;
        }

        public void UpdateScore(int addScore) {
            currentScore += addScore;
            scoreText.text = $"Score {currentScore}";
        }

        public void UpdateLives(int currentLives) {
            livesText.text = $"Lives {currentLives}";
        }
    }
}