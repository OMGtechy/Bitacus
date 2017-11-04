using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Bitacus.AST
{
    [DebuggerDisplay("{value}")]
    public class Lexeme
    {
        public enum LexemeKind
        {
            Number,
            Operator
        };

        public LexemeKind Kind { get; private set; }

        private object value;

        public decimal Number
        {
            get { return ((decimal?)value).Value; }
            set { this.value = value; Kind = LexemeKind.Number; }
        }
        public Operator Operator
        {
            get { return (Operator)value; }
            set { this.value = value; Kind = LexemeKind.Operator; }
        }
    }

    public static class Lexer
    {
        private const NumberStyles allowedNumberStyle =
                                                NumberStyles.Number
                                              ^ NumberStyles.AllowTrailingWhite
                                              ^ NumberStyles.AllowTrailingSign;

        private static Lexeme LexNumber(ref string toParse)
        {
            if (string.IsNullOrEmpty(toParse))
            {
                return null;
            }

            for (int digit = 1; ; ++digit)
            {
                decimal value;
                // TODO: are there tons of substrings getting allocated here, or does it just store offsets?
                if (digit > toParse.Length || !decimal.TryParse(toParse.Substring(0, digit), allowedNumberStyle, null, out value))
                {
                    var numericString = toParse.Substring(0, digit - 1);
                    toParse = toParse.Substring(digit - 1);

                    return new Lexeme
                    {
                        Number = decimal.Parse(numericString)
                    };
                }
            }
        }

        public static List<Lexeme> Lex(string toParse)
        {
            if (string.IsNullOrWhiteSpace(toParse))
            {
                return null;
            }

            toParse = toParse.Trim();

            var lexemes = new List<Lexeme>();

            while (toParse.Length > 0)
            {
                Operator op = 0;

                if (decimal.TryParse(toParse[0].ToString(), allowedNumberStyle, null, out decimal value))
                {
                    lexemes.Add(LexNumber(ref toParse));
                }
                else if (op.TryParseFromSymbol(toParse[0], out op))
                {
                    lexemes.Add(new Lexeme
                    {
                        Operator = op
                    });

                    toParse = toParse.Substring(1);
                }
                else
                {
                    return null;
                }

                toParse = toParse.TrimStart();
            }

            return lexemes;
        }
    }
}
