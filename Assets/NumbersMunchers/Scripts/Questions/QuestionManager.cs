using System.Collections.Generic;
using NumbersMunchers.Scripts.Questions;
using UnityEngine;

namespace NumbersMunchers.Scripts {
    public class QuestionManager : MonoBehaviour {
        
        [SerializeField] private List<QuestionModuleBehaviour> questionModules = new List<QuestionModuleBehaviour>();
        [SerializeField] private DifficultyLevel currentDifficulty;
        public QuestionInfo currentQuestion;
        
        private void Start() {
            Random.InitState((int)System.DateTime.Now.Ticks);
        }

        public QuestionInfo CreateNewQuestion() {
            currentQuestion = GetRandomModule().CreateQuestion(currentDifficulty);
            return currentQuestion;
        }

        private QuestionModuleBehaviour GetRandomModule() {
            int index = Random.Range(0, questionModules.Count);
            return questionModules[index];
        }
    }

}