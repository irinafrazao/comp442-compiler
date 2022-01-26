using CompilerComp442.Src.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompilerComp442.Src.Lexical
{
    public static class LexicalAnalyzer
    {
        // still need to add OPERATORS AND PUNCTUATION
        // still need to deal with COMMENTS
        // test floats
        // fixing spaces in middle of lines => just discard them

        public static LexicalAnalyzerResponse ExtractNextToken(string lineOfText, int lineNumber)
        {
            Queue<char> pendingCharacters = new Queue<char>();
            var characterCounter = 0;

            // lines could be just white spaces
            if (string.IsNullOrEmpty(lineOfText) || string.IsNullOrWhiteSpace(lineOfText) || lineOfText.Equals("\t"))
            {
                // final result: new line or tabs or spaces!
                return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.newLineOrTabOrSpaces);
            }

            if (!lineOfText.Last().Equals('$'))
            {
                // used to denote the end of the line
                lineOfText = lineOfText + "$";
            }

            var currentCharacter = lineOfText[characterCounter];

            if (LexicalUtils.IsNonZero(currentCharacter) | currentCharacter.Equals('0'))
            {
                // both states lead to q7 but only the one that start with nonZero needs to check for digits
                if (LexicalUtils.IsNonZero(currentCharacter))
                {
                    while (LexicalUtils.IsDigit(currentCharacter))
                    {
                        pendingCharacters.Enqueue(currentCharacter);
                        characterCounter++;
                        currentCharacter = lineOfText[characterCounter];
                    }
                }
                else
                {
                    // q3: might be an integer or float
                    pendingCharacters.Enqueue(currentCharacter);
                    characterCounter++;
                    currentCharacter = lineOfText[characterCounter];
                }
                
                // q7: might be a float
                if (currentCharacter.Equals('.'))
                {
                    pendingCharacters.Enqueue(currentCharacter);
                    characterCounter++;
                    currentCharacter = lineOfText[characterCounter];

                    while (LexicalUtils.IsDigit(currentCharacter))
                    {
                        pendingCharacters.Enqueue(currentCharacter);
                        characterCounter++;
                        currentCharacter = lineOfText[characterCounter];
                    }

                    if (LexicalUtils.IsNonZero(currentCharacter) | currentCharacter.Equals('0'))
                    {
                        pendingCharacters.Enqueue(currentCharacter);
                        characterCounter++;
                        currentCharacter = lineOfText[characterCounter];

                        if (currentCharacter.Equals('e'))
                        {
                            // q12
                            pendingCharacters.Enqueue(currentCharacter);
                            characterCounter++;
                            currentCharacter = lineOfText[characterCounter];

                            if (currentCharacter.Equals('+') | currentCharacter.Equals('-'))
                            {
                                pendingCharacters.Enqueue(currentCharacter);
                                characterCounter++;
                                currentCharacter = lineOfText[characterCounter];

                                if (currentCharacter.Equals('0'))
                                {
                                    // final result q11: its a float!
                                    return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.floatNum);
                                }
                                else if (LexicalUtils.IsNonZero(currentCharacter))
                                {
                                    pendingCharacters.Enqueue(currentCharacter);
                                    characterCounter++;
                                    currentCharacter = lineOfText[characterCounter];

                                    while (LexicalUtils.IsDigit(currentCharacter))
                                    {
                                        pendingCharacters.Enqueue(currentCharacter);
                                        characterCounter++;
                                        currentCharacter = lineOfText[characterCounter];
                                    }

                                    // final result q11: its a float!
                                    return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter - 1, TokenType.floatNum);
                                }
                                else
                                {
                                    // final result q11: its a float!
                                    return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter - 1, TokenType.floatNum);
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
                            // final result q11: its a float!
                            return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.floatNum);
                        }
                    }
                    else
                    {
                        // final result: its a float!
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
                pendingCharacters.Enqueue(currentCharacter);
                characterCounter++;
                currentCharacter = lineOfText[characterCounter];

                // q4, q5, q6 still might be ids
                while (LexicalUtils.IsLetter(currentCharacter) | LexicalUtils.IsDigit(currentCharacter) | currentCharacter.Equals('_'))
                {
                    pendingCharacters.Enqueue(currentCharacter);
                    characterCounter++;
                    currentCharacter = lineOfText[characterCounter];
                }

                // could be a reserved word
                var strIdOrReservedWord = new string(pendingCharacters.ToArray<char>());
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
                // final result: error invalid character!
                return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.invalidCharacterError);
            }
        }

        private static LexicalAnalyzerResponse BuildResponse(Queue<char> pendingCharacters, int lineNumber,
            string lineOfText, int characterCounter, TokenType tokenType)
        {
            return new LexicalAnalyzerResponse
            {
                Token = new Token
                {
                    Lexeme = new string(pendingCharacters.ToArray<char>()),
                    Type = tokenType,
                    LineNumber = lineNumber
                },
                RemainderOfInputTextLine = lineOfText.Remove(0, characterCounter)
            };
        }
    }
}
