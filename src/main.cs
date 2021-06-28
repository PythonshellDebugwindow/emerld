using System;
using System.IO;
using Emerld;

static class MainClass
{
  public static int Main(string[] args)
  {
    if(args.Length != 1)
    {
      Console.WriteLine("Usage: mono emerld.exe FILE");
      return 1;
    }
    if(!File.Exists(args[0]))
    {
      Console.WriteLine("Usage: mono emerld.exe FILE\nPlease provide a valid FILE.");
      return 1;
    }
    
    string program = File.ReadAllText(args[0]);
    Runner.Run(program, args);
    return 0;
  }
}
