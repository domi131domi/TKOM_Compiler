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

        TokenPosition currentPosition;
        int currentChar;

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
        Token GetToken()
        {
            SkipToToken();
            if (IsEOT())
                return new Token(null, TokenType.SPECIAL_EOT, currentPosition);
            Token token = null;
            token = getOperator();

            return null;
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
                    return new Token('/', TokenType.OPERATOR_DIV, currentPosition);
                case '*':
                    return new Token('*', TokenType.OPERATOR_MUL, currentPosition);
                case '+':
                    return new Token('+', TokenType.OPERATOR_ADD, currentPosition);
                case '-':
                    return new Token('-', TokenType.OPERATOR_SUB, currentPosition);
                case '=':
                    return new Token('=', TokenType.OPERATOR_EQUAL, currentPosition);

            }
        }
    }
}
