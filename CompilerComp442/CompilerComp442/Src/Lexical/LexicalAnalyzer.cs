using CompilerComp442.Src.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerComp442.Src.Lexical
{
    public static class LexicalAnalyzer
    {
        public static LexicalAnalyzerResponse ExtractNextToken(string lineOfText, int lineNumber)
        {
            string character = lineOfText.Substring(0, 1);
            lineOfText = lineOfText.Remove(0, 1);

            System.Console.WriteLine(lineOfText);

            Console.Read();




            return new LexicalAnalyzerResponse();
        }
    }
}
