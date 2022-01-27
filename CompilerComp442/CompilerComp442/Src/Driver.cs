using CompilerComp442.Src.Lexical;
using CompilerComp442.Src.Model;
using System;
using System.IO;

namespace CompilerComp442.Src
{
    public class Driver
    {
        static void Main(string[] args)
        {
            // ASSIGNMENT 1 - tokenize input file using lexical analyzer

            //test case 1 - lexpositivegrading.src
            string inputDirectory = System.IO.Path.GetFullPath(@"..\..\Src\Input\");
            string filepath = Path.Combine(inputDirectory, "lexicaltest2.src");

            string[] textLines = System.IO.File.ReadAllLines(filepath);

            int lineNumber = 1;
            foreach(var line in textLines)
            {
                LexicalAnalyzerResponse response = new LexicalAnalyzerResponse { RemainderOfInputTextLine = line };

                do
                {
                    // send line to lexical analyzer
                    response = LexicalAnalyzer.ExtractNextToken(response.RemainderOfInputTextLine, lineNumber);

                    // write token to file here (good tokens and errors)
                    if (!response.Token.Type.Equals(TokenType.newLineOrTabOrSpaces))
                    {
                        System.Console.WriteLine(response.Token.Type + " => " + response.Token.Lexeme);
                    }

                    //make sure line is entirely tokenized
                } while (!string.IsNullOrEmpty(response.RemainderOfInputTextLine));

                lineNumber++;
            }

            // so the console program doesnt close when its done running
            Console.Read();
        }
    }
}