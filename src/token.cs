namespace Emerld
{
  class Token
  {
    public string value {get; private set;}
    public TokenType type {get; private set;}
    
    public Token(string value)
    {
      this.value = value;
      this.type = Util.GetTokenType(value);
    }
    public Token(int value)
    {
      this.value = value.ToString();
      this.type = TokenType.INT;
    }
    public Token(string value, TokenType type)
    {
      this.value = value;
      this.type = type;
    }

    public override string ToString()
    {
      return "[" + this.type.ToString() + ": " + this.value + "]";
    }
  }
}
