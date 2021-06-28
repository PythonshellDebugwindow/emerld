using System;

namespace Emerld
{
  static class Errors
  {
    private static void PrintAndDie(string msg)
    {
      Console.WriteLine("Error: " + msg);
      Environment.Exit(1);
    }
    public static void InvalidToken(string value)
    {
      PrintAndDie("Invalid token `" + value + "`");
    }
    public static void InvalidId(string value)
    {
      PrintAndDie("Invalid ID `" + value + "`");
    }
    public static void ObjectHasNo(string what)
    {
      PrintAndDie("Object has no " + what);
    }
    public static void InvalidTokenAtPosition(Token token, int position)
    {
      PrintAndDie($"Invalid token `{token.value}` at {position}");
    }
    public static void ProgramIsEmpty()
    {
      PrintAndDie("Program is empty");
    }
    public static void InvalidEofAtExpected(int index, string expected)
    {
      PrintAndDie($"Found EOF at {index}, expected `{expected}`");
    }
    public static void NoConstructor()
    {
      PrintAndDie("Object has no constructor (this should not happen)");
    }
    public static void RepeatedClassName(string cl)
    {
      PrintAndDie($"Class name `{cl}` found twice");
    }
    public static void RepeatedFunctionNameInClass(string cl, string fn)
    {
      PrintAndDie($"Function name `{fn}` found twice in class `{cl}`");
    }
    public static void NoMainClass()
    {
      PrintAndDie("No `Main` class");
    }
    public static void NoMainFunction()
    {
      PrintAndDie("No `main` function in `Main` class");
    }
    public static void InvalidTokenInStatement(Token token)
    {
      PrintAndDie($"Invalid token `{token}` in statement");
    }
    public static void InvalidInt(string s)
    {
      PrintAndDie($"`{s}` was supposed to be a valid int, but isn't (probably Emerld's fault)");
    }
    public static void InvalidValue(string s)
    {
      PrintAndDie($"Invalid value: `{s}`");
    }
    public static void NoPropertyNamed(string s)
    {
      PrintAndDie($"Object has no property named {s}");
    }
    public static void NoFunctionNamedInClass(string f, string c)
    {
      PrintAndDie($"Class `{c}` has no function named `{f}`");
    }
    public static void InvalidTokenValueForType(Token token)
    {
      PrintAndDie($"Invalid value `{token.value}` for token of type `{token.type}`");
    }
    public static void ObjectIsNull()
    {
      PrintAndDie("Object is null");
    }
    public static void WrongNumberOfChildren(string type, int expected, int got)
    {
      PrintAndDie($"Wrong number of children for `{type}`: expected {expected}, got {got}");
    }
    public static void CannotAdd(string left, string right)
    {
      PrintAndDie($"Cannot add {left} to {right}");
    }
    public static void CannotSubtract(string left, string right)
    {
      PrintAndDie($"Cannot subtract {right} from {left}");
    }
    public static void TypeIsInvalid(string type, string expected, string got)
    {
      PrintAndDie($"Invalid type for {type}: expected {expected}, got {got}");
    }
    public static void NoClassNamed(string name)
    {
      PrintAndDie($"There is no class named {name}");
    }
    public static void AdditionReturnedNoResult()
    {
      PrintAndDie("Adding two objects returned no result");
    }
    public static void SubtractionReturnedNoResult()
    {
      PrintAndDie("Subtracting two objects returned no result");
    }
    public static void ClassHasNoConstructor(string cl)
    {
      PrintAndDie($"Class `{cl}` has no constructor");
    }
    public static void InvalidFor(string i, string o, string p)
    {
      PrintAndDie($"Invalid {i} for {o} ({p})");
    }
    public static void IntHasNoMethodNamed(string name)
    {
      PrintAndDie($"Int has no method named `{name}`");
    }
    public static void StringHasNoMethodNamed(string name)
    {
      PrintAndDie($"String has no method named `{name}`");
    }
    public static void IdRequiredForAssignment()
    {
      PrintAndDie("An identifier is required for assignment");
    }
    public static void CannotAssignToLiteralValue()
    {
      PrintAndDie("Cannot assign to a class or function name");
    }
  }
}
