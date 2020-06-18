using Stateless;
using Stateless.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachine
{
    public class LeaveRequest
    {
        private enum State { Open, ApprovedLevel1, ApprovedLevel2, Acccepted, Rejected, Cancelled }

        private enum Trigger { Submit, Approve, Reject, Cancel }

        private readonly StateMachine<State, Trigger> _machine;
        // The TriggerWithParameters object is used when a trigger requires a payload.
        //private readonly StateMachine<State, Trigger>.TriggerWithParameters<string> _assignTrigger;

        private readonly int _numberOfDays;
        private string _assignee;

        public LeaveRequest(int numberOfDays)
        {
            _numberOfDays = numberOfDays;

            // Instantiate a new state machine in the Open state
            _machine = new StateMachine<State, Trigger>(State.Open);

            // Instantiate a new trigger with a parameter. 
            //_assignTrigger = _machine.SetTriggerParameters<string>(Trigger.Assign);

            _machine.Configure(State.Open)
                .Permit(Trigger.Submit, State.ApprovedLevel1)
                .Permit(Trigger.Cancel, State.Cancelled);

            _machine.Configure(State.ApprovedLevel1)
                //.SubstateOf(State.Open)
                //.OnEntryFrom(_assignTrigger, OnAssigned)  // This is where the TriggerWithParameters is used. Note that the TriggerWithParameters object is used, not something from the enum
                .PermitIf(Trigger.Approve, State.ApprovedLevel2, () => _numberOfDays > 3)
                .PermitIf(Trigger.Approve, State.Acccepted, () => _numberOfDays <= 3)
                .Permit(Trigger.Reject, State.Rejected);
            //.OnExit(OnDeassigned);

            _machine.Configure(State.ApprovedLevel2)
                //.SubstateOf(State.Open)
                //.OnEntryFrom(_assignTrigger, OnAssigned)  // This is where the TriggerWithParameters is used. Note that the TriggerWithParameters object is used, not something from the enum
                .Permit(Trigger.Approve, State.Acccepted)
                .Permit(Trigger.Reject, State.Rejected);
            //.OnExit(OnDeassigned);

        }

        public void Submit()
        {
            _machine.Fire(Trigger.Submit);
        }

        public void Approve()
        {
            _machine.Fire(Trigger.Approve);
        }

        public void Reject()
        {
            _machine.Fire(Trigger.Reject);
        }

        //public bool CanAssign => _machine.CanFire(Trigger.Assign);

        public void Cancel()
        {
            _machine.Fire(Trigger.Cancel);
        }

        /// <summary>
        /// This method is called automatically when the Assigned state is entered, but only when the trigger is _assignTrigger.
        /// </summary>
        /// <param name="assignee"></param>
        private void OnAssigned(string assignee)
        {
            if (_assignee != null && assignee != _assignee)
                SendEmailToAssignee("Don't forget to help the new employee!");

            _assignee = assignee;
            SendEmailToAssignee("You own it.");
        }
        /// <summary>
        /// This method is called when the state machinie exits the Assigned state
        /// </summary>
        private void OnDeassigned()
        {
            SendEmailToAssignee("You're off the hook.");
        }

        private void SendEmailToAssignee(string message)
        {
            Console.WriteLine("{0}, RE {1}: {2}", _assignee, _numberOfDays, message);
        }

        public string ToDotGraph()
        {
            return UmlDotGraph.Format(_machine.GetInfo());
        }
    }
}
