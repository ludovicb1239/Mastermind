using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind
{
    using static GlobalDefinitions;
    public static class GlobalDefinitions
    {

        public static int LENGTH = 4;
        public static char[] COLORS = { 'R', 'V', 'B', 'J', 'N', 'M', 'O', 'G' };
    }
    public static class Utils
    {
        public struct EvaluationAnswers
        {
            public ushort wellPlacedCount;
            public ushort misPlacedCount;

            public EvaluationAnswers()
            {
                wellPlacedCount = 0;
                misPlacedCount = 0;
            }
        }
        public static EvaluationAnswers Evaluation(char[] solution, char[] combination)
        {
            EvaluationAnswers answers = new();

            int length = solution.Length;
            int[] usedIndicesSolution = new int[length];
            int[] usedIndicesCombination = new int[length];

            if (solution.Length != combination.Length)
                throw new ArgumentException("The lengths of solution and combination must be equal.");

            for (int i = 0; i < length; i++)
            {
                if (solution[i] == combination[i])
                {
                    answers.wellPlacedCount++;
                    usedIndicesSolution[i] = 1;
                    usedIndicesCombination[i] = 1;
                }
            }

            for (int i = 0; i < length; i++)
            {
                if (usedIndicesSolution[i] == 1)
                    continue;

                for (int k = 0; k < length; k++)
                {
                    if (solution[i] == combination[k] && k != i && usedIndicesCombination[k] == 0)
                    {
                        answers.misPlacedCount++;
                        usedIndicesCombination[k] = 1;
                        break;
                    }
                }
            }

            return answers;
        }

        public static HashSet<char[]> GeneratePossibleCombinations(char[] testedCombination, EvaluationAnswers evaluation)
        {
            HashSet<char[]> combinationSet = new HashSet<char[]>();
            List<char[]> combinationsList = GenerateProduct(LENGTH);

            foreach (var combination in combinationsList)
            {
                EvaluationAnswers evalResult = Evaluation(combination, testedCombination);

                if (evalResult.wellPlacedCount == evaluation.wellPlacedCount && (evalResult.wellPlacedCount + evalResult.misPlacedCount) == (evalResult.wellPlacedCount + evalResult.misPlacedCount))
                {
                    combinationSet.Add(combination);
                }
            }

            return combinationSet;
        }

        // Equivalent of itertools.product in C#.
        private static List<char[]> GenerateProduct(int length)
        {
            List<char[]> answ = new();
            foreach (var element in COLORS)
            {
                foreach (var sequence in GenerateProduct(length - 1))
                {
                    answ.Add(new[] { element }.Concat(sequence).ToArray());
                }
            }
            return answ;
        }
    }
}
