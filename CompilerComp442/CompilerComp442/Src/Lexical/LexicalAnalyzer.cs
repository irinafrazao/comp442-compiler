using CompilerComp442.Src.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompilerComp442.Src.Lexical
{
    public static class LexicalAnalyzer
    {
        // need to move to a file
        // move line read inside??
        // need to finalize test cases
        // fix spaces and tabs and new lines! (or the 2 files from teacher wont run)
        // still need to deal with COMMENTS

        private static Queue<char> pendingCharacters = new Queue<char>();
        private static int characterCounter = 0;
        private static char currentCharacter;
        private static string lineOfText = "";

        public static LexicalAnalyzerResponse ExtractNextToken(string text, int lineNumber)
        {
            // reset static class fields
            lineOfText = text;
            pendingCharacters = new Queue<char>();
            characterCounter = 0;
            
            // replace tabs by space AND lines could be just white spaces
            lineOfText = lineOfText.Replace("/t", " ");
            if (string.IsNullOrEmpty(lineOfText) || string.IsNullOrWhiteSpace(lineOfText))
            {
                // final result: new line or tabs or spaces!
                return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.newLineOrTabOrSpaces);
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
                        MoveToFollowingCharacter();
                    }
                }
                else
                {
                    // q3: might be an integer or float
                    MoveToFollowingCharacter();
                }
                
                // q7: might be a float
                if (currentCharacter.Equals('.'))
                {
                    MoveToFollowingCharacter();

                    while (LexicalUtils.IsDigit(currentCharacter))
                    {
                        MoveToFollowingCharacter();

                        if (currentCharacter.Equals('0'))
                        {
                            // final result: its a float!
                            return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.floatNum);
                        }
                    }

                    if (currentCharacter.Equals('e'))
                    {
                        // q12
                        MoveToFollowingCharacter();

                        // optionaly floats could have plus or minus after the exponent
                        if (currentCharacter.Equals('+') | currentCharacter.Equals('-'))
                        {
                            MoveToFollowingCharacter();
                        }

                        if (currentCharacter.Equals('0'))
                        {
                            MoveToFollowingCharacter();

                            // final result q11: its a float!
                            return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.floatNum);
                        }
                        else if (LexicalUtils.IsNonZero(currentCharacter))
                        {
                            MoveToFollowingCharacter();

                            while (LexicalUtils.IsDigit(currentCharacter))
                            {
                                MoveToFollowingCharacter();
                            }

                            // final result q11: its a float!
                            return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.floatNum);
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
                    // final result q3: its an integer!
                    return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.intNum);
                }

            }
            else if (LexicalUtils.IsLetter(currentCharacter))
            {
                //q1: might be an id
                MoveToFollowingCharacter();

                // q4, q5, q6 still might be ids
                while (LexicalUtils.IsLetter(currentCharacter) | LexicalUtils.IsDigit(currentCharacter) | currentCharacter.Equals('_'))
                {
                    MoveToFollowingCharacter();
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
                // character is part of an operator or punctuation
                if (LexicalUtils.IsOperatorsOrPunctuation(currentCharacter.ToString()))
                {
                    // might be a 2 character operator or punctuation substring
                    MoveToFollowingCharacter();

                    if (LexicalUtils.IsOperatorsOrPunctuation(currentCharacter.ToString()))
                    {
                        MoveToFollowingCharacter();
                    }

                    // final result: one or two character operator or punctuation!
                    var operatorOrPunctuationType = LexicalUtils.GetTokenTypeForOperatorsOrPunctuation
                        (new string(pendingCharacters.ToArray<char>())).Value;

                    return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, operatorOrPunctuationType);
                }
                else
                {
                    MoveToFollowingCharacter();

                    // final result: error invalid character!
                    return BuildResponse(pendingCharacters, lineNumber, lineOfText, characterCounter, TokenType.invalidCharacterError);
                }
            }
        }

        private static LexicalAnalyzerResponse BuildResponse(Queue<char> pendingCharacters, int lineNumber,
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
                    Lexeme = new string(pendingCharacters.ToArray<char>()),
                    Type = tokenType,
                    LineNumber = lineNumber
                },
                RemainderOfInputTextLine = lineOfText.Remove(0, characterCounter)
            };
        }

        private static void MoveToFollowingCharacter()
        {
            pendingCharacters.Enqueue(currentCharacter);
            characterCounter++;
            currentCharacter = lineOfText[characterCounter];
        }
    }
}
