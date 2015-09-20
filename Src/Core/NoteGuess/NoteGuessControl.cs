using Common.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MrKeys.NoteGuess
{
    class NoteGuessControl : ITestControlService
    {

        private readonly UserControl _control;
        private readonly IUnityContainer _container;

        public NoteGuessControl(IUnityContainer container)
        {
            _container = container;
            _control = _container.Resolve<NoteGuess>();
        }
        
        public UserControl Control
        {
            get
            {
                return _control;
            }
        }

        public string Name
        {
            get
            {
                return "Guess The Note";
            }
        }

        public void MarkTest()
        {
            throw new NotImplementedException();
        }

        public void StartTest()
        {
            throw new NotImplementedException();
        }

    }
}
