using System.Windows;
using Microsoft.Practices.Unity;

namespace MrKeys
{
    public partial class App : Application
    {
        IUnityContainer _container;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            //Not sure why this is needed... The MS practices example does it this way...  
            base.OnStartup(e);
            _container = new UnityContainer();

            Bootstrap();
            
            var window = _container.Resolve<MainWindow>();
            window.DataContext = _container.Resolve<MainWindowViewModel>();
            window.Show();
        }

       
    }
}
