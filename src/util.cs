using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Emerld
{
  static class Util
  {
    public static Regex EmerldIdRegex {get; private set;}
    public static Regex EmerldIncompleteIdRegex {get; private set;}
    public static Regex EmerldIncompleteStringRegex {get; private set;}
    
    static Util()
    {
      EmerldIdRegex = new Regex(@"^((my|your)\$)?[A-Za-z_][A-Za-z0-9_]*$");
      EmerldIncompleteIdRegex = new Regex(@"^([myour]{2,4}\$?)?([A-Za-z_][A-Za-z0-9_]*)?$");
      EmerldIncompleteStringRegex = new Regex(@"""[^""]*""?");
    }
    public static int SubstrCount(string str, string substr)
    {
      return Regex.Matches(str, Regex.Escape(substr)).Count;
    }
    private static bool IsValidEmerldString(string str)
    {
      return EmerldIncompleteStringRegex.IsMatch(str);
      /*if(str[0] != '"' || str[str.Length - 1] != '"')
        return false;
      
      return str.Count(c => c == '\"') == 2;*/
    }
    public static bool IsValidCompleteEmerldId(string str)
    {
      return EmerldIdRegex.IsMatch(str);
    }
    public static bool IsValidIdChar(char c)
    {
      return (c >= 'A' && c <= 'Z')
          || (c >= 'a' && c <= 'z')
          || (c >= '0' && c <= '9')
          || c == '_' || c == '$';
    }
    public static bool IsOperator(char c)
    {
      return c == '+' || c == '-' || c == '.';
    }
    public static TokenType GetTokenType(string str)
    {
      if(str.All(char.IsDigit))
        return TokenType.INT;
      else if(IsValidEmerldString(str))
        return TokenType.STRING;
      else if(EmerldIncompleteIdRegex.IsMatch(str))
        return TokenType.ID;
      else if(str.Length == 1 && IsOperator(str[0]))
        return TokenType.OP;
      else if(str == "=")
        return TokenType.ASSIGN;
      else if(str == "{")
        return TokenType.BRACE_OPEN;
      else if(str == "}")
        return TokenType.BRACE_CLOSE;
      else
        return TokenType.INVALID;
    }
    public static TokenType GetTokenType(char c)
    {
      if(c >= '0' && c <= '9')
        return TokenType.INT;
      else if(IsValidIdChar(c))
        return TokenType.ID;
      else if(IsOperator(c))
        return TokenType.OP;
      else if(c == '=')
        return TokenType.ASSIGN;
      else if(c == '{')
        return TokenType.BRACE_OPEN;
      else if(c == '}')
        return TokenType.BRACE_CLOSE;
      else
        return TokenType.INVALID;
    }
    
    public static object DoAndReturnNull(Action action)
    {
      action();
      return null;
    }
  }
}
