using System;
using TKOM_Compiler;

namespace Compiler_Tests_From_File
{
    class Program
    {
        static void Main(string[] args)
        {
            string folder = "D:\\Projekty\\TKOM_Compiler\\Compiler_Tests_From_File\\Files\\";
            string file = "LexerTestFile.txt";
            TokensTest test = new TokensTest();
            test.SetLexer(folder+file);
            test.PrintAllTokens();
        }
    }
}
