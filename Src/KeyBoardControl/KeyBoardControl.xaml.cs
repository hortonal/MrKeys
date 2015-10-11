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
using Common.Events;
using Microsoft.Practices.Unity;

namespace KeyBoardControlLibrary
{
    /// <summary>
    /// Interaction logic for KeyBoardControl.xaml
    /// </summary>
    public partial class KeyBoardControl : UserControl
    {
        private IVirtualKeyBoard _keyboard;
        public KeyBoardControl(IVirtualKeyBoard keyBoard): this()
        {
            keyBoard.DrawKeys(KeyBoardCanvas);

            _keyboard = keyBoard;


            var window = Application.Current.MainWindow;
            //Hookup keyboard presses
            window.KeyDown += (o, e) => _keyboard.HandleKeyboardPress(o, e);
            window.KeyUp += (o, e) => _keyboard.HandleKeyboardPress(o, e);
        }
        
        public KeyBoardControl()
        {
            InitializeComponent();
        }
    }
}
