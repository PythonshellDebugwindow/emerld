using System.Collections.Generic;

namespace Emerld
{
  using StatementList = List<Tree<Token>>;

  class FunctionTree
  {
    public string name;
    public StatementList statements;
    
    public FunctionTree(string name)
    {
      this.name = name;
      this.statements = new StatementList();
    }
    public void Add(Tree<Token> tt)
    {
      this.statements.Add(tt);
    }
  }
}
