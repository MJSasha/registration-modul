using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using RegistrationModul.Models;
using RegistrationModul.Services;
using RegistrationModule.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace RegistrationModul.ViewModels
{
    public partial class RegistrationViewModel : ViewModelBase
    {
        [Required]
        public string Name { get => name; set { this.RaiseAndSetIfChanged(ref name, value); ValidateProperty(value, nameof(Name)); } }
        [Required]
        [EmailAddress]
        public string Login { get => login; set { this.RaiseAndSetIfChanged(ref login, value); ValidateProperty(value, nameof(Login)); } }
        [Required]
        [RegularExpression(@"^(?=(?:\D*\d){4})[a-zA-Z\d]{6}$", ErrorMessage = "The password must have 6 characters, 4 of which are digits.")]
        public string Password { get => password; set { this.RaiseAndSetIfChanged(ref password, value); ValidateProperty(value, nameof(Password)); } }
        [Required]
        [Phone]
        public string Phone { get => phone; set { this.RaiseAndSetIfChanged(ref phone, value); ValidateProperty(value, nameof(Phone)); } }
        [Required]
        public string Address { get => address; set { this.RaiseAndSetIfChanged(ref address, value); ValidateProperty(value, nameof(Address)); } }

        public string ErrorMessage { get => errorMessage; set => this.RaiseAndSetIfChanged(ref errorMessage, value); }
        public bool ShowError { get => showError; set => this.RaiseAndSetIfChanged(ref showError, value); }

        private string name;
        private string login;
        private string password;
        private string phone;
        private string address;

        private bool showError;
        private string errorMessage;


        public RegistrationViewModel(IScreen screen) : base(screen)
        {
            ValidateProperties();
        }

        public RegistrationViewModel()
        {
            ValidateProperties();
        }

        [RelayCommand]
        private async Task RegistrationButtonClicked()
        {
            var storageService = new AuthService();
            var salt = Utils.GenerateSalt();
            var user = new User
            {
                Name = Name,
                Login = Login,
                Credentials = new Credentials { Password = Utils.HashPassword(password, salt), Salt = salt },
                Phone = Phone,
                Address = Address,
            };

            try
            {
                await storageService.CreateUser(user);
                Router.Navigate.Execute(new NotepadViewModel(this));
            }
            catch (InvalidDataException ex)
            {
                ErrorMessage = ex.Message;
                ShowError = true;
            }
        }

        [RelayCommand]
        private void Exit()
        {
            Router.Navigate.Execute(new LoginViewModel(this));
        }

        private void ValidateProperties()
        {
            ValidateProperty(Name, nameof(Name));
            ValidateProperty(Login, nameof(Login));
            ValidateProperty(Password, nameof(Password));
            ValidateProperty(Phone, nameof(Phone));
            ValidateProperty(Address, nameof(Address));
        }
    }
}