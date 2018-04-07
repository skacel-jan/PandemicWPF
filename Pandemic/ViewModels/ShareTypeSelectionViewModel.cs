using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Pandemic.ViewModels
{
    public class ShareTypeSelectionViewModel : SelectionViewModel<ShareType>
    {
        public ShareTypeSelectionViewModel(IEnumerable<ShareType> items, Action<ShareType> callbackAction) : base(items, callbackAction)
        {
        }
    }
}