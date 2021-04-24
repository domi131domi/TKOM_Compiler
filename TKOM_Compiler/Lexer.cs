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
            if (!isText)
                SkipToToken();
            if (IsEOT())
                return new Token(null, TokenType.SPECIAL_EOT, currentPosition);
            Token token = null;
            tokenPosition = currentPosition;

            token = GetText();
            if (token == null)
                token = GetOperator();
            if (token == null)
                token = GetNumber();
            if (token == null)
            {
                string identifier = BuildIdentifier();
                if(!string.IsNullOrEmpty(identifier))
                    token = GetKeyword(identifier);
            }
            if(token == null)
            {
                char unknown = (char)currentChar;
                currentChar = Source.Read();
                currentPosition.column++;
                return new Token(unknown, TokenType.UNKNOWN, tokenPosition);
            }
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
            if ((char)currentChar == '/')
            {
                int next = Source.Peek();
                if (next != -1 && (char)next == '/')
                {
                    Source.Read();
                    currentChar = Source.Read();
                    while (currentChar != '\r' && currentChar != '\n')
                    {
                        currentChar = Source.Read();
                        if (IsEOT())
                            return;
                    }
                    if (currentChar == '\r' && Source.Peek() == '\n')
                        Source.Read();
                    currentChar = Source.Read();
                    currentPosition.line++;
                    currentPosition.column = 1;
                }
                else if (next != -1 && (char)next == '*')
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


        /// <summary>
        /// Gets token
        /// </summary>
        /// <returns>Returns token if any operator is recognised, otherwise null</returns>
        private Token GetOperator()
        {
            switch (currentChar)
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
                case '?':
                    currentChar = Source.Read();
                    currentPosition.column++;
                    return new Token('?', TokenType.OPERATOR_QM, tokenPosition);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Reads text after quotation mark and creates token
        /// </summary>
        private Token GetText()
        {
            if (currentChar == '"')
            {
                currentChar = Source.Read();
                currentPosition.column++;
                isText = !isText;
                return new Token('"', TokenType.QUOTATION, tokenPosition);
            }
            if (isText)
            {
                StringBuilder text = new StringBuilder();
                while (currentChar != '"')
                {
                    text.Append((char)currentChar);
                    currentChar = Source.Read();
                    currentPosition.column++;
                    if (IsEOT())
                        break;
                }
                return new Token(text.ToString(), TokenType.VAL_TEXT, tokenPosition);
            }
            return null;
        }

        /// <summary>
        /// Creates token for number
        /// </summary>
        /// <returns>Token Integer or Token PeriodNumber or Token Double, depends of type of read number</returns>
        private Token GetNumber()
        {
            if (currentChar == '0')
            {
                currentChar = Source.Read();
                currentPosition.column++;
                if (currentChar != '.')
                    return CreateTokenFromWord('0', TokenType.BAD_NUMBER);

                currentChar = Source.Read();
                currentPosition.column++;
                if (char.IsDigit((char)currentChar))
                {
                    return CreateDecimalNumber();
                }
                else if (currentChar == '(')
                {
                    currentChar = Source.Read();
                    currentPosition.column++;
                    return CreatePeriodNumberToken(0);
                }
                else
                {
                    return CreateTokenFromWord("0.", TokenType.BAD_NUMBER);
                }
            }
            else if (char.IsDigit((char)currentChar))
            {
                int result = 0;
                while (!IsEOT() && !IsWhite())
                {
                    if (char.IsDigit((char)currentChar))
                    {
                        result = result * 10 + (currentChar - '0');
                        currentChar = Source.Read();
                        currentPosition.column++;
                    }
                    else if (currentChar == '.')
                    {
                        currentChar = Source.Read();
                        currentPosition.column++;
                        return CreateDecimalNumber(result);
                    }
                    else
                    {
                        return new Token(result, TokenType.VAL_INTEGER, tokenPosition);
                    }
                }
                return new Token(result, TokenType.VAL_INTEGER, tokenPosition);
            }

            return null;
        }

        /// <summary>
        /// Creates unknown token with value of text untill first white
        /// </summary>
        /// <param name="first">adds character as a first letter of token</param>
        /// <returns>Returns unknown token</returns>
        private Token CreateTokenFromWord(char first, TokenType type)
        {
            StringBuilder text = new StringBuilder();
            text.Append(first);
            while (!IsWhite())
            {
                text.Append((char)currentChar);
                currentChar = Source.Read();
                currentPosition.column++;
            }
            return new Token(text.ToString(), type, tokenPosition);
        }

        /// <summary>
        /// Creates unknown token with value of text untill first white
        /// </summary>
        /// <param name="first">adds string as a first letters of token</param>
        /// <returns>Returns unknown token</returns>
        private Token CreateTokenFromWord(string first, TokenType type)
        {
            StringBuilder text = new StringBuilder();
            text.Append(first);
            while (!IsWhite())
            {
                text.Append((char)currentChar);
                currentChar = Source.Read();
                currentPosition.column++;
            }
            return new Token(text.ToString(), type, tokenPosition);
        }

        bool IsWhite()
        {
            return (currentChar == ' ' || currentChar == '\r' || currentChar == '\n');
        }

        private Token CreateDecimalNumber(double prefix = 0)
        {
            int power = 1;
            double dec = 0;
            while (!IsWhite() && !IsEOT() && char.IsDigit((char)currentChar))
            {
                dec = dec * 10 + (currentChar - '0');
                power++;
                currentChar = Source.Read();
                currentPosition.column++;
            }
            dec = dec / Math.Pow(10, power - 1) + prefix;
            if (currentChar == '(')
            {
                currentChar = Source.Read();
                currentPosition.column++;
                return CreatePeriodNumberToken(dec);
            }
            return new Token(dec, TokenType.VAL_DOUBLE, tokenPosition);
        }

        private Token CreatePeriodNumberToken(double prefix)
        {
            int power = 1;
            int period = 0;
            while (!IsWhite() && !IsEOT() && char.IsDigit((char)currentChar))
            {
                period = period * 10 + (currentChar - '0');
                power++;
                currentChar = Source.Read();
                currentPosition.column++;
            }
            if (currentChar != ')')
                return new Token(new PeriodNumber(prefix, period), TokenType.EXPECTED_CLOSING_BRACKET, tokenPosition);
            currentChar = Source.Read();
            currentPosition.column++;
            return new Token(new PeriodNumber(prefix, period), TokenType.VAL_PERIOD_NUMBER, tokenPosition);
        }

        private string BuildIdentifier()
        {
            StringBuilder iden = new StringBuilder();
            while ((currentChar >= 'A' && currentChar <= 'z') || currentChar == '_')
            {
                iden.Append((char)currentChar);
                currentChar = Source.Read();
                currentPosition.column++;
            }
            return iden.ToString();
        }

        /// <summary>
        /// If string is keyword returns token else null
        /// </summary>
        private Token GetKeyword(string word)
        {
            TokenType tokenType;
            if(KeyWords.TryGetValue(word,out tokenType))
            {
                Token token = new Token(word, tokenType, tokenPosition);
                if (tokenType == TokenType.VAL_TRUE)
                    token.Value = true;
                else if (tokenType == TokenType.VAL_FALSE)
                    token.Value = false;
                return token;
            }
            return new Token(word, TokenType.IDENTIFIER, tokenPosition);
        }


        private static Dictionary<string, TokenType> KeyWords = new Dictionary<string, TokenType>()
        {
            { "while", TokenType.KEYWORD_WHILE},
            { "if", TokenType.KEYWORD_IF},
            { "else", TokenType.KEYWORD_ELSE},
            { "return", TokenType.KEYWORD_RETURN},
            { "void", TokenType.TYPE_VOID},
            { "int", TokenType.TYPE_INT},
            { "double", TokenType.TYPE_DOUBLE},
            { "bool", TokenType.TYPE_BOOL},
            { "string", TokenType.TYPE_STRING},
            { "frac", TokenType.TYPE_FRAC},
            { "true", TokenType.VAL_TRUE},
            { "false", TokenType.VAL_FALSE},
        };
    }
}
