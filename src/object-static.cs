using System;
using System.Collections.Generic;

namespace Emerld
{
  partial class Object
  {
    public static Object ProgramObject {get; private set;}
    
    static Object()
    {
      ProgramObject = new Object();
    }
    
    public static Object Add(Object left, Object right)
    {
      if(left.ints.ContainsKey(""))
      {
        if(right.ints.ContainsKey(""))
        {
          int sum = left.ints[""] + right.ints[""];
          return Object.FromValue(new Token(sum), null);
        }
        else
        {
          string typeR = right.strs.ContainsKey("") ? "string" : "object";
          Errors.CannotAdd("int", typeR);
          return null;
        }
      }
      else if(left.strs.ContainsKey(""))
      {
        if(right.ints.ContainsKey(""))
        {
          string concat = left.strs[""] + right.ints[""];
          return Object.FromValue(new Token(concat), null);
        }
        else if(right.strs.ContainsKey(""))
        {
          string concat = left.strs[""] + right.strs[""];
          return Object.FromValue(new Token(concat), null);
        }
        else
        {
          Errors.CannotAdd("int", "object");
          return null;
        }
      }
      else
      {
        //Object + ?
        if(right.ints.ContainsKey(""))
        {
          Errors.CannotAdd("object", "int");
          return null;
        }
        else if(right.strs.ContainsKey(""))
        {
          Errors.CannotAdd("object", "string");
          return null;
        }
        else if(left.add == null)
        {
          Errors.ObjectHasNo("`+` (addition) method");
          return null;
        }
        else
        {
          //Object + Object
          Object self = new Object();
          self.SetProperty("rhs", right);
          Runner.Run(left.add, left, self);
          
          if(!self.HasProperty("result"))
          {
            Errors.AdditionReturnedNoResult();
            return null;
          }
          return self.GetProperty("result");
        }
      }
    }
    
    public static Object Subtract(Object left, Object right)
    {
      if(left.ints.ContainsKey(""))
      {
        if(right.ints.ContainsKey(""))
        {
          int diff = left.ints[""] - right.ints[""];
          return Object.FromValue(new Token(diff), null);
        }
        else
        {
          string typeR = right.strs.ContainsKey("") ? "string" : "object";
          Errors.CannotSubtract("int", typeR);
          return null;
        }
      }
      else if(left.strs.ContainsKey(""))
      {
        Errors.CannotSubtract("string", "anything");
        return null;
      }
      else
      {
        //Object - ?
        if(right.ints.ContainsKey(""))
        {
          Errors.CannotSubtract("object", "int");
          return null;
        }
        else if(right.strs.ContainsKey(""))
        {
          Errors.CannotSubtract("object", "string");
          return null;
        }
        else if(left.subtract == null)
        {
          Errors.ObjectHasNo("`-` (subtraction) method");
          return null;
        }
        else
        {
          //Object - Object
          Object self = new Object();
          self.SetProperty("rhs", right);
          Runner.Run(left.subtract, left, self);
          
          if(!self.HasProperty("result"))
          {
            Errors.SubtractionReturnedNoResult();
            return null;
          }
          return self.GetProperty("result");
        }
      }
    }
    public static void SetProgramArgs(string[] args)
    {
      ProgramObject.SetProperty("argc", args.Length);
      for(int i = 0; i < args.Length; ++i)
        ProgramObject.SetProperty("arg" + i, args[i]);
    }
    
    public static Object FromValue(string s, Token nextToken, Object current, Object caller)
    {
      return FromValue(new Token(s), nextToken, current, caller);
    }
    public static Object FromValue(Token token, Token nextToken)
    {
      return FromValue(token, nextToken, null, null);
    }
    public static Object FromValue(Token token, Token nextToken, Object current, Object caller)
    {
      Object o = new Object();
      switch(token.type)
      {
        case TokenType.INT:
          int n;
          if(int.TryParse(token.value, out n))
            o.ints.Add("", n);
          else
            Errors.InvalidInt(token.value);
          break;
        case TokenType.STRING:
          o.strs.Add("", token.value);
          break;
        case TokenType.ID:
          if(nextToken != null && nextToken.type == TokenType.ASSIGN)
          {
            o.literalVal = token.value;
            return o;
          }
          
          if(Util.IdIsMy(token.value))
          {
            string s = Util.GetBaseId(token.value);
            if(current.HasProperty(s))
            {
              Object propVal = current.GetProperty(s);
              if(propVal.objs.ContainsKey(""))
                o.objs.Add("", propVal.objs[""]);
              else
                o.objs.Add("", propVal);
            }
            else if(current.HasFunction(s))
            {
              Runner.Run(current.funcs[s], current, current);
              return null;
            }
          }
          else if(Util.IdIsYour(token.value))
          {
            string s = Util.GetBaseId(token.value);
            if(caller.HasProperty(s))
            {
              Object propVal = caller.GetProperty(s);
              if(propVal.objs.ContainsKey(""))
                o.objs.Add("", propVal.objs[""]);
              else
                o.objs.Add("", propVal);
            }
            else if(caller.HasFunction(s))
            {
              Runner.Run(caller.funcs[s], caller, current);
              return null;
            }
          }
          else
          {
            //Name of function or class
            o.literalVal = token.value;
          }
          break;
        default:
          Errors.InvalidValue(token.type.ToString());
          break;
      }
      return o;
    }

    public static Object NewFromClass(ClassTree cls, Object you)
    {
      Object o = new Object(cls.GetFunctionByName(cls.name),
                            cls.GetFunctionByName("+"),
                            cls.GetFunctionByName("-"));
      Dictionary<string, FunctionTree> cd = cls.ToDictionary();
      foreach(KeyValuePair<string, FunctionTree> kvp in cd)
      {
        o.funcs.Add(kvp.Key, kvp.Value);
      }
      try
      {
        Runner.Run(o.constructor, o, you);
      }
      catch(KeyNotFoundException)
      {
        Errors.NoConstructor();
        Environment.Exit(1);
      }
      return o;
    }
    
    public static Object Property(Object o, string name, Object current, Object caller)
    {
      if(o.ints.ContainsKey(name))
      {
        return Object.FromValue(new Token(o.ints[name]), null);
      }
      else if(o.strs.ContainsKey(name))
      {
        return Object.FromValue(new Token(o.strs[name]), null);
      }
      else if(o.objs.ContainsKey(name))
      {
        return o.objs[name];
      }
      else if(o.funcs.ContainsKey(name))
      {
        Runner.Run(o.funcs[name], current, caller);
        return null;
      }
      else
      {
        Errors.NoPropertyNamed(name);
        return null;
      }
    }
  }
}
