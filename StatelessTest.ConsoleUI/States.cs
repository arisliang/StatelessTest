using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatelessTest.ConsoleUI
{
    public abstract class BaseState
    {

    }

    public class OffHook : BaseState
    {

    }

    public class Ringing : BaseState
    {

    }

    public class Connected : BaseState
    {

    }

    public class OnHold : BaseState
    {

    }

    public class PhoneDestroyed : BaseState
    {

    }
}
