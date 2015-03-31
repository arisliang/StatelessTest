using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StatelessTest.ConsoleUI
{
    public abstract class BaseState
    {
        public virtual void OnEntry(StateMachine<BaseState, Trigger> context)
        {
            Console.WriteLine("Entering {0} state", this.GetType().Name);

            Console.WriteLine("--Is in state {0} {1}", typeof(Connected).Name, context.IsInState(this as Connected));
        }

        public virtual void OnExit(StateMachine<BaseState, Trigger> context)
        {
            Console.WriteLine("Exiting {0} state", this.GetType().Name);

            Console.WriteLine("--Is in state {0} {1}", typeof(Connected).Name, context.IsInState(this as Connected));
        }
    }

    public class OffHook : BaseState
    {

    }

    public class Ringing : BaseState
    {

    }

    public class Connected : BaseState
    {
        public override void OnEntry(StateMachine<BaseState, Trigger> context)
        {
            base.OnEntry(context);

            StartCallTimer();
        }

        public override void OnExit(StateMachine<BaseState, Trigger> context)
        {
            base.OnExit(context);

            StopCallTimer();
        }

        private void StartCallTimer()
        {
            Console.WriteLine("[Timer:] Call started at {0}", DateTime.Now);
            var rnd = new Random();
            var call = rnd.Next(5000);
            Thread.Sleep(call);
        }

        private void StopCallTimer()
        {
            Console.WriteLine("[Timer:] Call ended at {0}", DateTime.Now);
        }
    }

    public class OnHold : Connected
    {
        public override void OnEntry(StateMachine<BaseState, Trigger> context)
        {
            base.OnEntry(context);
        }

        public override void OnExit(StateMachine<BaseState, Trigger> context)
        {
            base.OnExit(context);
        }
    }

    public class PhoneDestroyed : BaseState
    {

    }
}
