using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using TKOM_Compiler;

namespace LexerTests
{
    [TestClass]
    public class GetTokenTests
    {
        [TestMethod]
        public void GetOperatorTest()
        {
            string text = "/ < <= > >= == != ( ) { } = * \" \" && || ";
            Lexer lex = new Lexer(new StringReader(text));
            Assert.AreEqual<TokenType>(TokenType.OPERATOR_DIV, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.BOOL_OPERATOR_LESS, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.BOOL_OPERATOR_LESSEQ, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.BOOL_OPERATOR_GR, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.BOOL_OPERATOR_GREQ, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.BOOL_OPERATOR_EQ, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.BOOL_OPERATOR_NOTEQ, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.OPEN_BRACKET, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.CLOSE_BRACKET, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.OPEN_C_BRACKET, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.CLOSE_C_BRACKET, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.OPERATOR_EQ, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.OPERATOR_MUL, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.QUOTATION, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.VAL_TEXT, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.QUOTATION, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.BOOL_OPERATOR_AND, lex.GetToken().Type);
            Assert.AreEqual<TokenType>(TokenType.BOOL_OPERATOR_OR, lex.GetToken().Type);
        }

        [TestMethod]
        public void GetTextTokenTest()
        {
            string text = "\" text \"";
            Lexer lex = new Lexer(new StringReader(text));
            Assert.AreEqual<TokenType>(TokenType.QUOTATION, lex.GetToken().Type);
            Token token = lex.GetToken();
            Assert.AreEqual<TokenType>(TokenType.VAL_TEXT, token.Type);
            Assert.AreEqual<string>(" text ", (string)token.Value);
            Assert.AreEqual<TokenType>(TokenType.QUOTATION, lex.GetToken().Type);
        }
    }
}
