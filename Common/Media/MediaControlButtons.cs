using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace Common
{
    namespace MediaControls
    {
        public class BaseMediaButton : Button
        {
            public Grid ShapeContainerGrid { get; private set; }

            public BaseMediaButton()
            {
                ShapeContainerGrid = new Grid();
                this.Background = new LinearGradientBrush(Colors.LightBlue, Colors.White, 45.0);
                                
                //Attempt to make buttons prettier - FAIL....
                //var controlBorderTemplate = new ControlTemplate();
                //var borderFE = new FrameworkElementFactory(typeof(Border));
                //var grtidFE = new FrameworkElementFactory(typeof(Grid));
                //grtidFE.SetValue(Grid.BackgroundProperty, Brushes.Yellow);
                //borderFE.SetValue(Border.CornerRadiusProperty, new CornerRadius(4));
                //borderFE.AppendChild(grtidFE);
                //controlBorderTemplate.VisualTree = borderFE;
                //this.Template = controlBorderTemplate;
                
                this.AddChild(ShapeContainerGrid);
            }
        }

        public class PlayButton : BaseMediaButton
        {
            
            public PlayButton()
            {   
                var s = new Polygon()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    Points = new PointCollection(new List<Point>()
                                                    {
                                                        new Point(2,0),
                                                        new Point(2,16),
                                                        new Point(10,8)
                                                    }),
                    Fill = Brushes.Black,
                };
                
                this.ShapeContainerGrid.Children.Add(s);
            }
        }

        public class StopButton : BaseMediaButton
        {
            public StopButton()
            {
                var s = new Rectangle()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    Width = 12,
                    Height = 12,
                    Fill = Brushes.Black,
                };

                this.ShapeContainerGrid.Children.Add(s);
            }    
        }

        public class RecordButton : BaseMediaButton
        {
            public RecordButton()
            {
                var s = new Ellipse()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    Width = 12,
                    Height = 12,
                    Fill = Brushes.Red,
                };

                this.ShapeContainerGrid.Children.Add(s);
            }
        }

        public class PauseButton : BaseMediaButton
        {
            public PauseButton()
            {
                double centralOffset = 8;
                double stripeWidth = 4;
                double stripeHeight = 12;
                
                var sLeftRect = new Rectangle()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    Width = stripeWidth,
                    Height = stripeHeight,
                    Fill = Brushes.Black,
                    Margin = new Thickness(0, 0, centralOffset, 0),
                };

                var sRightRect = new Rectangle()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    Width = stripeWidth,
                    Height = stripeHeight,
                    Fill = Brushes.Black,
                    Margin = new Thickness(centralOffset, 0, 0, 0),
                };
                
                this.ShapeContainerGrid.Children.Add(sLeftRect);
                this.ShapeContainerGrid.Children.Add(sRightRect);
            }
        }

    }
}
