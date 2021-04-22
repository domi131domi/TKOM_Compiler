using System;
using System.Collections.Generic;
using System.Text;

namespace TKOM_Compiler
{
    public class Token
    {
        public object Value { get; set; }
        public TokenCategory Category { get; set; }
        public TokenType Type { get; set; }
        public TokenPosition Position { get; set; }

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
        BRACKET = 5,
        VALUE = 6,
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
        OPERATOR_EQ = 305,
        OPERATOR_QM = 306,  //? - operator more info in README.txt
        QUOTATION = 307,
        SEMICOLON = 308,

        //BoolOperators
        BOOL_OPERATOR_NEG = 401,
        BOOL_OPERATOR_NOTEQ = 402,
        BOOL_OPERATOR_EQ = 403,
        BOOL_OPERATOR_OR = 404,
        BOOL_OPERATOR_AND = 405,
        BOOL_OPERATOR_LESSEQ = 406,
        BOOL_OPERATOR_LESS = 407,
        BOOL_OPERATOR_GREQ = 408,
        BOOL_OPERATOR_GR = 409,
        //Brackets
        OPEN_BRACKET = 501,
        CLOSE_BRACKET = 502,
        OPEN_C_BRACKET = 503,
        CLOSE_C_BRACKET = 504,
        //Values
        TEXT = 601,
        //Special
        SPECIAL_EOT = 901,
        UNKNOWN = 902
    }

    public struct TokenPosition
    {
        public int column;
        public int line;
    }
}
