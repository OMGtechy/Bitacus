using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bitacus.AST;

namespace TestBitacus.AST
{
    [TestClass]
    public class TestLexer
    {
        [TestMethod]
        public void TestLexPositiveNumber()
        {
            {
                // GIVEN: the string "3"
                // WHEN: it is lexed
                var lexemes = Lexer.Lex("3");

                // THEN: 1 lexeme is produced
                Assert.IsNotNull(lexemes);
                Assert.AreEqual(1, lexemes.Count);

                // THEN: its contents are correct
                var lexeme = lexemes[0];
                Assert.IsNotNull(lexeme);
                Assert.AreEqual(Lexeme.LexemeKind.Number, lexeme.Kind);
                Assert.AreEqual(3, lexeme.Number);
            }

            {
                // GIVEN: the string "  42  "
                // WHEN: it is lexed
                var lexemes = Lexer.Lex("  42  ");

                // THEN: 1 lexeme is produced
                Assert.IsNotNull(lexemes);
                Assert.AreEqual(1, lexemes.Count);

                // THEN: its contents are correct
                var lexeme = lexemes[0];
                Assert.IsNotNull(lexeme);
                Assert.AreEqual(Lexeme.LexemeKind.Number, lexeme.Kind);
                Assert.AreEqual(42, lexeme.Number);
            }
        }

        [TestMethod]
        public void TestLexNegativeNumber()
        {
            {
                // GIVEN: the string "-40"
                // WHEN: it is lexed
                var lexemes = Lexer.Lex("-40");

                // THEN: 2 lexemes are produced
                Assert.IsNotNull(lexemes);
                Assert.AreEqual(2, lexemes.Count);

                // THEN: their contents are correct
                var lexeme1 = lexemes[0];
                var lexeme2 = lexemes[1];
                Assert.IsNotNull(lexeme1);
                Assert.IsNotNull(lexeme2);
                Assert.AreEqual(Lexeme.LexemeKind.Operator, lexeme1.Kind);
                Assert.AreEqual(Lexeme.LexemeKind.Number, lexeme2.Kind);
                Assert.AreEqual(Operator.Minus, lexeme1.Operator);
                Assert.AreEqual(40, lexeme2.Number);
            }

            {
                // GIVEN: the string " - 344 "
                // WHEN: it is lexed
                var lexemes = Lexer.Lex(" - 344 ");

                // THEN: 2 lexemes are produced
                Assert.IsNotNull(lexemes);
                Assert.AreEqual(2, lexemes.Count);

                // THEN: their contents are correct
                var lexeme1 = lexemes[0];
                var lexeme2 = lexemes[1];
                Assert.IsNotNull(lexeme1);
                Assert.IsNotNull(lexeme2);
                Assert.AreEqual(Lexeme.LexemeKind.Operator, lexeme1.Kind);
                Assert.AreEqual(Lexeme.LexemeKind.Number, lexeme2.Kind);
                Assert.AreEqual(Operator.Minus, lexeme1.Operator);
                Assert.AreEqual(344, lexeme2.Number);
            }
        }

        [TestMethod]
        public void TestLexNothing()
        {
            {
                // GIVEN: null
                // WHEN: it is lexed
                // THEN: null is returned
                Assert.IsNull(Lexer.Lex(null));
            }

            {
                // GIVEN: an empty string
                // WHEN: it is lexed
                // THEN: null is returned
                Assert.IsNull(Lexer.Lex(""));
            }

            {
                // GIVEN: a string of only whitespace
                // WHEN: it is lexed
                // THEN: null is returnd
                Assert.IsNull(Lexer.Lex("      "));
            }
        }
    }
}
