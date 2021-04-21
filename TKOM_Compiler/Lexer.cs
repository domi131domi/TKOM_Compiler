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
            if (IsEOT())
                return new Token(null, TokenType.EOT, currentPosition);
            SkipToToken();
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
                    while (currentChar != '*' && Source.Peek() != '/')
                    {
                        currentChar = Source.Read();
                    }
                    Source.Read();
                    currentChar = Source.Read();
                    currentPosition.column++;
                }
            }
        }
        private bool IsEOT()
        {
            return currentChar == -1;
        }

    }
}
