Main
{
  main
  {
    io.clear
    "GENIUS HOUR:
THE
  ______                     _     _ 
 |  ____|                   | |   | |
 | |__   _ __ ___   ___ _ __| | __| |
 |  __| |  _   _ \ / _ \  __| |/ _  |
 | |____| | | | | |  __/ |  | | (_| |
 |______|_| |_| |_|\___|_|  |_|\____|

PROGRAMMING LANGUAGE

Press ENTER to continue...".print
    io.read
    io.clear
    
    "Emerld is a new programming language.".print
    io.read
    "It's modern, high-level, and object oriented.".print
    io.read
    "It does away with the clutter of most languages.".print
    io.read
    io.clear
    
    "Here is a comparison between Java, an existing language, and Emerld.".print
    io.read
    " ____________________________________________________________
|                                   |                        |
|               JAVA                |         EMERLD         |
|___________________________________|________________________|
|                                   |                        |
| class Example extends Object {    | Example                |
|   public void func(Object your) { | {                      |
|     System.out.print(`Hello!\n`); |    func                |
|   }                               |    {                   |
| }                                 |         `Hello!`.print |
|                                   |    }                   |
|                                   | }                      |
|___________________________________|________________________|
".print
    io.read
    "As you can see, Emerld is often shorter and simpler than languages such as Java.".print

    io.read
    io.clear
    
    "A month ago, the Emerld didn't have any implementations. This meant that it couldn't actually be run.".print
    io.read
    "Since then, however, I have been working hard to make an interpreter for the language.".print
    io.read
    "An interpreter takes as input a program in a language--in this case, Emerld--and executes it.".print
    io.read
    "This presentation itself was written in Emerld.".print
    io.read
    io.clear

    "=================
INTERNAL WORKINGS
=================
".print
    io.read
    "First, a program is read from a file into the interpreter. The file main.em contains the code to print this text.".print
    io.read
    "Then, that program is fed into a tokenizer, which breaks the code into tokens.".print
    io.read
    "Each token has a value (0 for zero, + for addition, etc.), and a type (INT for integers, STRING for strings, OP for operators, etc.). The tokenizer makes a list of these tokens and gives it back to the rest of the interpreter.".print
    io.read
    "After tokenization, the tokens are given to the parser, which assembles them into trees.".print
    io.read
    "That might sound odd, but a tree is a way of representing data where each point of data can have one or more children, which themselves can have children, and so on. Trees are very convenient for interpreters.".print
    io.read
    
    "Then, a runner actually runs the tokens; it takes the trees as input and produces the program's output as output.".print
    io.read
    "This process has allowed this text to reach you.".print
    io.read
    io.clear
    
    "============
CONTINUATION
============".print
    io.read
    
    "I think I will continue this project.".print
    io.read
    "Most of the language currently works, but there are some parts that don't.".print
    io.read
    "I might also extend it further so it can be more usable.".print
    io.read
    "My website, http://masonmackinnon.com, will have more information about Emerld soon. If you're interested, I think you should look at it.".print
    io.read
    io.clear
    
    "=========
THANK YOU
=========".print
    io.read
    "Thank you for listening.".print
    io.read
  }
}
