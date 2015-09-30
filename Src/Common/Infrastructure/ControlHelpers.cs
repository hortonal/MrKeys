using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Common.Infrastructure
{
    public class ControlHelpers
    {

        public static bool IsControlLoaded<T>(Panel parentControl, string controlName) where T : FrameworkElement
        {
            foreach (T c in parentControl.Children.OfType<T>())
            {
                if (c.Name == controlName) return true;
            }
            return false;
        }

        public static T GetControl<T>(Panel parentControl, string controlName) where T : FrameworkElement
        {
            foreach (T c in parentControl.Children.OfType<T>())
            {
                if (c.Name == controlName) return c;
            }
            return null;
    }
    }
}
