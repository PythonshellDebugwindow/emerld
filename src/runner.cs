using System;
using System.Linq;
using System.Collections.Generic;

namespace Emerld
{
  static class Runner
  {
    static List<ClassTree> classes;
    
    public static void Run(string program, string[] args)
    {
      var toks = Tokenizer.Tokenize(program);
      var parsed = Parser.MakeFromTokens(toks);
      Run(parsed, args);
    }
    private static void Run(List<ClassTree> cls, string[] args)
    {
      Util.WriteLine("Running from Main");
      Object.SetProgramArgs(args);
      
      classes = cls;
      
      ValidateClassStructures();
      
      ClassTree mainClass = classes.Where(ct => ct.name == "Main").First();
      FunctionTree mainFunction = mainClass.functions.Where(ft => ft.name == "main").First();
      Object mainObject = new Object();
      Run(mainFunction, mainObject, Object.ProgramObject);
    }

    private static void ValidateClassStructures()
    {
      Util.WriteLine("Validating class structures...");
      
      //No main class/function
      if(!classes.Any(ct => ct.name == "Main"))
        Errors.NoMainClass();
      if(!classes.Any(ct => ct.functions.Any(ft => ft.name == "main")))
        Errors.NoMainFunction();
      
      //Repeated class name
      for(int i = 0; i < classes.Count; ++i)
      {
        for(int j = i + 1; j < classes.Count; ++j)
        {
          if(classes[i].name == classes[j].name)
            Errors.RepeatedClassName(classes[i].name);
        }
        
        //Repeated function name
        List<FunctionTree> fts = classes[i].functions;
        for(int j = 0; j < fts.Count; ++j)
        {
          for(int k = j + 1; k < fts.Count; ++k)
          {
            if(fts[j].name == fts[k].name)
              Errors.RepeatedFunctionNameInClass(classes[i].name, fts[j].name);
          }
        }
      }
      Util.WriteLine("Validated, seems fine");
    }

    public static void Run(FunctionTree ft, Object current, Object caller)
    {
      foreach(Tree<Token> statement in ft.statements)
        Run(statement, current, caller);
    }
    private static Object Run(Tree<Token> statement, Object current, Object caller)
    {
      if(statement.value == null)
        throw new System.ArgumentException("Statement.Value was null!");
      switch(statement.value.type)
      {
        case TokenType.INT:
        case TokenType.STRING:
        case TokenType.ID:
          Token next = statement.parent != null ? statement.parent.value : null;
          return Object.FromValue(statement.value, next, current, caller);
        case TokenType.OP:
          if(statement.children.Count != 2)
          {
            Errors.WrongNumberOfChildren("operation", 2, statement.children.Count);
            return null;
          }
          
          Object left = Run(statement.children[0], current, caller);
          Object right = Run(statement.children[1], current, caller);
          while(left.objs.ContainsKey(""))
            left = left.objs[""];
          while(right.objs.ContainsKey(""))
            right = right.objs[""];
          Util.RPrint(left);
          Util.RPrint(right);
          
          switch(statement.value.value)
          {
            case "+":
              return Object.Add(left, right);
            case "-":
              return Object.Subtract(left, right);
            case ".":
              if(left == null || right == null)
              {
                Errors.ObjectIsNull();
                return null;
              }
              else if(left.literalVal == "io" && right.literalVal == "clear")
              {
                Console.Clear();
                return null;
              }
              else if(left.literalVal == "io" && right.literalVal == "read")
              {
                Token t = new Token($@"""{Console.ReadLine()}""");
                return Object.FromValue(t, null);
              }
              else if(left.literalVal == null)
              {
                if(left.ints.ContainsKey("") || left.strs.ContainsKey(""))
                {
                  bool leftIsInt = left.ints.ContainsKey("");
                  if(right.literalVal != null)
                  {
                    if(right.literalVal == "print")
                    {
                      if(leftIsInt)
                      {
                        Console.WriteLine(left.ints[""]);
                      }
                      else
                      {
                        string lstr = left.strs[""];
                        lstr = lstr.Substring(1);
                        lstr = lstr.Remove(lstr.Length - 1);
                        lstr = lstr.Replace('`', '\"');
                        Console.WriteLine(lstr);
                      }
                      return null;
                    }
                    else
                    {
                      if(leftIsInt)
                        Errors.IntHasNoMethodNamed(right.literalVal);
                      else
                        Errors.StringHasNoMethodNamed(right.literalVal);
                      return null;
                    }
                  }
                  else
                  {
                    string typeR = right.ints.ContainsKey("") ? "int" : "object";
                    if(right.strs.ContainsKey("")) typeR = "str";
                    Errors.TypeIsInvalid("right-hand side of `.`", "literal value", typeR);
                    return null;
                  }
                }
                //Object.?
                if(right.literalVal == null)
                {
                  string typeR = right.ints.ContainsKey("") ? "int" : "object";
                  if(right.strs.ContainsKey("")) typeR = "str";
                  Errors.TypeIsInvalid("right-hand side of `.`", "literal value", typeR);
                  return null;
                }
                //Object.String
                string propName = right.literalVal;
                
                if(left.HasFunction(propName))
                {
                  Run(left.funcs[propName], left, current);
                  return null;
                }
                return left.GetProperty(propName);
              }
              else if(right.literalVal == null)
              {
                string typeR = right.ints.ContainsKey("") ? "int" : "object";
                if(right.strs.ContainsKey("")) typeR = "str";
                Errors.TypeIsInvalid("right-hand side of `.`", "literal value", typeR);
              }
              
              string leftLiteralVal = left.literalVal;
              string rightLiteralVal = right.literalVal;
              
              if(leftLiteralVal == null)
              {
                Errors.InvalidFor("left-hand side", "`.` operator", $"`{left.ToString()}`");
                return null;
              }
              else if(rightLiteralVal == null)
              {
                Errors.InvalidFor("right-hand side", "`.` operator", $"`{right.ToString()}`");
                return null;
              }
              
              ClassTree lct = Util.GetClassByName(classes, leftLiteralVal);
              Object o = Object.NewFromClass(lct, current);
              
              FunctionTree rft = lct.GetFunctionByName(rightLiteralVal);
              Run(rft, o, current);
              return null;
            default:
              Errors.InvalidTokenValueForType(statement.value);
              return null;
          }
        case TokenType.ASSIGN:
          Object aleft = Run(statement.children[0], current, caller);
          Object aright = Run(statement.children[1], current, caller);
          while(aleft.objs.ContainsKey(""))
            aleft = aleft.objs[""];
          while(aright.objs.ContainsKey(""))
            aright = aright.objs[""];
          
          if(aleft.literalVal == null)
          {
            Errors.IdRequiredForAssignment();
            return null;
          }
          string s = Util.GetBaseId(aleft.literalVal);
          if(Util.IdIsMy(aleft.literalVal))
          {
            current.SetProperty(s, aright);
          }
          else if(Util.IdIsYour(aleft.literalVal))
          {
            caller.SetProperty(s, aright);
          }
          else
          {
            Errors.CannotAssignToLiteralValue();
          }
          return null;
        default:
          Errors.InvalidTokenInStatement(statement.value);
          return null;
      }
    }
  }
}
