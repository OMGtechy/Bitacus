using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bitacus.AST
{
    public static class Parser
    {
        private static readonly List<List<Operator>> orderOfOperations = new List<List<Operator>>
        {
            new List<Operator> { Operator.Asterisk, Operator.Slash },
            new List<Operator> { Operator.Plus, Operator.Minus }
        };

        private static List<Lexeme> Simplify(List<Lexeme> lexemes)
        {
            for (int i = 0; i < lexemes.Count;)
            {
                while (i + 1 < lexemes.Count)
                {
                    // make sure that this lexeme and the next are operators
                    if ((lexemes[i].Kind != Lexeme.LexemeKind.Operator)
                      || (lexemes[i + 1].Kind != Lexeme.LexemeKind.Operator))
                    {
                        break;
                    }

                    var thisOp = lexemes[i].Operator;
                    var nextOp = lexemes[i + 1].Operator;

                    if (thisOp == Operator.Asterisk || thisOp == Operator.Slash)
                    {
                        // this is fine, but we're not going to be squashing them
                        break;
                    }

                    if (nextOp == Operator.Asterisk || nextOp == Operator.Slash)
                    {
                        // strings like +*, **, // and -/ (etc) are not considered valid
                        return null;
                    }

                    if (thisOp == Operator.Minus || nextOp == Operator.Minus)
                    {
                        if (thisOp == Operator.Minus && nextOp == Operator.Minus)
                        {
                            lexemes.RemoveRange(i, 2);
                        }
                        else
                        {
                            lexemes[i].Operator = Operator.Minus;
                            lexemes.RemoveAt(i + 1);
                        }
                    }
                }

                ++i;
            }

            return lexemes;
        }

        public static IExpression Parse(List<Lexeme> toParse)
        {
            if (toParse == null || toParse.Count == 0)
            {
                return null;
            }

            // handle brackets
            for (int i = 0; i < toParse.Count;)
            {
                if (toParse[i].Kind == Lexeme.LexemeKind.LeftBracket)
                {
                    int unclosedCount = 1;
                    for(int j = i + 1; j < toParse.Count; ++j)
                    {
                        switch(toParse[j].Kind)
                        {
                            case Lexeme.LexemeKind.LeftBracket: unclosedCount += 1; break;
                            case Lexeme.LexemeKind.RightBracket: unclosedCount -= 1; break;
                        }

                        if(unclosedCount == 0)
                        {
                            var subList = toParse.GetRange(i + 1, j - i - 1);
                            toParse.RemoveRange(i, 1 + j - i);

                            var value = Parse(subList).Evaluate().Value;
                            toParse.Insert(i, Lexer.Lex(value.ToString()).First());
                            break;
                        }
                    }

                    if(unclosedCount != 0)
                    {
                        return null;
                    }
                }
                else
                {
                    ++i;
                }
            }

            var lexemes = Simplify(toParse);

            if (lexemes == null || lexemes.Count == 0)
            {
                return null;
            }

            var expressions = lexemes.Where(l => l.Kind == Lexeme.LexemeKind.Number)
                                     .Select(l => (IExpression) new ValueExpression { Value = l.Number }).ToList();

            var operators = lexemes.Where(l => l.Kind == Lexeme.LexemeKind.Operator)
                                     .Select(l => l.Operator ).ToList();

            Debug.Assert(expressions.Count > 0);

            if (expressions.Count == operators.Count)
            {
                // the only valid data that can reach here is something like "-1",
                // anything else should have been simplified, or is invalid (such as "/1")
                if (operators[0] != Operator.Minus)
                {
                    return null;
                }

                operators.RemoveAt(0);

                // at this stage it'll only contain ValueExpressions
                var valueExpression = expressions[0] as ValueExpression;
                valueExpression.Value = -valueExpression.Value;
            }

            // handle other operators
            foreach (var opSet in orderOfOperations)
            {
                Debug.Assert(expressions.Count == operators.Count + 1);

                for (int i = 0; i < operators.Count;)
                {
                    var op = operators[i];

                    if (opSet.Contains(op))
                    {
                        var lhs = expressions[i];
                        var rhs = expressions[i + 1];

                        expressions.RemoveRange(i, 2);
                        operators.RemoveAt(i);

                        expressions.Insert(i, new BinaryExpression
                        {
                            LeftExpression = lhs,
                            Operator = op,
                            RightExpression = rhs
                        });
                    }
                    else
                    {
                        ++i;
                    }
                }
            }

            return expressions.Count == 1 ? expressions[0] : null;
        }
    }
}
