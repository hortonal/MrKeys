using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;

namespace ScoreControlLibrary
{
    internal class Staffs: List<Staff>
    {
        RenderHelper _renderHelper;

        public Staffs(RenderHelper renderHelper): base()
        {
            _renderHelper = renderHelper;
        }

        public void AddBarLine(double noteTime)
        {

            Line singleBarLine = new Line()
            {
                Y1 = 0,
                Y2 = LowestBarYCoord - HighestBarYCoord,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            _renderHelper.AddItemToRender(noteTime, singleBarLine, HighestBarYCoord, 8, RenderItemType.BarDivision);
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
