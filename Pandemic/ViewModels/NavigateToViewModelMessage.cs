using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.ViewModels
{
    internal class NavigateToViewModelMessage : MessageBase
    {
        public string NavigateTo { get;  }

        internal NavigateToViewModelMessage(string navigateTo)
        {
            NavigateTo = navigateTo;
        }
    }
}
