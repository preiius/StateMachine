using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachine
{
    /// <summary>
    /// The 'State' abstract class
    /// </summary>
    abstract class State
    {
        // reference to context allows states themselves to pass info back to the context
        // or use context in one of the method calls
        public Context Owner { get; set; }
        public string Assignee { get; set; }

        public abstract void Assign(Context context, string assignee);
        public abstract void Defer(Context context);
        public abstract void Close(Context context);
    }

    /// <summary>
    /// Concrete 'State' classes
    /// </summary>
    class OpenState : State
    {
        public OpenState(Context context)
        {
            this.Owner = context;
            this.Assignee = string.Empty;
        }

        public override void Assign(Context context, string assignee)
        {
            context.State = new AssignedState(context, assignee);
        }

        public override void Defer(Context context) => throw new InvalidOperationException();

        public override void Close(Context context) => throw new InvalidOperationException();
    }

    class AssignedState : State
    {
        public AssignedState(Context context) : this(context, string.Empty)
        {
        }

        public AssignedState(Context context, string assignee)
        {
            this.Owner = context;
            this.Assignee = assignee;
        }

        public override void Assign(Context context, string assignee)
        {
            context.State = new AssignedState(context, assignee);
        }

        public override void Defer(Context context)
        {
            context.State = new DeferredState(context);
        }

        public override void Close(Context context)
        {
            context.State = new ClosedState(context);
        }
    }

    class DeferredState : State
    {
        public DeferredState(Context context)
        {
            this.Owner = context;
            //this.Assignee = state.Assignee;
        }

        public override void Assign(Context context, string assignee)
        {
            context.State = new AssignedState(context, assignee);
        }

        public override void Defer(Context context) => throw new InvalidOperationException();

        public override void Close(Context context) => throw new InvalidOperationException();
    }

    class ClosedState : State
    {
        public ClosedState(Context context)
        {
            this.Owner = context;
            this.Assignee =string.Empty;
        }

        public override void Assign(Context context, string assignee) => throw new InvalidOperationException();

        public override void Defer(Context context) => throw new InvalidOperationException();

        public override void Close(Context context) => throw new InvalidOperationException();
    }

    /// <summary>
    /// The 'Context' class
    /// </summary>
    class Context
    {
        private State _state;

        public Context()
        {
            // open state by default
            this.State = new OpenState(this);
        }

        public State State
        {
            get { return _state; }
            set
            {
                _state = value; 
                Console.WriteLine($"State: {_state.GetType().Name}, Assignee: {_state.Assignee}");
            }
        }

        public void Assign(string assignee)
        {
            _state.Assign(this, assignee);
        }

        public void Defer()
        {
            _state.Defer(this);
        }

        public void Close()
        {
            _state.Close(this);
        }
    }
}
