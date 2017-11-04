using Bitacus.AST;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestBitacus.AST
{
    [TestClass]
    public class TestBinaryExpression
    {
        [TestMethod]
        public void TestExpressAdd()
        {
            // GIVEN: an expression representing 2 + 5
            var lhs = new ValueExpression { Value = 2 };
            var rhs = new ValueExpression { Value = 5 };

            var binaryExpression = new BinaryExpression
            {
                LeftExpression = lhs,
                RightExpression = rhs,
                Operator = Operator.Plus
            };

            // WHEN: it is evaluated
            var evaluation = binaryExpression.Evaluate();

            // THEN: the result has a value of 7
            Assert.AreEqual(7, evaluation.Value);
        }

        [TestMethod]
        public void TestExpressSubtract()
        {
            // GIVEN: an expression representing 9 - 4
            var lhs = new ValueExpression { Value = 9 };
            var rhs = new ValueExpression { Value = 4 };

            var binaryExpression = new BinaryExpression
            {
                LeftExpression = lhs,
                RightExpression = rhs,
                Operator = Operator.Minus
            };

            // WHEN: it is evaluated
            var evaluation = binaryExpression.Evaluate();

            // THEN: the result has a value of 5
            Assert.AreEqual(5, evaluation.Value);
        }

        [TestMethod]
        public void TestExpressMultiply()
        {
            // GIVEN: an expression representing 6 * 3
            var lhs = new ValueExpression { Value = 6 };
            var rhs = new ValueExpression { Value = 3 };

            var binaryExpression = new BinaryExpression
            {
                LeftExpression = lhs,
                RightExpression = rhs,
                Operator = Operator.Asterisk
            };

            // WHEN: it is evaluated
            var evaluation = binaryExpression.Evaluate();

            // THEN: the result has a value of 18
            Assert.AreEqual(18, evaluation.Value);
        }

        [TestMethod]
        public void TestExpressDivide()
        {
            // GIVEN: an expression representing 8 / 4
            var lhs = new ValueExpression { Value = 8 };
            var rhs = new ValueExpression { Value = 4 };

            var binaryExpression = new BinaryExpression
            {
                LeftExpression = lhs,
                RightExpression = rhs,
                Operator = Operator.Slash
            };

            // WHEN: it is evaluated
            var evaluation = binaryExpression.Evaluate();

            // THEN: the result has a value of 2
            Assert.AreEqual(2, evaluation.Value);
        }
    }
}
