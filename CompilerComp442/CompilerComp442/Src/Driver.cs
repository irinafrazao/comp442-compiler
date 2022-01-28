using CompilerComp442.Src.Lexical;
using CompilerComp442.Src.Model;
using System.IO;

namespace CompilerComp442.Src
{
    public class Driver
    {
        public static void Main(string[] args)
        {
            // ASSIGNMENT 1 - tokenize input file using lexical analyzer
            PrintFilesFromLexicalAnalyzer("lexpositivegrading");
            PrintFilesFromLexicalAnalyzer("lexnegativegrading");
            PrintFilesFromLexicalAnalyzer("lexicaltest1");
        }

        private static void PrintFilesFromLexicalAnalyzer(string filenameWithoutExtension)
        {
            string inputDirectory = System.IO.Path.GetFullPath(@"..\..\Src\Input\");
            string outputDirectory = System.IO.Path.GetFullPath(@"..\..\Src\Output\");
            string filepath = Path.Combine(inputDirectory, filenameWithoutExtension + ".src");

            string[] textLines = System.IO.File.ReadAllLines(filepath);

            using (StreamWriter fileOutTokens = new StreamWriter(Path.Combine(outputDirectory, filenameWithoutExtension + ".outlextokens")),
                                fileOutErrors = new StreamWriter(Path.Combine(outputDirectory, filenameWithoutExtension + ".outlexerrors")))
            {
                bool newLineTokenFile = false;
                int lineNumber = 1;
                foreach (var line in textLines)
                {
                    LexicalAnalyzerResponse response = new LexicalAnalyzerResponse { RemainderOfInputTextLine = line };

                    do
                    {
                        // send line to lexical analyzer
                        response = LexicalAnalyzer.ExtractNextToken(response.RemainderOfInputTextLine, lineNumber);

                        // write to files
                        if (response != null)
                        {
                            var outputLine = "[" + response.Token.Type + ", " + response.Token.Lexeme
                                + ", " + response.Token.LineNumber + "]";

                            // write errors in both files
                            if (response.Token.Type.Equals(TokenType.invalidCharacterError))
                            {
                                if (newLineTokenFile) fileOutTokens.WriteLine();
                                fileOutTokens.Write(outputLine);
                                    
                                fileOutErrors.WriteLine(outputLine);
                            }
                            // write tokens only in the tokens file
                            else
                            {
                                if (newLineTokenFile) fileOutTokens.WriteLine();
                                fileOutTokens.Write(outputLine);
                            }

                            if (newLineTokenFile) newLineTokenFile = false;
                        }

                        //make sure line is entirely tokenized
                    } while (response != null && !string.IsNullOrEmpty(response.RemainderOfInputTextLine));

                    lineNumber++;
                    newLineTokenFile = true;
                }
            }
        }
    }
}