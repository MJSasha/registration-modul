using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using RegistrationModul.Models;
using RegistrationModul.Services;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RegistrationModul.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public string Login { get; set; }
    public string Password { get; set; }

    public string ErrorMessage { get => errorMessage; set => this.RaiseAndSetIfChanged(ref errorMessage, value); }
    public bool ShowError { get => showError; set => this.RaiseAndSetIfChanged(ref showError, value); }

    private bool showError;
    private string errorMessage;

    [RelayCommand]
    private async Task SubmitButtonClicked()
    {
        if (!CheckAllFiLadsFilled())
        {
            ErrorMessage = "Not all fields are filled!";
            ShowError = true;
            return;
        }
        var storageService = new StorageService();
        var isUserExist = await storageService.CheckUserExist(Login, Password);

        if (!isUserExist)
        {
            ErrorMessage = "Incorrect login or password!";
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

    private bool CheckAllFiLadsFilled()
    {
        return !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
    }
}
