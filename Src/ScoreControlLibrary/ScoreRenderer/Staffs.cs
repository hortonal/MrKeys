using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;

namespace ScoreControlLibrary.ScoreRenderer
{
    internal enum BarLineStyle { Light, LightHeavy }

    internal class Staffs: List<Staff>
    {
        RenderHelper _renderHelper;

        public Staffs(RenderHelper renderHelper): base()
        {
            _renderHelper = renderHelper;
        }

        public void AddBarLine(double noteTime, BarLineStyle barLineStyle = BarLineStyle.Light)
        {

            double xLineOffset = 0;
            double xPostBarLineOffset = ScoreLayoutDetails.LineSpacing_Y;

            if (barLineStyle == BarLineStyle.LightHeavy)
            {
                xLineOffset = -ScoreLayoutDetails.LineSpacing_Y / 2;
                xPostBarLineOffset = 0;
            }
            
            Line singleBarLine = new Line()
            {
                Y1 = 0,
                Y2 = LowestBarYCoord - HighestBarYCoord,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                X1 = xLineOffset,
                X2 = xLineOffset
            };

            _renderHelper.AddItemToRender(noteTime, singleBarLine, HighestBarYCoord, xPostBarLineOffset, 0, RenderItemType.BarDivision);

            if(barLineStyle == BarLineStyle.LightHeavy)
            {
                singleBarLine = new Line()
                {
                    Y1 = 0,
                    Y2 = LowestBarYCoord - HighestBarYCoord,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };

                _renderHelper.AddItemToRender(noteTime, singleBarLine, HighestBarYCoord, 0, 0, RenderItemType.BarDivision);

            }
        }

        private double LowestBarYCoord
        {
            get
            {
                return (from staff in this
                        orderby staff.LowestLine_Y
                        select staff.LowestLine_Y).LastOrDefault();
            }
        }

        private double HighestBarYCoord
        {
            get
            {
                return (from staff in this
                        orderby staff.HighestLine_Y
                        select staff.HighestLine_Y).FirstOrDefault();
            }
        }
        
    }
}
