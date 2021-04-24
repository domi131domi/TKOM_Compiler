using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using TKOM_Compiler;

namespace LexerTests
{
    [TestClass]
    public class GetNumberTests
    {
        [TestMethod]
        public void BadNumber()
        {
            {
                string text = "01234 ";
                Lexer lex = new Lexer(new StringReader(text));
                Token token = lex.GetToken();
                Assert.AreEqual<char>(' ', (char)lex.CurrentChar);
                Assert.AreEqual<int>(6, lex.CurrentPosition.column);
                Assert.AreEqual<int>(1, lex.CurrentPosition.line);
                Assert.AreEqual<TokenType>(TokenType.BAD_NUMBER, token.Type);
                Assert.AreEqual<string>("01234", (string)token.Value);
            }

            {
                string text = "0.12n34 ";
                Lexer lex = new Lexer(new StringReader(text));
                Token token = lex.GetToken();
                Assert.AreEqual<char>('n', (char)lex.CurrentChar);
                Assert.AreEqual<int>(5, lex.CurrentPosition.column);
                Assert.AreEqual<int>(1, lex.CurrentPosition.line);
                Assert.AreEqual<TokenType>(TokenType.VAL_DOUBLE, token.Type);
                Assert.AreEqual<double>(0.12, (double)token.Value);
            }
        }

        [TestMethod]
        public void DoubleNumber()
        {
            {
                string text = "0.345456 ";
                Lexer lex = new Lexer(new StringReader(text));
                Token token = lex.GetToken();
                Assert.AreEqual<TokenType>(TokenType.VAL_DOUBLE, token.Type);
                Assert.AreEqual<double>(0.345456, (double)token.Value);
                Assert.AreEqual<int>(9, lex.CurrentPosition.column);
                Assert.AreEqual<int>(1, lex.CurrentPosition.line);
            }

            {
                string text = "0.345\n444";
                Lexer lex = new Lexer(new StringReader(text));
                Token token = lex.GetToken();
                Assert.AreEqual<TokenType>(TokenType.VAL_DOUBLE, token.Type);
                Assert.AreEqual<double>(0.345, (double)token.Value);
                Assert.AreEqual<int>(6, lex.CurrentPosition.column);
                Assert.AreEqual<int>(1, lex.CurrentPosition.line);
            }

            {
                string text = "13.123213";
                Lexer lex = new Lexer(new StringReader(text));
                Token token = lex.GetToken();
                Assert.AreEqual<TokenType>(TokenType.VAL_DOUBLE, token.Type);
                Assert.AreEqual<double>(13.123213, (double)token.Value);
            }

            {
                string text = "   13.1232s13";
                Lexer lex = new Lexer(new StringReader(text));
                Token token = lex.GetToken();
                Assert.AreEqual<TokenType>(TokenType.VAL_DOUBLE, token.Type);
                Assert.AreEqual<double>(13.1232, (double)token.Value);
            }

        }

        [TestMethod]
        public void PeriodNumberTests()
        {
            {
                string text = "0.(3433)";
                Lexer lex = new Lexer(new StringReader(text));
                Token token = lex.GetToken();
                Assert.AreEqual<TokenType>(TokenType.VAL_PERIOD_NUMBER, token.Type);
                PeriodNumber per = new PeriodNumber(0, 3433);
                Assert.AreEqual<double>(per.DoublePart, ((PeriodNumber)token.Value).DoublePart);
                Assert.AreEqual<int>(per.Period, ((PeriodNumber)token.Value).Period);
                Assert.AreEqual<int>(9, lex.CurrentPosition.column);
                Assert.AreEqual<int>(1, lex.CurrentPosition.line);
            }

            {
                string text = "   13.123(3433)";
                Lexer lex = new Lexer(new StringReader(text));
                Token token = lex.GetToken();
                Assert.AreEqual<TokenType>(TokenType.VAL_PERIOD_NUMBER, token.Type);
                PeriodNumber per = new PeriodNumber(13.123, 3433);
                Assert.AreEqual<double>(per.DoublePart, ((PeriodNumber)token.Value).DoublePart);
                Assert.AreEqual<int>(per.Period, ((PeriodNumber)token.Value).Period);
            }

            {
                string text = "   13.123(343n";
                Lexer lex = new Lexer(new StringReader(text));
                Token token = lex.GetToken();
                Assert.AreEqual<TokenType>(TokenType.EXPECTED_CLOSING_BRACKET, token.Type);
                PeriodNumber per = new PeriodNumber(13.123, 343);
                Assert.AreEqual<double>(per.DoublePart, ((PeriodNumber)token.Value).DoublePart);
                Assert.AreEqual<int>(per.Period, ((PeriodNumber)token.Value).Period);
            }
        }
    }
}
