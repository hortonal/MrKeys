using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
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
                return items.Max(x => x.XPosition);
            }
            return 0;
        }

        public void AddItemToRender(double noteTime, FrameworkElement element, double yPosition, double xOffset, RenderItemType itemType, int pitchId = 0)
        {
            element.HorizontalAlignment = HorizontalAlignment.Left;
            element.VerticalAlignment = VerticalAlignment.Top;
            _renderItemsDictionary.Add(noteTime, new RenderItem(element, yPosition, xOffset, itemType, pitchId));
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
            
            double xRightOffset = ScoreLayoutDetails.LineSpacing_Y * 4; 
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

        internal double GetMaxHorizontalPosition()
        {
            return GetHorizontalPositionForNoteTime(_renderItemsDictionary.Keys.Max());
        }

        private IEnumerable<RenderItem> GetItemsByType(RenderItemType type, IEnumerable<RenderItem> renderItems)
        {
            return (from item in renderItems
                    where item.ItemType == type
                    select item);
        }

        internal void MarkNote(double noteTime, int markForNote, int pitchId)
        {
            List<RenderItem> renderItems;
            if (_renderItemsDictionary.TryGetValue(noteTime, out renderItems))
            {
                foreach(var noteItem in renderItems.Where(r => r.ItemType == RenderItemType.Note && r.PitchId == pitchId))
                {
                    TextBlock tb = noteItem.UIElement as TextBlock;
                    if (tb != null)
                    {
                        byte green = (byte) (255 * markForNote / 10);
                        byte red = (byte)(255 * (10 - markForNote) / 10);

                        SolidColorBrush brush = new SolidColorBrush();
                        brush.Color = Color.FromArgb(255, red, green , 0);
                        
                        tb.Foreground = brush;
                    }                    
                }
            } 
        }

        internal void ResetNoteColours()
        {
            List<RenderItem> renderItems;
            foreach (var noteTime in _renderItemsDictionary.Keys)
                if (_renderItemsDictionary.TryGetValue(noteTime, out renderItems))
                {
                    foreach (var noteItem in renderItems.Where(r => r.ItemType == RenderItemType.Note))
                    {
                        TextBlock tb = noteItem.UIElement as TextBlock;
                        if (tb != null)
                        {
                            tb.Foreground = Brushes.Black;
                        }
                    }
                }
        }
    }
}
