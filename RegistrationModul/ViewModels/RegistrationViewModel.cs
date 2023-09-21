using ReactiveUI;
using System;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RegistrationModul.ViewModels
{
    public class RegistrationViewModel : ViewModelBase, IRoutableViewModel
    {
        public IScreen HostScreen { get; }
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];

        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public ICommand RegistrationButtonClickedCommand { get; }

        public RegistrationViewModel(IScreen screen) => HostScreen = screen;
        public RegistrationViewModel() { }

        private async Task SubmitButtonClicked()
        {
            
        }
    }
}