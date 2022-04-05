using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHA3;
using SHA3.Net;
namespace Zad3
{
    
    class Program
    {
        public static Random rand = new Random();
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("The number of objects must be more 3!");
                return;
            }
            if(args.Length%2==0)
            {
                Console.WriteLine("The number of objects must be odd!");
                return;
            }
            for(int i=0;i<args.Length;i++)
            {
                for(int j=i+1;j<args.Length;j++)
                    if(args[i]==args[j])
                    {
                        Console.WriteLine("Object names must not match!");
                        return;
                    }
            }

            string a;
            string key;
            while (true)
            {
                Console.Clear();
                try
                {
                    int step = rand.Next(0, args.Length);
                    while (true)
                    {
                        Console.Clear();
                        key = Shifr.make_shifr(args, step);
                        for (int i = 0; i < args.Length; i++)
                            Console.WriteLine($"{i + 1} - {args[i]}");
                        Console.WriteLine($"0 - exit\n? - help");
                        a = Console.ReadLine();
                        if (a == "0")
                            return;
                        else if (a == "?")
                        {
                            Help.help_menu(args);
                            Console.ReadKey();
                        }
                        else
                        {
                            bool accept = false;
                            for(int i=1;i<=args.Length;i++)
                                if(a == Convert.ToString(i))
                                {
                                    accept = true;
                                    break;
                                }
                            if (accept)
                                break;
                        }
                    }



                    Win.battle(args, step, Convert.ToInt32(a) - 1);
                    Console.WriteLine($"HMAC key: {key}");
                }
                catch { throw; }
                Console.ReadKey();
            }


        }
    }
    class Help
    {
        public static void help_menu(string[] args)
        {
            for (int i = -1; i < args.Length; i++)
            {
                if (i % 2 == 0)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                for (int j = -1; j < args.Length; j++)
                {
                    if (j == -1 && i == -1)
                        Console.Write($"\t");
                    else if (j == -1)
                        Console.Write($"{args[i]}\t");
                    else if (i == -1)
                        Console.Write($"{args[j]}\t");
                    else
                    {
                        int gran;
                        if (i == j)
                        {
                            Console.Write("Draw\t");
                        }
                        else
                        {
                            gran = (args.Length - 1) / 2;
                            for (; gran > 0; gran--)
                                if (i >= gran)
                                {
                                    if (args[i - gran] == args[j])
                                    {
                                        Console.Write("Win\t");
                                        break;
                                    }
                                }
                                else
                                {
                                    if (args[args.Length + i - gran] == args[j])
                                    {
                                        Console.Write("Win\t");
                                        break;
                                    }
                                }
                            if (gran == 0)
                                Console.Write("Lose\t");
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    class Win
    {
        public static void battle(string[] args, int step, int a)
        {
            Console.WriteLine($"Your move: {args[a]}");
            Console.WriteLine($"Computer move: {args[step]}");
            if (a == step)
            {
                Console.WriteLine("Draw!\t");
            }
            else
            {
                int gran;
                gran = (args.Length - 1) / 2;
                for (; gran > 0; gran--)
                    if (a >= gran)
                    {
                        if (args[a - gran] == args[step])
                        {
                            Console.WriteLine("You Win!\t");
                            break;
                        }
                    }
                    else
                    {
                        if (args[args.Length + a - gran] == args[step])
                        {
                            Console.WriteLine("You Win!\t");
                            break;
                        }
                    }
                if (gran == 0)
                    Console.WriteLine("You Lose!\t");
            }
        }
    }
    class Shifr
    {
        private static readonly string alphabet = "0123456789abcdef";
        public static string make_shifr( string[] args, int step)
        {
            string key = "";
            for (int i = 0; i < 64; i++)
            {
                key += alphabet[Program.rand.Next(alphabet.Length)];
            }
            var bytes = Sha3.Sha3256().ComputeHash(Encoding.ASCII.GetBytes(key + args[step]));
            var hex = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            Console.WriteLine($"HMAC: {hex.ToString()}");
            return key; 
        }
    }
}
