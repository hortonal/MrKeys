using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ScoreControlLibrary.Glyphs;

namespace ScoreControlLibrary
{
    internal class RenderHelper
    {
        private Canvas _scorePanel;
        private RenderItemsDictionary _renderItemsDictionary;
        public MusicGlyphs Glyphs { get; private set; }

        public RenderHelper(Canvas scorePanel)
        {
            _scorePanel = scorePanel;
            _renderItemsDictionary = new RenderItemsDictionary();
            Glyphs = new MusicGlyphs();
        }

        public double GetHorizontalPositionForNoteTime(double noteTime)
        {
            List<RenderItem> items;
            if(_renderItemsDictionary.TryGetValue(noteTime, out items))
            {
                return items.Where(x => x.ItemType == RenderItemType.Note).Max(x => x.XPosition);
            }
            return 0;
        }

        public void AddItemToRender(double noteTime, FrameworkElement element, double yPosition, double xOffset, RenderItemType itemType)
        {
            element.HorizontalAlignment = HorizontalAlignment.Left;
            element.VerticalAlignment = VerticalAlignment.Top;
            _renderItemsDictionary.Add(noteTime, new RenderItem(element, yPosition, xOffset, itemType));
        }

        public void RenderItemXY(FrameworkElement element, double x, double y)
        {
            element.HorizontalAlignment = HorizontalAlignment.Left;
            element.VerticalAlignment = VerticalAlignment.Top;
            element.Margin = new Thickness(x, y, 0, 0);
            _scorePanel.Children.Add(element);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xStartingOffset">The starting x position to begin rendering</param>
        /// <returns>the last xPosition rendered</returns>
        public double RenderItems(double xStartingOffset)
        {
            double currentX = xStartingOffset;

            //renderItemsDictionary.OrderBy();
            //Step through each NoteTime item
            foreach (double noteTime in _renderItemsDictionary.Keys)
            {
                //Render items by type: BarLines, Clefs, Keys, Timing Signatures, then notes
                currentX += RenderSpecificItems(RenderItemType.BarDivision, noteTime, currentX);
                currentX += RenderSpecificItems(RenderItemType.Clef, noteTime, currentX);
                currentX += RenderSpecificItems(RenderItemType.Key, noteTime, currentX);
                currentX += RenderSpecificItems(RenderItemType.TimeSignature, noteTime, currentX);
                currentX += RenderSpecificItems(RenderItemType.LedgerLine, noteTime, currentX);
                currentX += RenderSpecificItems(RenderItemType.NoteStem, noteTime, currentX);
                currentX += RenderSpecificItems(RenderItemType.Note, noteTime, currentX);
            }
            return currentX;
        }

        private double RenderSpecificItems(RenderItemType type, double noteTime, double currentX)
        {
            
            double xRightOffset = 1000; 
            bool hadItemsToRender = false;
            var items = GetItemsByType(type, _renderItemsDictionary[noteTime]);
            foreach (RenderItem item in items)
            {
                hadItemsToRender = true;
                
                if (item.XOffset < xRightOffset) xRightOffset = item.XOffset;
                //Store the final object horizontal position for later
                item.XPosition = currentX;

                RenderItemXY(item.UIElement, currentX, item.YPosition);                   
            }

            if (hadItemsToRender) return xRightOffset;
            return 0;
        }

        private IEnumerable<RenderItem> GetItemsByType(RenderItemType type, IEnumerable<RenderItem> renderItems)
        {
            return (from item in renderItems
                    where item.ItemType == type
                    select item);
        }
    }
}
