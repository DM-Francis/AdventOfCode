using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day18
{
    public class Program
    {
        static void Main(string[] args)
        {
            string[] rawExpressions = File.ReadAllLines("input.txt");

            string[] additionPriorityExpressions = rawExpressions.Select(e => AddBracketsAroundAdditionOperations(e)).ToArray();

            long sum = rawExpressions.Select(e => EvaluateExpression(e)).Sum();
            long sum2 = additionPriorityExpressions.Select(e => EvaluateExpression(e)).Sum();

            Console.WriteLine(sum);
        }

        public static long EvaluateExpression(string rawExpression)
        {
            string[] tokens = rawExpression.Replace("(", "( ").Replace(")", " )").Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var values = new List<long>();
            var operations = new List<string>();

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];

                if (token is "*" or "+")
                {
                    operations.Add(token);
                    continue;
                }

                long value;
                if (token == "(")
                {
                    int innerStart = i + 1;
                    int innerEnd = FindClosingBracketIndex(i, tokens);
                    string innerExpression = string.Join(" ", tokens[innerStart..innerEnd]);

                    value = EvaluateExpression(innerExpression);
                    i = innerEnd;
                }
                else
                {
                    value = long.Parse(token.ToString());
                }

                values.Add(value);
            }

            for (int j = 0; j < operations.Count; j++)
            {
                if (operations[j] == "+")
                {

                }
            }

            return 0; // TODO Rewrite all this code
        }

        private static int FindClosingBracketIndex(int openingBracketIndex, string[] tokens)
        {
            int openBracketCount = 0;
            for (int i = openingBracketIndex; i < tokens.Length; i++)
            {
                if (tokens[i] == "(")
                    openBracketCount++;
                else if (tokens[i] == ")")
                    openBracketCount--;

                if (openBracketCount == 0)
                    return i;
            }

            throw new InvalidOperationException("Closing bracket not found");
        }

        private static long ApplyOperation(string operation, long left, long right)
        {
            return operation switch
            {
                "*" => left * right,
                "+" => left + right,
                _ => throw new ArgumentException("Unrecognised operation character")
            };
        }

        public static string AddBracketsAroundAdditionOperations(string rawExpression)
        {
            List<string> tokens = rawExpression.Replace("(", "( ").Replace(")", " )").Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] == "+")
                {
                    tokens.Insert(i - 1, "(");
                    tokens.Insert(i + 3, ")");
                    i++;
                }
            }

            return string.Join(' ', tokens);
        }
    }
}
