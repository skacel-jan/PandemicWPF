namespace Game.Pandemic.ViewModels
{
    internal class NavigateToViewModelMessage
    {
        public string NavigateTo { get; }

        internal NavigateToViewModelMessage(string navigateTo)
        {
            NavigateTo = navigateTo;
        }
    }
}