using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ScoreControlLibrary
{
    internal enum RenderItemType { NoteLine, Clef, Key, TimeSignature, Note, NoteStem, Rest, BarDivision, LedgerLine, Alteration }

    internal class RenderItem
    {
        /// <summary>
        /// X position set only at render time!!
        /// </summary>
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public FrameworkElement UIElement { get; set; }
        public RenderItemType ItemType { get; set; }
        /// <summary>
        /// Suggested space to leave after element before the next one
        /// </summary>
        public double XDistanceToNextObject { get; set; }
        public double XOffset { get; set; }
        /// <summary>
        /// Used for interactive rendering per note
        /// </summary>
        public int PitchId { get; set; }

        /// <summary>
        /// Creates a render item object.  
        /// </summary>
        /// <param name="element"></param>
        /// <param name="yPosition"></param>
        /// <param name="itemType"></param>
        public RenderItem(FrameworkElement element, double yPosition, double xDistanceToNextItem, double xOffset, RenderItemType itemType, int pitchId)
        {
            YPosition = yPosition;
            UIElement = element;
            ItemType = itemType;
            XDistanceToNextObject = xDistanceToNextItem;
            XOffset = xOffset;
            PitchId = pitchId;
        }
    }
}
