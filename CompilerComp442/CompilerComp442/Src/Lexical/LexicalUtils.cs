using CompilerComp442.Src.Model;
using System.Collections.Generic;

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

        public static TokenType? GetTokenTypeForReservedWord(string text)
        {
            switch (text)
            {
                case "if":
                    return TokenType.ifKeyword;
                case "then":
                    return TokenType.thenKeyword;
                case "else":
                    return TokenType.elseKeyword;
                case "integer":
                    return TokenType.integerKeyword;
                case "float":
                    return TokenType.floatKeyword;
                case "void":
                    return TokenType.voidKeyword;
                case "public":
                    return TokenType.publicKeyword;
                case "private":
                    return TokenType.privateKeyword;
                case "func":
                    return TokenType.funcKeyword;
                case "var":
                    return TokenType.varKeyword;
                case "struct":
                    return TokenType.structKeyword;
                case "while":
                    return TokenType.whileKeyword;
                case "read":
                    return TokenType.readKeyword;
                case "write":
                    return TokenType.writeKeyword;
                case "return":
                    return TokenType.returnKeyword;
                case "self":
                    return TokenType.selfKeyword;
                case "inherits":
                    return TokenType.inheritsKeyword;
                case "let":
                    return TokenType.letKeyword;
                case "impl":
                    return TokenType.implKeyword;
                default:
                    return null;
            }  
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

        // use to simplify?????
        public static bool IsAlphaNum(char character)
        {
            // letter or digit or underscore
            return IsLetter(character) || IsDigit(character) || character.Equals('_');
        }
    }
}
