using System;
using System.Collections.Generic;
using System.Text;

namespace TKOM_Compiler
{
    public class Token
    {
        object Value { get; set; }
        TokenCategory Category { get; set; }
        TokenType Type { get; set; }
        TokenPosition Position { get; set; }

        public Token(object value, TokenType type, TokenPosition position)
        {
            Category = (TokenCategory)((int)type % 100);
            Type = type;
            Value = value;
            Position = position;
        }
    }

    public enum TokenCategory
    {
        KEYWORD = 1,
        CONST = 2,
        SPECIAL = 9
    }

    public enum TokenType
    {
        //Keywords
        WHILE = 101,
        IF = 102,
        //Special
        EOT = 901
    }

    public struct TokenPosition
    {
        public int column;
        public int line;
    }
}
