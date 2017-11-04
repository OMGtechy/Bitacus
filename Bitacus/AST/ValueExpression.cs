using System.Diagnostics;

namespace Bitacus.AST
{
    [DebuggerDisplay("{Value}")]
    public class ValueExpression : IExpression
    {
        public ValueExpression Evaluate()
        {
            return this;
        }

        public decimal Value { get; set; }
    }
}
