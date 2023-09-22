using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using RegistrationModul.Models;
using RegistrationModul.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RegistrationModul.ViewModels
{
    public partial class RegistrationViewModel : ViewModelBase, IRoutableViewModel
    {
        public IScreen HostScreen { get; }
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];

        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public string ErrorMessage { get => errorMessage; set => this.RaiseAndSetIfChanged(ref errorMessage, value); }
        public bool ShowError { get => showError; set => this.RaiseAndSetIfChanged(ref showError, value); }

        private bool showError;
        private string errorMessage;


        public RegistrationViewModel(IScreen screen) => HostScreen = screen;
        public RegistrationViewModel() { }

        [RelayCommand]
        private async Task RegistrationButtonClicked()
        {
            if (!CheckAllFiLadsFilled())
            {
                ErrorMessage = "Not all fields are filled!";
                ShowError = true;
                return;
            }

            var storageService = new StorageService();
            var user = new User { Name = Name, Login = Login, Password = Password };

            try
            {
                await storageService.CreateUser(user);
                Router.Navigate.Execute(new YourWelcomeViewModel(this));
            }
            catch (InvalidDataException ex)
            {
                ErrorMessage = ex.Message;
                ShowError = true;
            }
        }

        private bool CheckAllFiLadsFilled()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
        }
    }
}