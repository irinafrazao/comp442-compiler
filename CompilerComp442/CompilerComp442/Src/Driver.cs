using CompilerComp442.Src.Lexical;
using CompilerComp442.Src.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerComp442.Src
{
    public class Driver
    {
        static void Main(string[] args)
        {
            // ASSIGNMENT 1 - tokenize input file using lexical analyzer

            //test case 1 - lexpositivegrading.src
            /* string inputDirectory = System.IO.Path.GetFullPath(@"..\..\Src\Input\");
             string filepath = Path.Combine(inputDirectory, "lexpositivegrading.src");

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
                     //System.Console.WriteLine(response.Token.Lexeme);

                     // this makes sure that the line is entirely tokenized
                 } while (!string.IsNullOrEmpty(response.RemainderOfInputTextLine));

                 lineNumber++;
             }


             // so the console program doesnt close when its done running */

            System.Console.WriteLine(LexicalUtils.IsLetter('a'));
            System.Console.WriteLine(LexicalUtils.IsLetter('z'));
            System.Console.WriteLine(LexicalUtils.IsLetter('A'));
            System.Console.WriteLine(LexicalUtils.IsLetter('Z'));

            System.Console.WriteLine(LexicalUtils.IsDigit('0'));
            System.Console.WriteLine(LexicalUtils.IsDigit('1'));
            System.Console.WriteLine(LexicalUtils.IsDigit('9'));

            Console.Read();
        }
    }
}