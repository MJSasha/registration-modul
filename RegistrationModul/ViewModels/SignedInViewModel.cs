using ReactiveUI;
using System;

namespace RegistrationModul.ViewModels
{
    public class SignedInViewModel : MainWindowViewModel, IRoutableViewModel
    {
        public string Test { get; set; } = "Hello";

        public IScreen HostScreen { get; }

        public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];

        public SignedInViewModel(IScreen screen) => HostScreen = screen;
        public SignedInViewModel() { }
    }
}