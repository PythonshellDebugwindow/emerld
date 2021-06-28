using System.Collections.Generic;

namespace Emerld
{
  class ClassTree
  {
    public string name;
    public List<FunctionTree> functions;
    
    public ClassTree(string name)
    {
      this.name = name;
      this.functions = new List<FunctionTree>();
    }
    public void Add(FunctionTree ft)
    {
      this.functions.Add(ft);
    }
    public FunctionTree GetFunctionByName(string name)
    {
      foreach(FunctionTree f in this.functions)
        if(f.name == name)
          return f;
      
      if(name == "+")
        return new FunctionTree("+");
      else if(name == "-")
        return new FunctionTree("-");
      else if(name == this.name)
        Errors.ClassHasNoConstructor(this.name);
      else
        Errors.NoFunctionNamedInClass(name, this.name);
      return null;
    }
    public Dictionary<string, FunctionTree> ToDictionary()
    {
      Dictionary<string, FunctionTree> dict;
      dict = new Dictionary<string, FunctionTree>();
      foreach(FunctionTree ft in this.functions)
        dict.Add(ft.name, ft);
      return dict;
    }
  }
}
