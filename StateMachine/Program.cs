using System;
using BugTrackerExample;

namespace StateMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            BugTest();

            Console.ReadKey(false);
        }

        private static void BugTest()
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
        }
    }
}
