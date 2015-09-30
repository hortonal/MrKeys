using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Common.Infrastructure
{

    /// <summary>
    /// Custom Exception Handling
    /// </summary>
    public class Exceptions
    {

        /// <summary>
        /// Main Error Handler
        /// </summary>
        /// <param name="ex">Exception object</param>
        public static void ErrHandler(Exception ex)
        {
            ErrHandler("", ex);
        }

        public static void ErrHandler(string info, Exception ex)
        {
            // Show message to the user
            MessageBox.Show("An error occurred in " + ex.Source + ": " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
        }


    }

}
