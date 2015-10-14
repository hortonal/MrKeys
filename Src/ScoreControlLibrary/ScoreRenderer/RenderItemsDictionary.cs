using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ScoreControlLibrary.ScoreRenderer
{
    
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
