using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerComp442.Src.Lexical
{
    public static class LexicalUtils
    {
        public static bool IsReservedWord(string text)
        {
            List<string> keywords = new List<string>{
                "if", "then", "else", "integer", "float", "void", "public", "private",
                "func", "var", "struct", "while", "read", "write", "return", "self", "inherits",
                "let", "impl" };

            return keywords.Contains(text);
        }

        public static bool IsLetter(char character)
        {
            //[a-z] or [A-Z]
            return char.IsLetter(character);
        }

        public static bool IsDigit(char character)
        {
            // [0-9]
            return char.IsDigit(character);
        }

        public static bool IsNonZero(char character)
        {
            // [1-9]
            return IsDigit(character) && !character.Equals('0');
        }

        public static bool IsAlphaNum(char character)
        {
            // letter or digit or underscore
            return IsLetter(character) || IsDigit(character) || character.Equals('_');
        }

        public static bool IsFraction(string text)
        {
            // needs to start by period
            if (text.Substring(0, 1) != ".") return false;

            if (text.Length == 2)
            {
                //.0 through .9
                return IsDigit(char.Parse(text.Substring(1, 1)));
            }
            else
            {
                // cannot have trailing zeroes
                if (!IsNonZero(text.Last())) return false;

                // each character must be a digit
                foreach (char c in text)
                {
                    if (!IsDigit(c)) return false;
                }
            }

            return true;
        }

        public static bool IsId(string text)
        {
            if (text.Length == 1)
            {
                return IsLetter(char.Parse(text));
            }
            else
            {
                // first character must be a letter
                if (!IsLetter(char.Parse(text.Substring(0, 1)))) return false;

                // each character must be a digit
                foreach (char c in text)
                {
                    if (!IsAlphaNum(c)) return false;
                }
            }
        }

        public static bool IsInteger(string text)
        {
            if (text.Length == 1)
            {
                return text.Equals("0");
            }
            else
            {
                // first character must be non zero
                if (!IsNonZero(char.Parse(text.Substring(0, 1)))) return false;

                // each character must be a digit
                foreach (char c in text)
                {
                    if (!IsDigit(c)) return false;
                }
            }

            return true;
        }
    }
}
