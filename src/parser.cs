using System.Collections.Generic;

namespace Emerld
{
  using ProgramTree = List<ClassTree>;
  using TokenTree = Tree<Token>;

  static class Parser
  {
    private static List<Token> tokens;
    private static int objectiveCurTokIndex = 0;
    
    public static void SETTOKS(List<Token>t){tokens=t;}
    public static ProgramTree MakeFromTokens(List<Token> toks)
    {
      Parser.tokens = toks;
      ProgramTree programTree = new ProgramTree();
      ClassTree cl;
      while(tokens.Count > 0)
      {
        cl = ScanForClass();
        programTree.Add(cl);
      }
      return programTree;
    }
    private static ClassTree ScanForClass()
    {
      Util.WriteLine("$$$SCANNIGNG CLASS$$$");
      if(tokens[0].type != TokenType.ID)
        Errors.InvalidTokenAtPosition(tokens[0], 0);
      else if(tokens[1].type != TokenType.BRACE_OPEN)
        Errors.InvalidTokenAtPosition(tokens[1], 1);
      
      ClassTree ct = new ClassTree(tokens[0].value);
      tokens.RemoveAt(0); // ID
      tokens.RemoveAt(0); // {
      objectiveCurTokIndex += 2;
      
      int curTokIndex = 0;
      Token curTok = tokens[curTokIndex];
      int bracesBalance = 1;
      while(curTokIndex < tokens.Count)
      {
        curTok = tokens[0];
        
        if(curTok.type == TokenType.BRACE_OPEN)
          Errors.InvalidTokenAtPosition(curTok, curTokIndex);
        else if(curTok.type == TokenType.BRACE_CLOSE)
          --bracesBalance;
        
        if(bracesBalance <= 0)
          break;
        
        FunctionTree ft = ScanForFunction();
        ct.Add(ft);
      }
      tokens.RemoveAt(0); // }
      return ct;
    }
    public static FunctionTree ScanForFunction()
    {
      Util.WriteLine("---SCANING FUNCTION---");
      if(tokens[0].type != TokenType.ID)
        Errors.InvalidTokenAtPosition(tokens[0], 0);
      else if(tokens[1].type != TokenType.BRACE_OPEN)
        Errors.InvalidTokenAtPosition(tokens[1], 1);
      
      FunctionTree ft = new FunctionTree(tokens[0].value);
        Util.Write("    Tokens: ");
        foreach(Token t in tokens)Util.Write(t.value+" ");
        Util.WriteLine("");
      tokens.RemoveAt(0); // ID
      tokens.RemoveAt(0); // {
      objectiveCurTokIndex += 2;
      
      int curTokIndex = 0;
      Token curTok = tokens[curTokIndex];
      while(curTokIndex < tokens.Count)
      {
        Util.Write("    Tokens: ");
        foreach(Token t in tokens)Util.Write(t.value+" ");
        Util.WriteLine("");
        
        curTok = tokens[0];

        if(curTok.type == TokenType.BRACE_OPEN)
          Errors.InvalidTokenAtPosition(curTok, curTokIndex);
        else if(curTok.type == TokenType.BRACE_CLOSE)
          break;
        
        TokenTree tt = ScanForStatement();
        ft.Add(tt);
      }
      tokens.RemoveAt(0); // }
      Util.Write("    -Done. Tokens: ");
      foreach(Token t in tokens)Util.Write(t.value+" ");
      Util.WriteLine("");
      return ft;
    }
    public static TokenTree ScanForStatement()
    {
      Util.WriteLine("~~~SCANING STATMENT~~~");
        Util.Write("    Tokens: ");
        foreach(Token t in tokens)Util.Write(t.value+" ");
        Util.WriteLine("");
      
      TokenTree tt = new TokenTree(null);
      int curTokIndex = 0;
      Token curTok = tokens[curTokIndex];
      while(true)
      {
        curTok = tokens[curTokIndex];
        Util.PrintTokenTree(tt);
        switch(curTok.type)
        {
          case TokenType.ID:
          case TokenType.STRING:
          case TokenType.INT:
            if(tt.value == null)
              tt.value = curTok;
            else
              tt.AddChild(curTok);
            break;
          case TokenType.OP:
            TokenTree ttNewOp = new TokenTree(curTok);
            ttNewOp.AddChild(tt);
            tt = ttNewOp;
            break;
          case TokenType.ASSIGN:
            //Navigate to top
            while(tt.parent != null
                  && tt.parent.value.type != TokenType.IF)
              tt = tt.parent;
            
            TokenTree ttNewAssign = new TokenTree(curTok);
            ttNewAssign.AddChild(tt);
            ttNewAssign.AddChild(new TokenTree(null));
            tt = ttNewAssign.GetChild(1);
            break;
          case TokenType.IF:
            //Navigate to top
            while(tt.parent != null)
              tt = tt.parent;
            
            TokenTree ttNewIf = new TokenTree(curTok);
            ttNewIf.AddChild(tt);
            ttNewIf.AddChild(new TokenTree(null));
            tt = ttNewIf.GetChild(1);
            break;
          case TokenType.BRACE_CLOSE:
            if(curTokIndex - 1 > 0)
              tokens.RemoveRange(0, curTokIndex - 1);
            else
              tokens.RemoveAt(0);
            return tt;
          case TokenType.BRACE_OPEN:
          case TokenType.INVALID:
            Errors.InvalidTokenAtPosition(curTok, curTokIndex);
            break;
        }
        ++curTokIndex;
        ++objectiveCurTokIndex;
        Util.WriteLine("Token is: "+curTok);
        
        //If the next token has the same type, leave
        if(curTokIndex >= tokens.Count
        || Util.IsValue(curTok) && Util.IsValue(tokens[curTokIndex]))
          break;
        else if(curTok.type == tokens[curTokIndex].type)
          Errors.InvalidTokenAtPosition(tokens[curTokIndex], curTokIndex);
      }
      
      if(curTokIndex > 0)
        tokens.RemoveRange(0, curTokIndex);
      else
        tokens.RemoveAt(0);
      
      return tt;
    }
  }
}
