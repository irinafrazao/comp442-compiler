namespace CompilerComp442.Src.Model
{
    public class LexicalAnalyzerResponse
    {
        public Token Token { get; set; }

        // we need this because one input line can have several tokens
        public string RemainderOfInputTextLine { get; set; }
    }
}
