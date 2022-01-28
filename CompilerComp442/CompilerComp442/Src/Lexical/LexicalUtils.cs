using CompilerComp442.Src.Model;
using System.Collections.Generic;

namespace CompilerComp442.Src.Lexical
{
    public static class LexicalUtils
    {
        public static bool IsOperatorsOrPunctuation(string text)
        {
            List<string> operatorsOrPunctuation = new List<string>{
                "==", "<>", "<", ">", "<=", ">=", "+", "-",
                "*", "/", "=", "|", "&", "!", "(", ")", "{", "}", "[", "]",
                ";", ",", ".", ":", "::", "->" };

            return operatorsOrPunctuation.Contains(text);
        }

        public static TokenType? GetTokenTypeForOperatorsOrPunctuation(string text)
        {
            switch (text)
            {
                case "==":
                    return TokenType.doubleEquals;
                case "<>":
                    return TokenType.openAndCloseAngledBrackets;
                case "<":
                    return TokenType.openAngledBracket;
                case ">":
                    return TokenType.closedAngledBracket;
                case "<=":
                    return TokenType.lessThanOrEquals;
                case ">=":
                    return TokenType.greaterThanOrEquals;
                case "+":
                    return TokenType.plus;
                case "-":
                    return TokenType.minus;
                case "*":
                    return TokenType.star;
                case "/":
                    return TokenType.slash;
                case "=":
                    return TokenType.equal;
                case "|":
                    return TokenType.pipe;
                case "&":
                    return TokenType.ampersand;
                case "!":
                    return TokenType.exclamationMark;
                case "(":
                    return TokenType.openParenthesis;
                case ")":
                    return TokenType.closedParenthesis;
                case "{":
                    return TokenType.openCurlyBrace;
                case "}":
                    return TokenType.closedCurlyBrace;
                case "[":
                    return TokenType.openSquareBracket;
                case "]":
                    return TokenType.closedSquareBracket;
                case ";":
                    return TokenType.semicolon;
                case ",":
                    return TokenType.comma;
                case ".":
                    return TokenType.period;
                case ":":
                    return TokenType.colon;
                case "::":
                    return TokenType.doubleColon;
                case "->":
                    return TokenType.arrow;
                default:
                    return null;
            }
        }

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
    }
}
