using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using TKOM_Compiler;

namespace LexerTests
{
    [TestClass]
    public class KeyWordsIndefierTests
    {
        [TestMethod]
        public void KeyWords()
        {
            {
                string text = "while string if else";
                Lexer lex = new Lexer(new StringReader(text));
                Assert.AreEqual<TokenType>(TokenType.KEYWORD_WHILE, lex.GetToken().Type);
                Assert.AreEqual<TokenType>(TokenType.TYPE_STRING, lex.GetToken().Type);
                Assert.AreEqual<TokenType>(TokenType.KEYWORD_IF, lex.GetToken().Type);
                Assert.AreEqual<TokenType>(TokenType.KEYWORD_ELSE, lex.GetToken().Type);
            }
        }

        [TestMethod]
        public void Identifier()
        {
            {
                string text = "if    some_name_ return";
                Lexer lex = new Lexer(new StringReader(text));
                Assert.AreEqual<TokenType>(TokenType.KEYWORD_IF, lex.GetToken().Type);
                Assert.AreEqual<TokenType>(TokenType.IDENTIFIER, lex.GetToken().Type);
                Assert.AreEqual<TokenType>(TokenType.KEYWORD_RETURN, lex.GetToken().Type);
            }
        }

        [TestMethod]
        public void Unknown()
        {
            {
                string text = "while @";
                Lexer lex = new Lexer(new StringReader(text));
                Assert.AreEqual<TokenType>(TokenType.KEYWORD_WHILE, lex.GetToken().Type);
                Assert.AreEqual<TokenType>(TokenType.UNKNOWN, lex.GetToken().Type);
            }
        }
    }
}
