using BugTrackerExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            // with Stateless

            var bug = new Bug("Incorrect stock count");

            bug.Assign("Joe");
            bug.Defer();
            bug.Assign("Harry");
            bug.Assign("Fred");
            bug.Close();

            Console.WriteLine();
            Console.WriteLine("State machine:");
            Console.WriteLine(bug.ToDotGraph());


            Console.ReadKey(false);
        }
    }
}
