using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using TKOM_Compiler;

namespace LexerTests
{
    [TestClass]
    public class SkipToTokenTest
    {
        [TestMethod]
        public void SkipWhiteCharacters()
        {
            string text = "     \r\n \n \n IWorld";
            Lexer lex = new Lexer(new StringReader(text));
            MethodInfo method = lex.GetType().GetMethod("SkipToToken", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(lex, null);
            Assert.AreEqual<char>('I',(char)lex.CurrentChar);
            Assert.AreEqual<int>(2, lex.CurrentPosition.column);
            Assert.AreEqual<int>(4, lex.CurrentPosition.line);
        }

        [TestMethod]
        public void SkipComments()
        {
            string text = "/*komentarzyk */IWorld";
            Lexer lex = new Lexer(new StringReader(text));
            MethodInfo method = lex.GetType().GetMethod("SkipToToken", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(lex, null);
            Assert.AreEqual<char>('I', (char)lex.CurrentChar);
        }

        [TestMethod]
        public void SkipCommentsLine()
        {
            {
                string text = "//komentarzyk \nIWorld";
                Lexer lex = new Lexer(new StringReader(text));
                MethodInfo method = lex.GetType().GetMethod("SkipToToken", BindingFlags.NonPublic | BindingFlags.Instance);
                method.Invoke(lex, null);
                Assert.AreEqual<char>('I', (char)lex.CurrentChar);
                Assert.AreEqual<int>(1, lex.CurrentPosition.column);
                Assert.AreEqual<int>(2, lex.CurrentPosition.line);
            }
            {
                string text = "/*komentarzyk*/IWorld";
                Lexer lex = new Lexer(new StringReader(text));
                MethodInfo method = lex.GetType().GetMethod("SkipToToken", BindingFlags.NonPublic | BindingFlags.Instance);
                method.Invoke(lex, null);
                Assert.AreEqual<char>('I', (char)lex.CurrentChar);
                Assert.AreEqual<int>(16, lex.CurrentPosition.column);
                Assert.AreEqual<int>(1, lex.CurrentPosition.line);
            }
        }
        [TestMethod]
        public void SkipWhiteAndCommets()
        {
            {
                string text = "//komentarzyk \n         \r\n           /*aasd \n koaskdosa kaosd*/         \n\n \r\nIWorld";
                Lexer lex = new Lexer(new StringReader(text));
                MethodInfo method = lex.GetType().GetMethod("SkipToToken", BindingFlags.NonPublic | BindingFlags.Instance);
                method.Invoke(lex, null);
                Assert.AreEqual<char>('I', (char)lex.CurrentChar);
                Assert.AreEqual<int>(1, lex.CurrentPosition.column);
                Assert.AreEqual<int>(7, lex.CurrentPosition.line);
            }
        }
        [TestMethod]
        public void WithoutCommentAndWhite()
        {
            {
                string text = "IWorld";
                Lexer lex = new Lexer(new StringReader(text));
                MethodInfo method = lex.GetType().GetMethod("SkipToToken", BindingFlags.NonPublic | BindingFlags.Instance);
                method.Invoke(lex, null);
                Assert.AreEqual<char>('I', (char)lex.CurrentChar);
                Assert.AreEqual<int>(1, lex.CurrentPosition.column);
                Assert.AreEqual<int>(1, lex.CurrentPosition.line);
            }
        }
    }
   

}
