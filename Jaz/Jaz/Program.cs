using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JazInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath, fileExt;
            string[] myFile;
            Console.WriteLine("Enter the jaz file path:");
            //filePath = Console.ReadLine();
            filePath = "U:\\Jaz\\test.jaz";
            fileExt = Path.GetExtension(filePath);
            if (fileExt == ".jaz")
            {
                myFile = File.ReadAllLines(filePath);
                interpret(myFile);
            }
            else
            {
                Console.WriteLine("ERROR: Incorrect Path or File Extention.");
            }
        }

        public static void interpret(string[] file)
        {
            Stack<int> myStack = new Stack<int>();
            Stack<int> start = new Stack<int>();
            Dictionary<string, int> symbolTable = new Dictionary<string,int>();
            Dictionary<string, int> labels = new Dictionary<string, int>();
            int i, end, num1, num2, loc, pos;
            string line, key, input;

            i = 0;
            foreach (string s in file) //finds all labels for quick searches later when using call/goto
            {
                pos = s.IndexOf(' ');
                if (pos > 0)
                {
                    key = s.Substring(0, pos);
                    if (pos++ < s.Length)
                    {
                        input = s.Substring(pos++);
                        if (key == "label")
                        {
                            labels.Add(input, i);
                        }
                    }
                }
                i++;
            }

            i = 0;
            end = 0;
            while (end != -1 && i < file.Length)
            {
                input = null;
                line = file[i];
                pos = line.IndexOf(' ');
                if (pos > 0) //sets the key
                {
                    key = line.Substring(0, pos);
                    if (pos++ < line.Length)
                        input = line.Substring(pos++);
                }
                else
                {
                    key = line;
                }

                if (!String.IsNullOrEmpty(input)) //these keys require an input and will be skipped if one is not found
                {
                    if (key == "push")
                    {
                        num1 = Convert.ToInt32(input);
                        myStack.Push(num1);
                    }
                    else if (key == "rvalue")
                    {
                        symbolTable.Add(input, 0);
                        myStack.Push(0);
                    }
                    else if (key == "lvalue")
                    {
                        if (!symbolTable.ContainsKey(input))
                            symbolTable.Add(input, 0);
                        num1 = GetIndex(symbolTable, input);
                        myStack.Push(num1);
                    }
                    else if (key == "label")
                    {
                        break;
                    }
                    else if (key == "goto")
                    {
                        i = labels.Where(m => m.Key == input).Single().Value;
                    }
                    else if (key == "gofalse")
                    {
                        if (myStack.Pop() == 0)
                            i = labels.Where(m => m.Key == input).Single().Value;
                    }
                    else if (key == "gotrue")
                    {
                        if (myStack.Pop() != 0)
                            i = labels.Where(m => m.Key == input).Single().Value;
                    }
                    else if (key == "call")
                    {
                        num1 = Convert.ToInt32(input);
                        myStack.Push(num1);
                    }
                    else if (key == "show")
                    {
                        Console.WriteLine(input);
                    }
                    else
                    {
                        Console.WriteLine("Compilation error.");
                        end = -1;
                    }
                }
                else
                {
                    if (key == ":=")
                    {
                        num1 = myStack.Pop();
                        loc = myStack.Pop();
                        input = symbolTable.ElementAt(loc).Key;
                        symbolTable[input] = num1;
                    }
                    else if (key == "pop")
                    {
                        myStack.Pop();
                    }
                    else if (key == "copy")
                    {
                        num1 = myStack.Peek();
                        myStack.Push(num1);
                    }
                    else if (key == "+")
                    {
                        num1 = myStack.Pop();
                        num2 = myStack.Pop();
                        myStack.Push(num1 + num2);
                    }
                    else if (key == "-")
                    {
                        num1 = myStack.Pop();
                        num2 = myStack.Pop();
                        myStack.Push(num1 - num2);
                    }
                    else if (key == "/")
                    {
                        num1 = myStack.Pop();
                        num2 = myStack.Pop();
                        myStack.Push(num1 / num2);
                    }
                    else if (key == "div")
                    {
                        num1 = myStack.Pop();
                        num2 = myStack.Pop();
                        myStack.Push(num1 % num2);
                    }
                    else if (key == "&")
                    {
                        num1 = myStack.Pop();
                        num2 = myStack.Pop();
                        myStack.Push(num1 & num2);
                    }
                    else if (key == "!")
                    {
                        num1 = myStack.Pop();
                        if (num1 == 1)
                            myStack.Push(0);
                        else
                            myStack.Push(1);
                    }
                    else if (key == "|")
                    {
                        num1 = myStack.Pop();
                        num2 = myStack.Pop();
                        myStack.Push(num1 | num2);
                    }
                    else if (key == "<>")
                    {
                        num1 = myStack.Pop();
                        num2 = myStack.Pop();
                        myStack.Push(Convert.ToInt32(num1 != num2));
                    }
                    else if (key == "<=")
                    {
                        num1 = myStack.Pop();
                        num2 = myStack.Pop();
                        myStack.Push(Convert.ToInt32(num2 <= num1));
                    }
                    else if (key == ">=")
                    {
                        num1 = myStack.Pop();
                        num2 = myStack.Pop();
                        myStack.Push(Convert.ToInt32(num2 >= num1));
                    }
                    else if (key == ">")
                    {
                        num1 = myStack.Pop();
                        num2 = myStack.Pop();
                        myStack.Push(Convert.ToInt32(num2 > num1));
                    }
                    else if (key == "<")
                    {
                        num1 = myStack.Pop();
                        num2 = myStack.Pop();
                        myStack.Push(Convert.ToInt32(num2 < num1));
                    }
                    else if (key == "=")
                    {
                        num1 = myStack.Pop();
                        num2 = myStack.Pop();
                        myStack.Push(Convert.ToInt32(num2 == num1));
                    }
                    else if (key == "print")
                    {
                        Console.WriteLine(myStack.Peek());
                    }
                    else if (key == "show")
                    {
                        Console.WriteLine("");
                    }
                    else if (key == "begin")
                    {
                        break;
                    }
                    else if (key == "end")
                    {
                        break;
                    }
                    else if (key == "return")
                    {
                        i = start.Pop();
                    }
                    else if (key == "halt")
                    {
                        end = -1;
                    }
                    else
                    {
                        Console.WriteLine("Compilation error.");
                        end = -1;
                    }
                }
                key = file[i++];
            }
            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        public static int GetIndex(Dictionary<string, int> dictionary, string key)
        {
            for (int i = 0; i < dictionary.Count; i++)
            {
                if (dictionary.ElementAt(i).Key == key)
                    return i;
            }
            return -1;
        }
    }
}