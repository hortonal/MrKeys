using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Unity;

namespace MrKeys.NoteGuess
{
    /// <summary>
    /// Interaction logic for NoteGuess.xaml
    /// </summary>
    public partial class NoteGuess : UserControl
    {
        private readonly IUnityContainer _container;

        public NoteGuess(IUnityContainer container)
        {
            _container = container;
            DataContext = _container.Resolve<NoteGuessViewModel>();
            InitializeComponent();
        }

    }
}
