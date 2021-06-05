using System;

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
}
