using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages_Manager
{
    class Program
    {
        static void Main(string[] args)
        {
            int capacity = int.Parse(Console.ReadLine());

            string command = string.Empty;
            Dictionary<string, Dictionary<string, int>> Messages = new Dictionary<string, Dictionary<string, int>>();
            
            while (command != "Statistics")
            {
                string[] Command = Console.ReadLine().Split('=').ToArray();
                command = Command[0];
                if (command == "Statistics")
                {
                    break;
                }

                if (command == "Add")
                {
                    if (Messages.ContainsKey(Command[1])) { }
                    else
                    {
                        Messages.Add(Command[1], new Dictionary<string, int>
                        { { "sent", int.Parse(Command[2])}, { "received", int.Parse(Command[3]) } });
                    }
                }
                if (command == "Message")
                {
                    Message(Messages, Command[1], Command[2], capacity);
                }
                if (command == "Empty")
                {
                    if (Command[1] == "All")
                    {
                        Messages.Clear();
                    }
                    if (Messages.ContainsKey(Command[1]))
                    {
                        Messages.Remove(Command[1]);
                    }
                }
            }
            Dictionary<string, int> Ordered = new Dictionary<string, int>();
            Dictionary<string, int> Received = new Dictionary<string, int>();

            foreach (var item in Messages)
            {
                foreach (var value in item.Value)
                {
                    if (value.Key == "sent")
                    {
                        Received.Add(item.Key, value.Value);
                    }
                    else if (value.Key == "received")
                    {
                        Ordered.Add(item.Key, value.Value);
                    }
                }
            }

            Ordered = Ordered
                .OrderByDescending(a => a.Value)
                .ThenBy(a => a.Key)
                .ToDictionary(a => a.Key ,a => a.Value);

            foreach (var item in Received)
            {
                Ordered[item.Key] += item.Value;
            }

            StringBuilder output = new StringBuilder();
            output.AppendLine($"Users count: {Messages.Count}");

            foreach (var item in Ordered)
            {
                output.AppendLine($"{item.Key} - {item.Value}");
            }

            Console.WriteLine(output.ToString().Trim());
        }
        static Dictionary<string, Dictionary<string, int>> Message(Dictionary<string, Dictionary<string, int>> Messages, string sender, string received, int capacity)
        {
            if (Messages.ContainsKey(sender) && Messages.ContainsKey(received))
            {
                Messages[sender]["sent"]++;
                Messages[received]["received"]++;
                if (Messages[sender]["sent"] + Messages[sender]["received"] >= capacity)
                {
                    Console.WriteLine($"{sender} reached the capacity!");
                    Messages.Remove(sender);
                }
                if (Messages[received]["sent"] + Messages[received]["received"] >= capacity)
                {
                    Console.WriteLine($"{received} reached the capacity!");
                    Messages.Remove(received);
                }
            }
            return Messages;
        }
    }
}
