using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Common
{
    public class IOC
    {
        private static IUnityContainer _container;
        private static object lockObject = new object();

        public static IUnityContainer Container
        {
            get
            {
                lock (lockObject)
                {
                    if (_container == null)
                    {
                        _container = new UnityContainer();
                    }
                }
                return _container;
            }
        }
    }
}
