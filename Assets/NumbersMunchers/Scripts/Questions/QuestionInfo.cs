using System.Collections.Generic;

namespace NumbersMunchers.Scripts.Questions {
    public struct QuestionInfo {
        public string Question;
        public List<OperationType> OperationTypes;
        public List<int> TrueNumbers;
        public List<int> FalseNumbers;
        public DifficultyLevel Difficulty;
    }
}