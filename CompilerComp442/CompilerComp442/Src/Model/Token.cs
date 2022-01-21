namespace CompilerComp442.Src.Model
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Lexeme { get; set; }
        public int LineNumber { get; set; }
    }
}
