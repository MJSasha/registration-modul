using ReactiveUI;
using System;

namespace RegistrationModul.ViewModels
{
    public class YourWelcomeViewModel : ViewModelBase, IRoutableViewModel
    {
        public IScreen HostScreen { get; }
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];

        public YourWelcomeViewModel(IScreen screen) => HostScreen = screen;
        public YourWelcomeViewModel() { }
    }
}
