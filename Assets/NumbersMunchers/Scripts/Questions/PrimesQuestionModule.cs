using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NumbersMunchers.Scripts.Questions {
    public class PrimesQuestionModule : QuestionModuleBehaviour {
        [SerializeField] private List<Vector2Int> ranges;
        [SerializeField] private int maxPrimeNumber = 200;
        private List<int> _primes;

        private void Awake() {
            _primes = new List<int> { 2, 3, 5, 7 };
            CreatePrimes(maxPrimeNumber);
        }
        
        private void CreatePrimes(int maxNumber) {
            for (int i = 9; i <= maxNumber; i += 2) {
                if (IsPrime(i)) {
                    _primes.Add(i);
                }
            }
        }
        
        private bool IsPrime(int n) {
            //Run a loop from 2 to square root of n.
            for(int i=2; i*i<=n; i++){
                // if the number is divisible by i, then n is not a prime number.
                if(n%i==0)return false;
            }
            return true;
        }

        public override QuestionInfo CreateQuestion(DifficultyLevel level) {
            var trueNumbers = new List<int>();
            var falseNumbers = new List<int>();
            
            for (int i = 0; i < 5; i++) {
                trueNumbers.Add(_primes[Random.Range(0, _primes.Count)]);
            }

            for (int i = 2; i < 100; i++) {
                if (IsPrime(i)) {
                    trueNumbers.Add(i);
                } else if (falseNumbers.Count < 20) {
                    falseNumbers.Add(i);
                }
            }

            return new QuestionInfo {
                Question = "Find the Prime Numbers",
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