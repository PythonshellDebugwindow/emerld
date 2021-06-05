using Emerld;

class MainClass
{
  public static void Main(string[] args)
  {
    string program = @"Example
{
  method
  {
    my$n = 4
    my$n.print
    ""Hello World"".print
    my$methodTwo
  }
  methodTwo
  {
    5.print
    my$x = ""my$n: "" + my$n
    my$x.print
  }
}";
    var toks = Tokenizer.Tokenize(program);
    foreach(Token t in toks)
      System.Console.WriteLine(t);
    System.Console.WriteLine("==========");
    toks = Tokenizer.Tokenize(@"%""Hello there fellow.$""");
    foreach(Token t in toks)
      System.Console.WriteLine(t);
  }
}
