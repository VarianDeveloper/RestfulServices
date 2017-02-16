using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using EvalServiceLibrary;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** Evaluation Client Application ***\n");

            //EvalServiceClient client =
            //    new EvalServiceClient("BasicHttpBinding_IEvalService");

            WebChannelFactory<IEvalService> cf =
                new WebChannelFactory<IEvalService>(
                    new Uri("http://localhost:8080/evalservice"));
            IEvalService client = cf.CreateChannel();

            Console.WriteLine("Please enter a command: ");
            string command = Console.ReadLine();

            while (!command.Equals("exit"))
            {
                switch (command)
                {
                    case "submit":

                        Console.WriteLine("Please enter your name:");
                        string name = Console.ReadLine();
                        Console.WriteLine("Please enter your comments:");
                        string comments = Console.ReadLine();

                        Eval eval = new Eval();
                        eval.Timesent = DateTime.Now;
                        eval.Submitter = name;
                        eval.Comments = comments;

                        var resp = client.SubmitEval(eval);

                        Console.WriteLine("Evaluation submitted! Response is: " + resp + "\n");
                        break;

                    case "get":
                        Console.WriteLine("Please enter the eval id:");
                        string id = Console.ReadLine();

                        Eval fe = client.GetEval(id);
                        Console.WriteLine("{0} -- {1} said: {2} (id {3})\n", fe.Timesent, fe.Submitter, fe.Comments, fe.Id);
                        break;

                    case "list":

                        Console.WriteLine("Please enter the submitter name:");
                        name = Console.ReadLine();

                        List<Eval> evals = client.GetEvalsBySubmitter(name);

                        evals.ForEach(e => Console.WriteLine("{0} -- {1} said: {2} (id {3})", e.Timesent, e.Submitter, e.Comments, e.Id));
                        Console.WriteLine();
                        break;

                    case "remove":

                        Console.WriteLine("Please enter the eval id:");
                        id = Console.ReadLine();

                        client.RemoveEval(id);

                        Console.WriteLine("Evaluation {0} removed!\n", id);
                        break;

                    default:
                        Console.WriteLine("Unsupported command.");
                        break;
                }

                Console.WriteLine("Please enter a command: ");
                command = Console.ReadLine();
            }
        }
    }
}
