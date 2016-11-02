using System;
using System.AddIn.Hosting;
using System.Collections.ObjectModel;
using Calc1HostView;

namespace Calc1Host
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Assume that the current directory is the application folder,
            // and that it contains the pipeline folder structure.
            var addInRoot = Environment.CurrentDirectory + "\\Pipeline";

            // Update the cache files of the pipeline segments and add-ins.
            var warnings = AddInStore.Update(addInRoot);
            foreach (var warning in warnings)
            {
                Console.WriteLine(warning);
            }

            // Search for add-ins of type ICalculator (the host view of the add-in).
            var tokens = AddInStore.FindAddIns(typeof (ICalculator), addInRoot);

            // Ask the user which add-in they would like to use.
            var calcToken = ChooseCalculator(tokens);

            // Activate the selected AddInToken in a new application domain 
            // with the Internet trust level.
            var calc = calcToken.Activate<ICalculator>(AddInSecurityLevel.Internet);

            // Run the add-in.
            RunCalculator(calc);
        }

        private static AddInToken ChooseCalculator(Collection<AddInToken> tokens)
        {
            if (tokens.Count == 0)
            {
                Console.WriteLine("No calculators are available");
                return null;
            }

            Console.WriteLine("Available Calculators: ");
            // Show the token properties for each token in the AddInToken collection
            // (tokens), preceded by the add-in number in [] brackets.
            var tokNumber = 1;
            foreach (var token in tokens)
            {
                Console.WriteLine("\t[{0}]: {1} - {2}\n\t{3}\n\t\t {4}\n\t\t {5} - {6}", tokNumber, token.Name,
                    token.AddInFullName, token.AssemblyName, token.Description, token.Version, token.Publisher);
                tokNumber++;
            }

            Console.WriteLine("Which calculator do you want to use?");
            var line = Console.ReadLine();
            int selection;
            if (Int32.TryParse(line, out selection))
            {
                if (selection <= tokens.Count)
                    return tokens[selection - 1];
            }

            Console.WriteLine("Invalid selection : {0}. Please choose again.", line);
            return ChooseCalculator(tokens);
        }

        private static void RunCalculator(ICalculator calc)
        {
            if (calc == null)
            {
                // No Calculators were found; read a line and exit.
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Available operations: +, -, *, /");
            Console.WriteLine("Request a calculation, such as : 2 + 2");
            Console.WriteLine("Type \"exit\" to exit");
            var line = Console.ReadLine();

            while (line != null && !line.Equals("exit"))
            {
                // The Parser class parses the user's input.
                try
                {
                    var c = new Parser(line);
                    switch (c.Action)
                    {
                        case "+":
                            Console.WriteLine(calc.Add(c.A, c.B));
                            break;
                        case "-":
                            Console.WriteLine(calc.Subtract(c.A, c.B));
                            break;
                        case "*":
                            Console.WriteLine(calc.Multiply(c.A, c.B));
                            break;
                        case "/":
                            Console.WriteLine(calc.Divide(c.A, c.B));
                            break;
                        default:
                            Console.WriteLine("{0} is an invalid command. Valid commands are +,-,*,/", c.Action);
                            break;
                    }
                }
                catch
                {
                    Console.WriteLine(
                        "Invalide command: {0}. Commands must be formated: [number] [operation] [number]", line);
                }

                line = Console.ReadLine();
            }
        }
    }

    internal class Parser
    {
        public Parser(string line)
        {
            var parts = line.Split(' ');
            A = double.Parse(parts[0]);
            Action = parts[1];
            B = double.Parse(parts[2]);
        }

        public double B { get; private set; }
        public string Action { get; private set; }
        public double A { get; private set; }
    }
}