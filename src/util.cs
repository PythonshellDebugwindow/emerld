using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
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
      if(File.Exists("emerld.out"))
        File.Delete("emerld.out");
    }
    public static int SubstrCount(string str, string substr)
    {
      return Regex.Matches(str, Regex.Escape(substr)).Count;
    }
    private static bool IsValidEmerldString(string str)
    {
      return EmerldIncompleteStringRegex.IsMatch(str);
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
    
    public static bool IsSingle(string s)
    {
      switch(GetTokenType(s))
      {
        case TokenType.INT:
        case TokenType.STRING:
        case TokenType.ID:
        case TokenType.INVALID:
          return false;
        default:
          return true;
      }
    }
    public static object DoAndReturnNull(Action action)
    {
      action();
      return null;
    }

    public static bool IsValue(Token token)
    {
      switch(token.type)
      {
        case TokenType.INT:
        case TokenType.STRING:
        case TokenType.ID:
          return true;
        default:
          return false;
      }
    }
    
    public static void PrintTokenTree(Tree<Token> tt)
    {
      string v = tt.value != null ? tt.value.ToString() : "null";
      Util.WriteLine("'" + v + "' has " + tt.children.Count + " children");
      foreach(Tree<Token> tc in tt.children)
        PrintTokenTree(tc);
    }

    public static void WriteLine(object o, string path = "emerld.out")
    {
      using(StreamWriter sw = File.AppendText(path))
      {
        sw.WriteLine(o.ToString());
      }
    }
    public static void Write(object o, string path = "emerld.out")
    {
      using(StreamWriter sw = File.AppendText(path))
      {
        sw.Write(o.ToString());
      }
    }
    public static string GetBaseId(string id)
    {
      if(IdIsMy(id) || IdIsYour(id))
        return id.Split('$')[1];
      else
        return id;
    }
    public static bool IdIsMy(string id)
    {
      return id.StartsWith("my$");
    }
    public static bool IdIsYour(string id)
    {
      return id.StartsWith("your$");
    }
    public static ClassTree GetClassByName(List<ClassTree> classes, string name)
    {
      foreach(ClassTree ct in classes)
        if(ct.name == name)
          return ct;
      
      Errors.NoClassNamed(name);
      return null;
    }
    public static void RPrint(Object left)
    {
      if(left.literalVal != null)
        Util.WriteLine("<Literal (" + left.literalVal + ")>");
      else if(left.ints.ContainsKey(""))
        Util.WriteLine("<Int>");
      else if(left.strs.ContainsKey(""))
        Util.WriteLine("<String>");
      else
        Util.WriteLine("<Object>");
      
      foreach(KeyValuePair<string, int> s in left.ints)
        Util.WriteLine(s.Key + " i: " + s.Value);
      foreach(KeyValuePair<string, string> s in left.strs)
        Util.WriteLine(s.Key + " s: " + s.Value);
      foreach(KeyValuePair<string, Object> s in left.objs)
      {
        Util.WriteLine(s.Key+" o: ...");
        Util.RPrint(s.Value);
      }
    }
  }
}
