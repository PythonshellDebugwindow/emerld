using System;
using System.Collections.Generic;

namespace Emerld
{
  using OpFunction = Func<Object, Object, Object>;
  
  partial class Object
  {
    readonly FunctionTree constructor;
    readonly FunctionTree add, subtract;
    
    public Dictionary<string, FunctionTree> funcs {get; private set;}
    public Dictionary<string, Object> objs {get; private set;}
    public Dictionary<string, int> ints {get; private set;}
    public Dictionary<string, string> strs {get; private set;}
    
    public string literalVal;
    
    public Object()
    {
      this.constructor = new FunctionTree("");
      this.add = null;
      this.subtract = null;
      
      this.funcs = new Dictionary<string, FunctionTree>();
      this.objs = new Dictionary<string, Object>();
      this.ints = new Dictionary<string, int>();
      this.strs = new Dictionary<string, string>();
      
      this.literalVal = null;
    }

    private Object(FunctionTree constructor,
                   FunctionTree add,
                   FunctionTree subtract) : this()
    {
      this.constructor = constructor;
      this.add = add;
      this.subtract = subtract;
    }
    
    public void SetProperty(string name, int n)
    {
      this.ints[name] = n;
    }
    public void SetProperty(string name, string s)
    {
      this.strs[name] = s;
    }
    public void SetProperty(string name, Object o)
    {
      this.objs[name] = o;
    }
    
    public Object GetProperty(string name)
    {
      Object o = new Object();
      if(this.ints.ContainsKey(name))
        o.ints.Add("", this.ints[name]);
      else if(this.strs.ContainsKey(name))
        o.strs.Add("", this.strs[name]);
      else if(this.objs.ContainsKey(name))
        o.objs.Add("", this.objs[name]);
      else
        Errors.NoPropertyNamed(name);
      return o;
    }
    public bool HasProperty(string name)
    {
      return this.ints.ContainsKey(name)
          || this.strs.ContainsKey(name)
          || this.objs.ContainsKey(name);
    }
    
    public bool HasFunction(string name)
    {
      return this.funcs.ContainsKey(name);
    }
  }
}
