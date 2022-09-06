using System.Collections.Generic;
using UnityEngine;

namespace NumbersMunchers.Scripts.Questions {
    public class MultiplesQuestionModule : QuestionModuleBehaviour {
        [SerializeField] private List<Vector2Int> ranges;
        
        public override QuestionInfo CreateQuestion(DifficultyLevel level) {
            var range = ranges[((int) level) - 1];

            int number = range.x + (int) (Random.value * (range.y - range.x));
            var trueNumbers = new List<int>();
            var falseNumbers = new List<int>();

            for (int i = number; i < 100; i++) {
                if (i % number == 0) {
                    trueNumbers.Add(i);
                } else if (falseNumbers.Count < 10) {
                    falseNumbers.Add(i);
                }
            }

            return new QuestionInfo {
                Question = $"Multiples of {number}",
                OperationTypes = new List<OperationType>() {
                    OperationType.None
                },
                TrueNumbers = trueNumbers,
                FalseNumbers = falseNumbers,
                Difficulty = level,
            };
        }
    }
}