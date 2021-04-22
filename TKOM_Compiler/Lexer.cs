using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TKOM_Compiler
{
    public class Lexer
    {
        public TextReader Source { get; set; }
        public TokenPosition CurrentPosition { get { return currentPosition; } }
        public int CurrentChar { get { return currentChar; } }

        TokenPosition tokenPosition;
        TokenPosition currentPosition;
        int currentChar;
        const int MAX_KEYWORD_LENGHT = 15;
        bool isText = false;

        public Lexer(TextReader source)
        {
            Source = source;
            currentPosition.line = 1;
            currentPosition.column = 1;
            currentChar = Source.Read();
        }

        /// <summary>
        /// Gets one Token from source
        /// </summary>
        /// <returns>Returns token, if token wasn't recognised it returns token with category UNKNOWN</returns>
        public Token GetToken()
        {
            if(!isText)
                SkipToToken();
            if (IsEOT())
                return new Token(null, TokenType.SPECIAL_EOT, currentPosition);
            Token token = null;
            tokenPosition = currentPosition;

            token = GetText();
            if (token != null)
                return token;
            token = getOperator();

            return token;
        }

        /// <summary>
        /// Moves indicator in source skipping white characters and comments, also updates current position
        /// </summary>
        private void SkipToToken()
        {
            skipComment();
            while (!IsEOT() && char.IsWhiteSpace((char)currentChar))
            {
                if ((char)currentChar == '\n')
                {
                    currentPosition.column = 1;
                    currentPosition.line++;
                }
                else if (currentChar != '\r')
                    currentPosition.column++;
                currentChar = Source.Read();
                skipComment();
            }

        }

        /// <summary>
        /// Skips all comments and updates position
        /// </summary>
        private void skipComment()
        {
            if((char)currentChar == '/')
            {
                int next = Source.Peek();
                if(next != -1 && (char)next == '/')
                {
                    Source.Read();
                    currentChar = Source.Read();
                    while (currentChar != '\r' && currentChar != '\n')
                    {
                        currentChar = Source.Read();
                        if (IsEOT())
                            return;
                    }
                    if(currentChar == '\r' && Source.Peek() == '\n')
                        Source.Read();
                    currentChar = Source.Read();
                    currentPosition.line++;
                    currentPosition.column = 1;
                }
                else if(next != -1 && (char)next == '*')
                {
                    Source.Read();
                    currentChar = Source.Read();
                    currentPosition.column += 2;

                    while (!(currentChar == '*' && Source.Peek() == '/'))
                    {
                        currentChar = Source.Read();
                        if (IsEOT())
                            return;
                        if ((char)currentChar == '\n')
                        {
                            currentPosition.column = 1;
                            currentPosition.line++;
                        }
                        else if (currentChar != '\r')
                            currentPosition.column++;
                    }
                    Source.Read();
                    currentChar = Source.Read();
                    currentPosition.column += 2;
                }
            }
        }

        /// <summary>
        /// Checks if it is end of text source
        /// </summary>
        /// <returns>True if is end of source, false if it is not</returns>
        private bool IsEOT()
        {
            return currentChar == -1;
        }

        private Token getOperator()
        {
            switch(currentChar)
            {
                case '/':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    return new Token('/', TokenType.OPERATOR_DIV, tokenPosition);
                case '*':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    return new Token('*', TokenType.OPERATOR_MUL, tokenPosition);
                case '+':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    return new Token('+', TokenType.OPERATOR_ADD, tokenPosition);
                case '-':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    return new Token('-', TokenType.OPERATOR_SUB, tokenPosition);
                case '=':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    if (currentChar == '=')
                    {
                        currentChar = Source.Read();
                        currentPosition.column++;
                        return new Token("==", TokenType.BOOL_OPERATOR_EQ, tokenPosition);
                    }
                    return new Token('=', TokenType.OPERATOR_EQ, tokenPosition);
                case '!':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    if (currentChar == '=')
                    {
                        currentChar = Source.Read();
                        currentPosition.column++;
                        return new Token("!=", TokenType.BOOL_OPERATOR_NOTEQ, tokenPosition);
                    }
                    return new Token('!', TokenType.BOOL_OPERATOR_NEG, tokenPosition);
                case '|':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    if (currentChar == '|')
                    {
                        currentChar = Source.Read();
                        currentPosition.column++;
                        return new Token("||", TokenType.BOOL_OPERATOR_OR, tokenPosition);
                    }
                    return new Token('|', TokenType.UNKNOWN, tokenPosition);
                case '&':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    if (currentChar == '&')
                    {
                        currentChar = Source.Read();
                        currentPosition.column++;
                        return new Token("&&", TokenType.BOOL_OPERATOR_AND, tokenPosition);
                    }
                    return new Token('&', TokenType.UNKNOWN, tokenPosition);
                case '<':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    if (currentChar == '=')
                    {
                        currentChar = Source.Read();
                        currentPosition.column++;
                        return new Token("<=", TokenType.BOOL_OPERATOR_LESSEQ, tokenPosition);
                    }
                    return new Token('<', TokenType.BOOL_OPERATOR_LESS, tokenPosition);
                case '>':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    if (currentChar == '=')
                    {
                        currentChar = Source.Read();
                        currentPosition.column++;
                        return new Token(">=", TokenType.BOOL_OPERATOR_GREQ, tokenPosition);
                    }
                    return new Token('>', TokenType.BOOL_OPERATOR_GR, tokenPosition);
                case '(':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    return new Token('(', TokenType.OPEN_BRACKET, tokenPosition);
                case ')':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    return new Token(')', TokenType.CLOSE_BRACKET, tokenPosition);
                case '{':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    return new Token('{', TokenType.OPEN_C_BRACKET, tokenPosition);
                case '}':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    return new Token('}', TokenType.CLOSE_C_BRACKET, tokenPosition);
                case ';':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    return new Token(';', TokenType.SEMICOLON, tokenPosition);
                case '"':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    isText = !isText;
                    return new Token('"', TokenType.QUOTATION, tokenPosition);
                default:
                    return null;
            }
        }

        private Token GetText()
        {
            if(currentChar == '"')
            {
                currentChar = Source.Read();
                currentPosition.column++;
                isText = !isText;
                return new Token('"', TokenType.QUOTATION, tokenPosition);
            }
            if(isText)
            {
                StringBuilder text = new StringBuilder();
                while(currentChar != '"')
                {
                    text.Append((char)currentChar);
                    currentChar = Source.Read();
                    currentPosition.column++;
                    if (IsEOT())
                        break;
                }
                return new Token(text.ToString(), TokenType.TEXT, tokenPosition);
            }
            return null;
        }

        private Token getNumber()
        {
            if(currentChar == '0')
            {
                
            }
            return null;
        }
    }
}
