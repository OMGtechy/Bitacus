using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bitacus.AST;
using System.Collections.Generic;

namespace TestBitacus.AST
{
    [TestClass]
    public class TestParser
    {
        [TestMethod]
        public void TestParsePositiveNumber()
        {
            {
                // GIVEN: the lexeme for 42
                // WHEN: it is parsed
                var expression = Parser.Parse(new List<Lexeme>
                {
                    new Lexeme { Number = 42 }
                });

                // THEN: evaluating the expression yields 42
                Assert.AreEqual(42, expression.Evaluate().Value);
            }

            {
                // GIVEN: the lexeme for 0.1
                // WHEN: it is parsed
                var expression = Parser.Parse(new List<Lexeme>
                {
                    new Lexeme { Number = 0.1m }
                });

                // THEN: evaluating the expression yields 0.1
                Assert.AreEqual(0.1m, expression.Evaluate().Value);
            }
        }

        [TestMethod]
        public void TestParseNegativeNumber()
        {
            {
                // GIVEN: the lexemes for -12
                // WHEN: it is parsed
                var expression = Parser.Parse(new List<Lexeme>
                {
                    new Lexeme { Operator = Operator.Minus },
                    new Lexeme { Number = 12 }
                });

                // THEN: evaluating the expression yields -12
                Assert.AreEqual(-12, expression.Evaluate().Value);
            }

            {
                // GIVEN: the lexemes for -0.1
                // WHEN: it is parsed
                var expression = Parser.Parse(new List<Lexeme>
                {
                    new Lexeme { Operator = Operator.Minus },
                    new Lexeme { Number = 0.1m }
                });

                // THEN: evaluating the expression yields 0.1
                Assert.AreEqual(-0.1m, expression.Evaluate().Value);
            }
        }

        [TestMethod]
        public void TestParsePositiveAddExpression()
        {
            // GIVEN: the lexemes for 1 + 2
            // WHEN: it is parsed
            var expression = Parser.Parse(new List<Lexeme>
            {
                new Lexeme { Number = 1 },
                new Lexeme { Operator = Operator.Plus },
                new Lexeme { Number = 2 }
            });

            // THEN: evaluating the expression yields 3
            Assert.AreEqual(3, expression.Evaluate().Value);
        }

        [TestMethod]
        public void TestParseNegativeAddExpression()
        {
            {
                // GIVEN: the lexemes for -1 + 2
                // WHEN: it is parsed
                var expression = Parser.Parse(new List<Lexeme>
                {
                    new Lexeme { Operator = Operator.Minus },
                    new Lexeme { Number = 1 },
                    new Lexeme { Operator = Operator.Plus },
                    new Lexeme { Number = 2 }
                });

                // THEN: evaluating the expression yields 1
                Assert.AreEqual(1, expression.Evaluate().Value);
            }

            {
                // GIVEN: the lexemes for 3+-4
                // WHEN: it is parsed
                var expression = Parser.Parse(new List<Lexeme>
                {
                    new Lexeme { Number = 3 },
                    new Lexeme { Operator = Operator.Plus },
                    new Lexeme { Operator = Operator.Minus },
                    new Lexeme { Number = 4 }
                });

                // THEN: evaluating the expression yields -1
                Assert.AreEqual(-1, expression.Evaluate().Value);
            }
        }

        [TestMethod]
        public void TestParsePositiveSubtractExpression()
        {
            // GIVEN: the lexemes for 3 - 3;
            // WHEN: it is parsed
            var expression = Parser.Parse(new List<Lexeme>
            {
                new Lexeme { Number = 3 },
                new Lexeme { Operator = Operator.Minus },
                new Lexeme { Number = 3 }
            });

            // THEN: evaluating the expression yields 0
            Assert.AreEqual(0, expression.Evaluate().Value);
        }

        [TestMethod]
        public void TestParsePositiveMultiplyExpression()
        {
            // GIVEN: the lexemes for 3 * 3
            // WHEN: it is parsed
            var expression = Parser.Parse(new List<Lexeme>
            {
                new Lexeme { Number = 3 },
                new Lexeme { Operator = Operator.Asterisk },
                new Lexeme { Number = 3 }
            });

            // THEN: evaluating the expression yields 9
            Assert.AreEqual(9, expression.Evaluate().Value);
        }

        [TestMethod]
        public void TestParsePositiveDivideExpression()
        {
            // GIVEN: the lexemes for 3 / 3
            // WHEN: it is parsed
            var expression = Parser.Parse(new List<Lexeme>
            {
                new Lexeme { Number = 3 },
                new Lexeme { Operator = Operator.Slash },
                new Lexeme { Number = 3 }
            });

            // THEN: evaluating the expression yields 1
            Assert.AreEqual(1, expression.Evaluate().Value);
        }

        [TestMethod]
        public void TestParseOrderOfOperations()
        {
            {
                // GIVEN: the lexemes for 1 + 2 * 3
                // WHEN: it is parsed
                var expression = Parser.Parse(new List<Lexeme>
                {
                    new Lexeme { Number = 1 },
                    new Lexeme { Operator = Operator.Plus },
                    new Lexeme { Number = 2 },
                    new Lexeme { Operator = Operator.Asterisk },
                    new Lexeme { Number = 3 }
                });

                // THEN: evaluating the expression yields 7
                Assert.AreEqual(7, expression.Evaluate().Value);
            }

            {
                // GIVEN: the lexemes 1 * 2 + 3
                // WHEN: it is parsed
                var expression = Parser.Parse(new List<Lexeme>
                {
                    new Lexeme { Number = 1 },
                    new Lexeme { Operator = Operator.Asterisk },
                    new Lexeme { Number = 2 },
                    new Lexeme { Operator = Operator.Plus },
                    new Lexeme { Number = 3 }
                });

                // THEN: evaluating the expression yields 5
                Assert.AreEqual(5, expression.Evaluate().Value);
            }

            {
                // GIVEN: the lexemes for 5 * 7 / 7 + 9 - 3
                // WHEN: it is parsed
                var expression = Parser.Parse(new List<Lexeme>
                {
                    new Lexeme { Number = 5 },
                    new Lexeme { Operator = Operator.Asterisk },
                    new Lexeme { Number = 7 },
                    new Lexeme { Operator = Operator.Slash },
                    new Lexeme { Number = 7 },
                    new Lexeme { Operator = Operator.Plus },
                    new Lexeme { Number = 9 },
                    new Lexeme { Operator = Operator.Minus },
                    new Lexeme { Number = 3 }
                });

                // THEN: evaluating the expression yields 11
                Assert.AreEqual(11, expression.Evaluate().Value);
            }
        }

        [TestMethod]
        public void TestParseDoubleNegative()
        {
            // GIVEN: the lexemes for --1
            // WHEN: it is parsed
            var expression = Parser.Parse(new List<Lexeme>
            {
                new Lexeme { Operator = Operator.Minus },
                new Lexeme { Operator = Operator.Minus },
                new Lexeme { Number = 1 }
            });

            // THEN: evaluating the expression yields 1
            Assert.AreEqual(1, expression.Evaluate().Value);
        }
    }
}
