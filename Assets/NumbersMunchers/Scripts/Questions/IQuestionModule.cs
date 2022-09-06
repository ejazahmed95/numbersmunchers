using NumbersMunchers.Scripts.Questions;
using UnityEngine;

namespace NumbersMunchers.Scripts {
    public abstract class QuestionModuleBehaviour: MonoBehaviour {
        public abstract QuestionInfo CreateQuestion(DifficultyLevel level);
    }
}