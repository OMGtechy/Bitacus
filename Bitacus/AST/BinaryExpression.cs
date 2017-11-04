using System;
using System.Diagnostics;

namespace Bitacus.AST
{
    [DebuggerDisplay("{LeftExpression} {Operator} {RightExpression}")]
    public class BinaryExpression : IExpression
    {
        public ValueExpression Evaluate()
        {
            var leftValue = LeftExpression.Evaluate().Value;
            var rightValue = RightExpression.Evaluate().Value;

            switch (Operator)
            {
                case Operator.Plus:     return new ValueExpression { Value = leftValue + rightValue };
                case Operator.Minus:    return new ValueExpression { Value = leftValue - rightValue };
                case Operator.Asterisk: return new ValueExpression { Value = leftValue * rightValue };
                case Operator.Slash:    return new ValueExpression { Value = leftValue / rightValue };
            }

            throw new InvalidOperationException();
        }

        public IExpression LeftExpression  { get; set; }
        public IExpression RightExpression { get; set; }

        public Operator Operator { get; set; }
    }
}
