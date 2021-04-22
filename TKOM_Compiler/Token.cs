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
        OPERATOR = 3,
        BOOL_OPERATOR = 4,
        SPECIAL = 9
    }

    public enum TokenType
    {
        //Keywords
        KEYWORD_WHILE = 101,
        KEYWORD_IF = 102,

        //Operators
        OPERATOR_DIV = 301,
        OPERATOR_MUL = 302,
        OPERATOR_ADD = 303,
        OPERATOR_SUB = 304,
        OPERATOR_EQUAL = 305,
        OPERATOR_QM = 306,  //? - operator more info in README.txt
        //BoolOperators
        BOOL_OPERATOR_NEG = 401,
        //Special
        SPECIAL_EOT = 901
    }

    public struct TokenPosition
    {
        public int column;
        public int line;
    }
}
