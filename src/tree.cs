using System.Collections.Generic;

namespace Emerld
{
  class Tree<ValueT>
  {
    public List<Tree<ValueT>> children {get; private set;}
    public Tree<ValueT> parent {get; private set;}
    public ValueT value;
    
    public Tree(ValueT value)
    {
      this.children = new List<Tree<ValueT>>();
      this.parent = null;
      this.value = value;
    }
    public void AddChild(Tree<ValueT> tree)
    {
      this.children.Add(tree);
      tree.parent = this;
    }
    public void AddChild(ValueT val)
    {
      Tree<ValueT> child = new Tree<ValueT>(val);
      this.children.Add(child);
      child.parent = this;
    }
    public Tree<ValueT> GetChild(int n)
    {
      if(n >= 0 && n < this.children.Count)
        return this.children[n];
      else
        return null;
    }
    public bool SetChild(int n, Tree<ValueT> tree)
    {
      if(n >= 0 && n < this.children.Count)
      {
        this.children[n] = tree;
        tree.parent = this;
        return true;
      }
      return false;
    }
  }
}
