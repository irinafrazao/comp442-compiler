﻿namespace CompilerComp442.Src.Model
{
    // Add here also error types?
    public enum TokenType
    {
        id,
        intNum,
        floatNum, 
        doubleEquals,
        openAndCloseAngledBrackets,
        openAngledBracket,
        closedAngledBracket,
        lessThanOrEquals,
        greaterThanOrEquals,
        plus,
        minus,
        star,
        slash,
        equal,
        pipe,
        ampersand,
        exclamationMark,
        openParenthesis,
        closedParenthesis,
        openCurlyBrace,
        closedCurlyBrace,
        openSquareBracket,
        closedSquareBracket,
        semicolon,
        comma,
        period,
        colon,
        doubleColon,
        arrow,
        ifKeyword,
        thenKeyword,
        elseKeyword,
        integerKeyword,
        floatKeyword,
        voidKeyword,
        publicKeyword,
        privateKeyword,
        funcKeyword,
        varKeyword,
        structKeyword,
        whileKeyword,
        readKeyword,
        writeKeyword,
        returnKeyword,
        selfKeyword,
        inheritsKeyword,
        letKeyword,
        implKeyword
    }
}