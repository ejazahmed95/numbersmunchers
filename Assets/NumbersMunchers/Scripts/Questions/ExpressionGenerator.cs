using RangerRPG.Core;

namespace NumbersMunchers.Scripts {
    
    public class ExpressionGenerator: SingletonBehaviour<ExpressionGenerator> {
        public string GetExpression(int number) {
            return number.ToString();
        }
    }
}