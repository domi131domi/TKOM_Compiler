using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Assert.AreEqual<char>('W',(char)lex.Source.Read());
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
            Assert.AreEqual<char>('W', (char)lex.Source.Read());
        }

        [TestMethod]
        public void SkipCommentsLine()
        {
            {
                string text = "//komentarzyk \nIWorld";
                Lexer lex = new Lexer(new StringReader(text));
                MethodInfo method = lex.GetType().GetMethod("SkipToToken", BindingFlags.NonPublic | BindingFlags.Instance);
                method.Invoke(lex, null);
                Assert.AreEqual<char>('W', (char)lex.Source.Read());
            }
            {
                string text = "//komentarzyk \r\nIWorld";
                Lexer lex = new Lexer(new StringReader(text));
                MethodInfo method = lex.GetType().GetMethod("SkipToToken", BindingFlags.NonPublic | BindingFlags.Instance);
                method.Invoke(lex, null);
                Assert.AreEqual<char>('W', (char)lex.Source.Read());
            }
        }
    }
}
