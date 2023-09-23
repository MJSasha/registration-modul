using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace RegistrationModul.ViewModels
{
    public partial class YourWelcomeViewModel : ViewModelBase
    {
        public YourWelcomeViewModel(IScreen screen) : base(screen) { }
        public YourWelcomeViewModel() { }

        [RelayCommand]
        private void Exit()
        {
            Router.Navigate.Execute(new MainViewModel(this));
        }
    }
}
