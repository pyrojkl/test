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
            filePath = Console.ReadLine();
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
            Stack<int> myStack = null;
            Stack<int> start = null;

            int i, end, num1, num2, lvalue;
            string line;

            i = 0;
            end = 0;
            while (end != -1)
            {
                line = file[i];
                // need to add these to parse for the first space:
                // push
                // rvalue
                // lvalue
                // label
                // goto
                // gofalse
                // gotrue
                // show
                // call

                if (line == ":=")
                {
                    // will write num1 to variable at lvalue.
                    num1 = myStack.Pop();
                    lvalue = myStack.Pop();
                }
                else if (line == "pop")
                {
                    myStack.Pop();
                }
                else if (line == "copy")
                {
                    num1 = myStack.Peek();
                    myStack.Push(num1);
                }
                else if (line == "+")
                {
                    num1 = myStack.Pop();
                    num2 = myStack.Pop();
                    myStack.Push(num1 + num2);
                }
                else if (line == "-")
                {
                    num1 = myStack.Pop();
                    num2 = myStack.Pop();
                    myStack.Push(num1 - num2);
                }
                else if (line == "/")
                {
                    num1 = myStack.Pop();
                    num2 = myStack.Pop();
                    myStack.Push(num1 / num2);
                }
                else if (line == "div")
                {
                    num1 = myStack.Pop();
                    num2 = myStack.Pop();
                    myStack.Push(num1 % num2);
                }
                else if (line == "&")
                {
                    num1 = myStack.Pop();
                    num2 = myStack.Pop();
                    myStack.Push(num1 & num2);
                }
                else if (line == "!")
                {
                    num1 = myStack.Pop();
                    if (num1 == 1)
                        myStack.Push(0);
                    else
                        myStack.Push(1);
                }
                else if (line == "|")
                {
                    num1 = myStack.Pop();
                    num2 = myStack.Pop();
                    myStack.Push(num1 | num2);
                }
                else if (line == "<>")
                {
                    num1 = myStack.Pop();
                    num2 = myStack.Pop();
                    myStack.Push(Convert.ToInt32(num1 != num2));
                }
                else if (line == "<=")
                {
                    num1 = myStack.Pop();
                    num2 = myStack.Pop();
                    myStack.Push(Convert.ToInt32(num2 <= num1));
                }
                else if (line == ">=")
                {
                    num1 = myStack.Pop();
                    num2 = myStack.Pop();
                    myStack.Push(Convert.ToInt32(num2 >= num1));
                }
                else if (line == ">")
                {
                    num1 = myStack.Pop();
                    num2 = myStack.Pop();
                    myStack.Push(Convert.ToInt32(num2 > num1));
                }
                else if (line == "<")
                {
                    num1 = myStack.Pop();
                    num2 = myStack.Pop();
                    myStack.Push(Convert.ToInt32(num2 < num1));
                }
                else if (line == "=")
                {
                    num1 = myStack.Pop();
                    num2 = myStack.Pop();
                    myStack.Push(Convert.ToInt32(num2 == num1));
                }
                else if (line == "print")
                {
                    Console.WriteLine(myStack.Peek());
                }
                else if (line == "begin")
                {
                    //Do nothing?
                }
                else if (line == "end")
                {
                    //Do nothing?
                }
                else if (line == "return")
                {
                    i = start.Pop();
                }
                else if (line == "halt")
                {
                    end = -1;
                }
                line = file[i + 1];
            }
        }
    }
}