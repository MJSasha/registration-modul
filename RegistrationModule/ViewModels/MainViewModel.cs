using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using RegistrationModul.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace RegistrationModul.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [Required]
    [EmailAddress]
    public string Login { get => login; set { this.RaiseAndSetIfChanged(ref login, value); ValidateProperty(value, nameof(Login)); } }
    [Required]
    public string Password { get => password; set { this.RaiseAndSetIfChanged(ref password, value); ValidateProperty(value, nameof(Password)); } }

    public string ErrorMessage { get => errorMessage; set => this.RaiseAndSetIfChanged(ref errorMessage, value); }
    public bool ShowError { get => showError; set => this.RaiseAndSetIfChanged(ref showError, value); }


    private string login;
    private string password;

    private bool showError;
    private string errorMessage;

    public MainViewModel(IScreen screen) : base(screen)
    {
        ValidateProperties();
    }

    public MainViewModel()
    {
        ValidateProperties();
    }

    [RelayCommand]
    private async Task SubmitButtonClicked()
    {
        var storageService = new AuthService();
        var isUserExist = await storageService.CheckUserExist(Login, Password);

        if (!isUserExist)
        {
            ErrorMessage = "Incorrect login or password or UUID!";
            ShowError = true;
            return;
        }
        Router.Navigate.Execute(new YourWelcomeViewModel(this));
    }

    [RelayCommand]
    private async Task RegistrationButtonClicked()
    {
        Router.Navigate.Execute(new RegistrationViewModel(this));
    }

    private void ValidateProperties()
    {
        ValidateProperty(Login, nameof(Login));
        ValidateProperty(Password, nameof(Password));
    }
}
