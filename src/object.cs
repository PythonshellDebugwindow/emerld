using System;
using System.Collections.Generic;

namespace Emerld
{
  using Function = Action<Object, Object>;
  using OpFunction = Func<Object, Object, Object>;

  class Object
  {
    public const string ObjectConstructorName = "()";
    
    readonly Function constructor;
    readonly OpFunction add, subtract;
    readonly Dictionary<string, Function> funcs;
    readonly Dictionary<string, Object> objs;
    readonly Dictionary<string, int> ints;
    readonly Dictionary<string, string> strs;
    
    public Object()
    {
      this.constructor = (self, caller) => {};
      this.add = (self, arg) => Util.DoAndReturnNull(() => {
        Errors.ObjectHasNo("`+` method");
      }) as Object;
      this.subtract = (self, arg) => Util.DoAndReturnNull(() => {
        Errors.ObjectHasNo("`-` method");
      }) as Object;
      
      this.funcs = new Dictionary<string, Function>();
      this.objs = new Dictionary<string, Object>();
      this.ints = new Dictionary<string, int>();
      this.strs = new Dictionary<string, string>();
    }

    public static Object NewFromClass(Object cls, Object you)
    {
      Object o = new Object();
      foreach(KeyValuePair<string, Function> kvp in cls.funcs)
      {
        o.funcs.Add(kvp.Key, kvp.Value);
      }
      try
      {
        o.funcs[ObjectConstructorName](o, you);
      }
      catch(KeyNotFoundException)
      {
        Console.Error.WriteLine("Error: Object has no constructor (this should not happen)");
        Environment.Exit(1);
      }
      return o;
    }
    
    public void SetProperty(string name, Object o) => this.objs[name] = o;
    public void SetProperty(string name, int n) => this.ints[name] = n;
    public void SetProperty(string name, string s) => this.strs[name] = s;
  }
}
