using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatelessTest.ConsoleUI
{
    public class Container
    {
        private static IKernel _Kernel = null;
        public static IKernel Kernel
        {
            get
            {
                if (_Kernel == null)
                {
                    _Kernel = new StandardKernel();

                    _Kernel.Bind<BaseState>().To<OffHook>().InSingletonScope().Named(typeof(OffHook).Name);
                    _Kernel.Bind<BaseState>().To<Ringing>().InSingletonScope().Named(typeof(Ringing).Name);
                    _Kernel.Bind<BaseState>().To<Connected>().InSingletonScope().Named(typeof(Connected).Name);
                    _Kernel.Bind<BaseState>().To<OnHold>().InSingletonScope().Named(typeof(OnHold).Name);
                    _Kernel.Bind<BaseState>().To<PhoneDestroyed>().InSingletonScope().Named(typeof(PhoneDestroyed).Name);
                }

                return _Kernel;
            }
        }
    }
}
