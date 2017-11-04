using Bitacus.AST;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestBitacus.AST
{
    [TestClass]
    public class TestValueExpression
    {
        [TestMethod]
        public void TestExpressPositiveValue()
        {
            // GIVEN: a value expression with the value 5
            var valueExpression = new ValueExpression { Value = 5 };

            // WHEN: it is evaluated
            var evaluation = valueExpression.Evaluate();

            // THEN: the result has a value of 5
            Assert.AreEqual(5, evaluation.Value);
        }

        [TestMethod]
        public void TestExpressNegativeValue()
        {
            // GIVEN: a value expression with the value -2
            var valueExpression = new ValueExpression { Value = -2 };

            // WHEN: it is evaluated
            var evaluation = valueExpression.Evaluate();

            // THEN: the result has a value of -2
            Assert.AreEqual(-2, evaluation.Value);
        }

        [TestMethod]
        public void TestExpressZeroValue()
        {
            // GIVEN: a value expression with the value 0
            var valueExpression = new ValueExpression { Value = 0 };

            // WHEN: it is evaluated
            var evaluation = valueExpression.Evaluate();

            // THEN: the result has a value of 0
            Assert.AreEqual(0, evaluation.Value);
        }
    }
}
