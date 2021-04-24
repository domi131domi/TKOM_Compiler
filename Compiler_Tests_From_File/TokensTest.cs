using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TKOM_Compiler;

namespace Compiler_Tests_From_File
{
    class TokensTest
    {
        Lexer lexer;
        static private int width = 20;
        public void SetLexer(string filename)
        {
            try
            {
                lexer = new Lexer(new StreamReader(filename));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void PrintAllTokens()
        {
            Token token = lexer.GetToken();
            PrintHeader();
            while(token.Type != TokenType.SPECIAL_EOT)
            {
                PrintToken(token);
                token = lexer.GetToken();
            }
            PrintToken(token);
        }

        private void PrintHeader()
        {
            Console.WriteLine("Value".PadRight(width) + "|" + "Token Type".PadRight(width) + "|" + "Token Category".PadRight(width) + "|" + "Column".PadRight(width) + "|" + "line".PadRight(width) + "\n");
        }

        public static void PrintToken(Token token)
        {
            Console.WriteLine(((token.Value != null) ? token.Value.ToString().PadRight(width) : "".PadRight(width)) + '|' + token.Type.ToString().PadRight(width) + '|' + token.Category.ToString().PadRight(width) + '|' + token.Position.column.ToString().PadRight(width) + '|' + token.Position.line.ToString().PadRight(width));
        }
    }
}
