using CompilerComp442.Src.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompilerComp442.Src.Lexical
{
    /* Does NOT support:
     * - multi line block comments
     * - nested comments
     */
    public static class LexicalAnalyzer
    {
        private static Stack<char> pendingCharacters = new Stack<char>();
        private static int characterCounter = 0;
        private static char currentCharacter;
        private static string lineOfText = "";

        public static LexicalAnalyzerResponse ExtractNextToken(string text, int lineNumber)
        {
            // reset static class fields
            lineOfText = text;
            pendingCharacters = new Stack<char>();
            characterCounter = 0;
            
            // cleanup tab and leading and trailing white spaces
            lineOfText = lineOfText.Trim();

            // lines could be just white spaces
            if (string.IsNullOrEmpty(lineOfText) || string.IsNullOrWhiteSpace(lineOfText))
            {
                // final result: ignore this line because it was empty
                return null;
            }

            // used to denote the end of the line
            lineOfText = lineOfText + "$";

            // start parsing text
            currentCharacter = lineOfText[characterCounter];

            if (LexicalUtils.IsNonZero(currentCharacter) | currentCharacter.Equals('0'))
            {
                // both states lead to q7 but only the one that start with nonZero needs to check for digits
                if (LexicalUtils.IsNonZero(currentCharacter))
                {
                    while (LexicalUtils.IsDigit(currentCharacter))
                    {
                        StackAndMoveToNextCharacter();
                    }
                }
                else
                {
                    // q3: might be an integer or float
                    StackAndMoveToNextCharacter();
                }
                
                // q7: might be a float
                if (currentCharacter.Equals('.'))
                {
                    StackAndMoveToNextCharacter();

                    if (!LexicalUtils.IsDigit(currentCharacter))
                    {
                        // backtracking 1 character
                        pendingCharacters.Pop();
                        characterCounter--;

                        // final result: its an int
                        return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.intNum);
                    }

                    while (LexicalUtils.IsDigit(currentCharacter))
                    {
                        StackAndMoveToNextCharacter();

                        if (currentCharacter.Equals('0'))
                        {
                            // final result: its a float!
                            return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.floatNum);
                        }
                    }

                    if (currentCharacter.Equals('e'))
                    {
                        // q12
                        StackAndMoveToNextCharacter();

                        // optionaly floats could have plus or minus after the exponent
                        if (currentCharacter.Equals('+') | currentCharacter.Equals('-'))
                        {
                            StackAndMoveToNextCharacter();
                        }

                        if (currentCharacter.Equals('0'))
                        {
                            StackAndMoveToNextCharacter();

                            // final result q11: its a float!
                            return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.floatNum);
                        }
                        else if (LexicalUtils.IsNonZero(currentCharacter))
                        {
                            StackAndMoveToNextCharacter();

                            while (LexicalUtils.IsDigit(currentCharacter))
                            {
                                StackAndMoveToNextCharacter();
                            }

                            // final result q11: its a float!
                            return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.floatNum);
                        }
                        else
                        {
                            // backtracking 1 character
                            pendingCharacters.Pop();
                            characterCounter--;

                            // final result q11: its a float!
                            return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.floatNum);
                        }
                    }
                    else
                    {
                        // final result q11: its a float!
                        return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.floatNum);
                    }
                }
                else
                {
                    // final result q3: its an integer!
                    return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.intNum);
                }

            }
            else if (LexicalUtils.IsLetter(currentCharacter))
            {
                //q1: might be an id
                StackAndMoveToNextCharacter();

                // q4, q5, q6 still might be ids
                while (LexicalUtils.IsLetter(currentCharacter) | LexicalUtils.IsDigit(currentCharacter) | currentCharacter.Equals('_'))
                {
                    StackAndMoveToNextCharacter();
                }

                // could be a reserved word
                var strIdOrReservedWord = MakeStringOutOfCharStack(pendingCharacters);
                if (LexicalUtils.IsReservedWord(strIdOrReservedWord))
                {
                    // final result: its a reserved word!
                    var typeReservedWord = LexicalUtils.GetTokenTypeForReservedWord(strIdOrReservedWord).Value;
                    return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, typeReservedWord);
                }
                else
                {
                    // final result: its an identifier!
                    return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.identifier);
                }
            }
            else
            {
                // character is part of an operator or punctuation
                if (LexicalUtils.IsOperatorsOrPunctuation(currentCharacter.ToString()))
                {
                    StackAndMoveToNextCharacter();

                    var operatorOrPunctuationType = LexicalUtils.GetTokenTypeForOperatorsOrPunctuation
                        (MakeStringOutOfCharStack(pendingCharacters)).Value;

                    // 1 character operator or punctuation!
                    var tempResponse = BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, operatorOrPunctuationType);

                    // might be a 2 character operator or punctuation substring
                    if (LexicalUtils.IsOperatorsOrPunctuation(currentCharacter.ToString()))
                    {
                        StackAndMoveToNextCharacter();

                        var operatorTypeToTest = LexicalUtils.GetTokenTypeForOperatorsOrPunctuation
                            (MakeStringOutOfCharStack(pendingCharacters));

                        if (operatorTypeToTest != null)
                        {
                            // 2 character operator or punctuation!
                            tempResponse = BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, operatorTypeToTest.Value);
                        }
                        else
                        {
                            // could be comments
                            var strToCheck = MakeStringOutOfCharStack(pendingCharacters);

                            // single line comment
                            if (strToCheck.Equals("//"))
                            {
                                while (characterCounter < lineOfText.Length - 1)
                                {
                                    StackAndMoveToNextCharacter();
                                }
                                tempResponse = BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.inlineComment);
                            }
                            else if(strToCheck.Equals("/*"))
                            {
                                while (characterCounter < lineOfText.Length - 1)
                                {
                                    StackAndMoveToNextCharacter();

                                    if (currentCharacter.Equals('*'))
                                    {
                                        StackAndMoveToNextCharacter();

                                        if (currentCharacter.Equals('/'))
                                        {
                                            StackAndMoveToNextCharacter();
                                            tempResponse = BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.blockComment);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // final result: one or two character operator or punctuation!
                    return tempResponse;
                }
                else
                {
                    StackAndMoveToNextCharacter();

                    // final result: error invalid character!
                    return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.invalidCharacterError);
                }
            }
        }

        private static LexicalAnalyzerResponse BuildResponse(Stack<char> pendingCharacters, int lineNumber,
            string lineOfText, int characterCounter, TokenType tokenType)
        {
            // take out the $ that we added previously to denote the end of the string
            if (lineOfText.Contains("$"))
            {
                lineOfText = lineOfText.Remove(lineOfText.Length - 1, 1);
            }
            
            return new LexicalAnalyzerResponse
            {
                Token = new Token
                {
                    Lexeme = MakeStringOutOfCharStack(pendingCharacters),
                    Type = tokenType,
                    LineNumber = lineNumber
                },
                RemainderOfInputTextLine = lineOfText.Remove(0, characterCounter)
            };
        }

        private static string MakeStringOutOfCharStack(Stack<char> characters)
        {
            var charArray = characters.ToArray<char>();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private static void StackAndMoveToNextCharacter()
        {
            pendingCharacters.Push(currentCharacter);
            characterCounter++;
            currentCharacter = lineOfText[characterCounter];
        }
    }
}
