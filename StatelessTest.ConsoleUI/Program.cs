using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ninject;

namespace StatelessTest.ConsoleUI
{
    class Program
    {
        #region properties
        static OffHook _OffHook
        {
            get
            {
                var state = (OffHook)Container.Kernel.Get<BaseState>(typeof(OffHook).Name);
                return state;
            }
        }
        static Ringing _Ringing
        {
            get
            {
                var state = (Ringing)Container.Kernel.Get<BaseState>(typeof(Ringing).Name);
                return state;
            }
        }
        static Connected _Connected
        {
            get
            {
                var state = (Connected)Container.Kernel.Get<BaseState>(typeof(Connected).Name);
                return state;
            }
        }
        static OnHold _OnHold
        {
            get
            {
                var state = (OnHold)Container.Kernel.Get<BaseState>(typeof(OnHold).Name);
                return state;
            }
        }
        static PhoneDestroyed _PhoneDestroyed
        {
            get
            {
                var state = (PhoneDestroyed)Container.Kernel.Get<BaseState>(typeof(PhoneDestroyed).Name);
                return state;
            }
        }

        #endregion

        static void Main(string[] args)
        {
            //PhoneEnumTest();
            PhoneClassTest();
        }

        static void PhoneEnumTest()
        {
            var phoneCall = new StateMachine<State, Trigger>(State.OffHook);

            phoneCall
                .Configure(State.OffHook)
                .Permit(Trigger.CallDialed, State.Ringing);
            phoneCall
                .Configure(State.Ringing)
                .Permit(Trigger.HungUp, State.OffHook)
                .Permit(Trigger.CallConnected, State.Connected);
            phoneCall
                .Configure(State.Connected)
                .OnEntry(t => StartCallTimer())
                .OnExit(t => StopCallTimer())
                .Permit(Trigger.LeftMessage, State.OffHook)
                .Permit(Trigger.HungUp, State.OffHook)
                .Permit(Trigger.PlacedOnHold, State.OnHold);
            phoneCall.Configure(State.OnHold)
                .SubstateOf(State.Connected)
                .Permit(Trigger.TakenOffHold, State.Connected)
                .Permit(Trigger.HungUp, State.OffHook)
                .Permit(Trigger.PhoneHurledAgainstWall, State.PhoneDestroyed);

            PrintEnum(phoneCall);
            FireEnum(phoneCall, Trigger.CallDialed);

            PrintEnum(phoneCall);
            FireEnum(phoneCall, Trigger.CallConnected);

            PrintEnum(phoneCall);
            FireEnum(phoneCall, Trigger.PlacedOnHold);

            PrintEnum(phoneCall);
            FireEnum(phoneCall, Trigger.TakenOffHold);

            PrintEnum(phoneCall);
            FireEnum(phoneCall, Trigger.HungUp);

            PrintEnum(phoneCall);
            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
        }

        static void PhoneClassTest()
        {
            var phoneCall = new StateMachine<BaseState, Trigger>(_OffHook);

            phoneCall
                .Configure(_OffHook)
                .Permit(Trigger.CallDialed, _Ringing);
            phoneCall
                .Configure(_Ringing)
                .Permit(Trigger.HungUp, _OffHook)
                .Permit(Trigger.CallConnected, _Connected);
            phoneCall
                .Configure(_Connected)
                .OnEntry(t => StartCallTimer())
                .OnExit(t => StopCallTimer())
                .Permit(Trigger.LeftMessage, _OffHook)
                .Permit(Trigger.HungUp, _OffHook)
                .Permit(Trigger.PlacedOnHold, _OnHold);
            phoneCall.Configure(_OnHold)
                .SubstateOf(_Connected)
                .Permit(Trigger.TakenOffHold, _Connected)
                .Permit(Trigger.HungUp, _OffHook)
                .Permit(Trigger.PhoneHurledAgainstWall, _PhoneDestroyed);

            PrintClass(phoneCall);
            FireClass(phoneCall, Trigger.CallDialed);

            PrintClass(phoneCall);
            FireClass(phoneCall, Trigger.CallConnected);

            PrintClass(phoneCall);
            FireClass(phoneCall, Trigger.PlacedOnHold);

            PrintClass(phoneCall);
            FireClass(phoneCall, Trigger.TakenOffHold);

            PrintClass(phoneCall);
            FireClass(phoneCall, Trigger.HungUp);

            PrintClass(phoneCall);
            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
        }

        static void StartCallTimer()
        {
            Console.WriteLine("[Timer:] Call started at {0}", DateTime.Now);
            var rnd = new Random();
            var call = rnd.Next(5000);
            Thread.Sleep(call);
        }

        static void StopCallTimer()
        {
            Console.WriteLine("[Timer:] Call ended at {0}", DateTime.Now);
        }

        static void PrintEnum(StateMachine<State, Trigger> phoneCall)
        {
            //Console.WriteLine("[Status:] {0}", phoneCall);
        }

        static void PrintClass(StateMachine<BaseState, Trigger> phoneCall)
        {
            //Console.WriteLine("[Status:] {0}", phoneCall);
        }

        static void FireEnum(StateMachine<State, Trigger> phoneCall, Trigger trigger)
        {
            Console.WriteLine("[Firing:] {0}", trigger);
            phoneCall.Fire(trigger);
        }

        static void FireClass(StateMachine<BaseState, Trigger> phoneCall, Trigger trigger)
        {
            Console.WriteLine("[Firing:] {0}", trigger);
            phoneCall.Fire(trigger);
        }
    }

    enum Trigger
    {
        CallDialed,
        HungUp,
        CallConnected,
        LeftMessage,
        PlacedOnHold,
        TakenOffHold,
        PhoneHurledAgainstWall
    }

    enum State
    {
        OffHook,
        Ringing,
        Connected,
        OnHold,
        PhoneDestroyed
    }
}
