using System;
using System.Windows;

namespace Common.Services
{
    /// <summary>
    /// This is a very bare bones implementation of a Dialog service.
    /// MessageBox is a bummer way to display messages in WPF.  Use TaskDialog or a TaskDialog replacment for a nicer experiece.
    /// You should also have File Open, File Save, Folder Browswer operations.
    /// </summary>
    public class ModalDialogService : IDialogService
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ModalDialogService"/> class.
        /// </summary>
        public ModalDialogService() { }

        #endregion

        #region Methods

        static MessageBoxButton GetButton(DialogButton button)
        {

            switch (button)
            {

                case DialogButton.OK: return MessageBoxButton.OK;

                case DialogButton.OKCancel: return MessageBoxButton.OKCancel;

                case DialogButton.YesNo: return MessageBoxButton.YesNo;

                case DialogButton.YesNoCancel: return MessageBoxButton.YesNoCancel;
            }
            throw new ArgumentOutOfRangeException("button", "Invalid button");
        }

        static MessageBoxImage GetImage(DialogImage image)
        {

            switch (image)
            {

                case DialogImage.Asterisk: return MessageBoxImage.Asterisk;

                case DialogImage.Error: return MessageBoxImage.Error;

                case DialogImage.Exclamation: return MessageBoxImage.Exclamation;

                case DialogImage.Hand: return MessageBoxImage.Hand;

                case DialogImage.Information: return MessageBoxImage.Information;

                case DialogImage.None: return MessageBoxImage.None;

                case DialogImage.Question: return MessageBoxImage.Question;

                case DialogImage.Stop: return MessageBoxImage.Stop;

                case DialogImage.Warning: return MessageBoxImage.Warning;
            }
            throw new ArgumentOutOfRangeException("image", "Invalid image");
        }

        static DialogResponse GetResponse(MessageBoxResult result)
        {

            switch (result)
            {

                case MessageBoxResult.Cancel: return DialogResponse.Cancel;

                case MessageBoxResult.No: return DialogResponse.No;

                case MessageBoxResult.None: return DialogResponse.None;

                case MessageBoxResult.OK: return DialogResponse.OK;

                case MessageBoxResult.Yes: return DialogResponse.Yes;
            }
            throw new ArgumentOutOfRangeException("result", "Invalid result");
        }

        /// <summary>
        /// Shows the exception in a MessageBox.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="image">The image.</param>
        /// <returns><see cref="DialogResponse"/>.OK</returns>
        public DialogResponse ShowException(String message, DialogImage image = DialogImage.Error)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, GetImage(image));
            return DialogResponse.OK;
        }

        /// <summary>
        /// Shows the message in a MessageBox.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="button">The button.</param>
        /// <param name="image">The image.</param>
        /// <returns><see cref="DialogResponse"/></returns>
        public DialogResponse ShowMessage(String message, String caption, DialogButton button, DialogImage image)
        {
            return GetResponse(MessageBox.Show(message, caption, GetButton(button), GetImage(image)));
        }

        #endregion
    }

}
