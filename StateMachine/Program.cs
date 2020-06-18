using BugTrackerExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachine
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


            // with state pattern

            Context c = new Context();

            // Issue requests, which changes state
            c.Assign("Joe");
            c.Defer();
            c.Assign("Harry");
            c.Assign("Fred");
            c.Close();


            Console.ReadKey(false);
        }
    }
}
