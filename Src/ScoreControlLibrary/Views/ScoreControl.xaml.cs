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
using MusicXml;

namespace ScoreControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for ScoreControl.xaml
    /// </summary>
    public partial class ScoreControl : UserControl
    {
        public ScoreControl(IUnityContainer container, XScore musicScore): this()
        {
            new ScoreRenderer(musicScore, ScoreGrid).Render();
        }

        public ScoreControl()
        {
            InitializeComponent();
        }
    }
}
