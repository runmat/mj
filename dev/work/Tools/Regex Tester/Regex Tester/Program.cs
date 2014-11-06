using System;
using System.Text.RegularExpressions;

namespace Regex_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var regExp1 = new string[] {@"[A-Z]{1,2}[0-9]{1,4}[H]{0,1}", @"[0-9]{1,6}", @"[0-9]{2,4}[A-Z]{1}"};
           
            //Console.WriteLine("Aktuelle RegExp: " + regExp1+", "+regExp2+", "+regExp3);
            var goOn = true;

            do
            {
                Console.WriteLine("zu prüfenden Ausdruck eingeben:");
                var input = Console.ReadLine();

                CheckExpressions(input,regExp1);

               // if (Regex.IsMatch(input, regExp1)) Console.WriteLine("Ausdruck stimmt mit Kennzeichentyp 1 überein.");

                
                 
               //if (Regex.IsMatch(input, regExp3)) Console.WriteLine("Ausdruck stimmt mit Kennzeichentyp 3 überein.");

                Console.WriteLine("Erneut? (j/n)");

                if( Console.ReadKey().Key == ConsoleKey.N)
                goOn = false;

                Console.WriteLine(); 
            } while (goOn);
         
            
        }

        private static void CheckExpressions(string input, string[] Expressions)
        {
            for (int i = 0; i < Expressions.Length;i++)
            {
                var match = Regex.Match(input, Expressions[i]);
            
                do
                {
                    if (match.Value == input)
                    {
                        Console.WriteLine(String.Format("Ausdruck stimmt mit Kennzeichentyp {0} überein.",i+1));
                        break;
                    }
                    else
                    {
                        match = match.NextMatch();
                    }

                } while (match.Success);
            }
        }
    }
}
