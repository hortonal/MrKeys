using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ScoreControlLibrary
{
    public enum RenderItemType { Clef, Key, TimeSignature, Note, NoteStem, Rest, BarDivision, LedgerLine }

    internal class RenderItem
    {
        /// <summary>
        /// X position set only at render time!!
        /// </summary>
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public FrameworkElement UIElement { get; set; }
        public RenderItemType ItemType { get; set; }
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
        public RenderItem(FrameworkElement element, double yPosition, double xOffsetToNextObject, RenderItemType itemType, int pitchId)
        {
            YPosition = yPosition;
            UIElement = element;
            ItemType = itemType;
            XOffset = xOffsetToNextObject;
            PitchId = pitchId;
        }
    }

    internal class RenderItemsDictionary: SortedDictionary<double, List<RenderItem>>
    {
        //Dictionary keeps a list of items rendered at a given X position 
        //i.e. Dictionary<noteTime, List<RenderItems>>
     
        public RenderItemsDictionary(): base()
        {       
        }

        //renderItems.Add(element, noteTime, yPosition, itemType);
        public void Add(double noteTime, RenderItem renderItem)
        {
            List<RenderItem> renderItems;
            //If list doesn't exist, create and add to dictionary.  If it does exist, 
            //we now hold a reference to it.  Add our new item to the list
            if (!this.TryGetValue(noteTime, out renderItems))
            {
                renderItems = new List<RenderItem>();
                this.Add(noteTime, renderItems);
            }

            renderItems.Add(renderItem);
        }

    }
}
