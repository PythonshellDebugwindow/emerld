using System.Collections.Generic;

namespace Emerld
{
  static class Tokenizer
  {
    private static List<Token> tokens;
    private static string curToken;
    
    private static void AddAndResetCurToken()
    {
      if(curToken == "if")
        tokens.Add(new Token(curToken, TokenType.IF));
      else
        tokens.Add(new Token(curToken));
      curToken = "";
    }
    
    public static List<Token> Tokenize(string program)
    {
      tokens = new List<Token>();
      curToken = "";
      bool stringMode = false, wasUsingStringMode = false;
      
      foreach(char curChar in program)
      {
        wasUsingStringMode = stringMode;
        if(curChar == '"' && !stringMode)
          stringMode = true;
        
        if(stringMode)
        {
          if(curToken.Length > 0 && curChar == '"'
             && !wasUsingStringMode)
            AddAndResetCurToken();
          
          curToken += curChar;
          if(curChar == '"' && wasUsingStringMode)
          {
            AddAndResetCurToken();
            stringMode = false;
          }
        }
        else
        {
          if(char.IsWhiteSpace(curChar))
          {
            if(curToken.Length > 0)
              AddAndResetCurToken();
          }
          else if(curToken == ""
            || (Util.GetTokenType(curToken) == TokenType.ID
                && char.IsDigit(curChar))
            || (Util.GetTokenType(curToken) == Util.GetTokenType(curChar)
                && !Util.IsSingle(curToken)))
          {
            curToken += curChar;
          }
          else
          {
            AddAndResetCurToken();
            curToken += curChar;
          }
        }
      }
      if(curToken.Length > 0)
        AddAndResetCurToken();
      
      if(tokens.Count == 0)
        Errors.ProgramIsEmpty();
      
      foreach(Token t in tokens)
      {
        if(t.type == TokenType.INVALID)
          Errors.InvalidToken(t.value);
        else if(t.type == TokenType.ID && !Util.IsValidCompleteEmerldId(t.value))
          Errors.InvalidId(t.value);
      }
      return tokens;
    }
  }
}
