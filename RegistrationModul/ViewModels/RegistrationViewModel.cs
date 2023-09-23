using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using RegistrationModul.Models;
using RegistrationModul.Services;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace RegistrationModul.ViewModels
{
    public partial class RegistrationViewModel : ViewModelBase
    {
        [Required]
        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        [Required]
        [EmailAddress]
        public string Login { get => login; set => this.RaiseAndSetIfChanged(ref login, value); }
        [Required]
        [RegularExpression(@"^(?=(?:\D*\d){4})[a-zA-Z\d]{6}$", ErrorMessage = "The password must have 6 characters, 4 of which are digits.")]
        public string Password { get => password; set => this.RaiseAndSetIfChanged(ref password, value); }
        [Required]
        [Phone]
        public string Phone { get => phone; set => this.RaiseAndSetIfChanged(ref phone, value); }
        [Required]
        public string Address { get => address; set => this.RaiseAndSetIfChanged(ref address, value); }

        public string ErrorMessage { get => errorMessage; set => this.RaiseAndSetIfChanged(ref errorMessage, value); }
        public bool ShowError { get => showError; set => this.RaiseAndSetIfChanged(ref showError, value); }

        private string name;
        private string login;
        private string password;
        private string phone;
        private string address;

        private bool showError;
        private string errorMessage;


        public RegistrationViewModel(IScreen screen) : base(screen) { }

        [RelayCommand]
        private async Task RegistrationButtonClicked()
        {
            var storageService = new StorageService();
            var user = new User
            {
                Name = Name,
                Login = Login,
                Password = Password,
                Phone = Phone,
                Address = Address,
            };

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
    }
}