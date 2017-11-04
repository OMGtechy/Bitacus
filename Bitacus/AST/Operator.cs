using System;

namespace Bitacus.AST
{
    public enum Operator
    {
        Asterisk,
        Slash,
        Plus,
        Minus
    }

    public static class OperatorExtensions
    {
        public static bool TryParseFromSymbol(this Operator op, char toParse, out Operator parsed)
        {
            switch (toParse)
            {
                case '+': parsed = Operator.Plus; return true;
                case '-': parsed = Operator.Minus; return true;
                case '*': parsed = Operator.Asterisk; return true;
                case '/': parsed = Operator.Slash; return true;
                default:
                    parsed = 0;
                    return false;
            }
        }

        public static Operator ParseFromSymbol(this Operator op, char toParse)
        {
            Operator result = 0;
            if(result.TryParseFromSymbol(toParse, out result))
            {
                return result;
            }

            throw new ArgumentException();
        }
    }
}
