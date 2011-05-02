using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Unity;
using Common.Services;
using Common;
using Common.Media;
using MrKeys;
using MrKeys.Testing;
using Microsoft.Practices.Unity.StaticFactory;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            // Ensure the current culture passed into bindings 
            // is the OS culture. By default, WPF uses en-US 
            // as the culture, regardless of the system settings.

            var container = IOC.Container;

            //Register some basic services to be used by child views later...
            container.RegisterType<IDialogService, ModalDialogService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMediaService, MediaService>(new ContainerControlledLifetimeManager());
            
        }
    }
}
