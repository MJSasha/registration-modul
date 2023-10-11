using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using RegistrationModul.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Timers;

namespace RegistrationModul.ViewModels;

public partial class MainViewModel : ViewModelBase, IDisposable
{
    [Required]
    [EmailAddress]
    public string Login { get => login; set { this.RaiseAndSetIfChanged(ref login, value); ValidateProperty(value, nameof(Login)); } }
    [Required]
    public string Password { get => password; set { this.RaiseAndSetIfChanged(ref password, value); ValidateProperty(value, nameof(Password)); } }

    public string ErrorMessage { get => errorMessage; set => this.RaiseAndSetIfChanged(ref errorMessage, value); }
    public bool ShowError { get => showError; set => this.RaiseAndSetIfChanged(ref showError, value); }
    public bool IsButtonEnabled { get => isButtonEnabled; set => this.RaiseAndSetIfChanged(ref isButtonEnabled, value); }


    private string login;
    private string password;

    private bool showError;
    private bool isButtonEnabled = true;
    private string errorMessage;

    private Timer timer;
    private int tryesCounter;

    public MainViewModel(IScreen screen) : base(screen)
    {
        Initialize();
    }

    public MainViewModel()
    {
        Initialize();
    }

    [RelayCommand]
    private async Task SubmitButtonClicked()
    {
        tryesCounter++;

        var storageService = new AuthService();
        var isUserExist = await storageService.CheckUserExist(Login, Password);

        if (!isUserExist)
        {
            if (tryesCounter > 4)
            {
                IsButtonEnabled = false;
                ErrorMessage = "Превышено число попыток, подождите 15 секунд";
                ShowError = true;
                timer.Start();
            }
            else
            {
                ErrorMessage = "Incorrect login or password or UUID!";
                ShowError = true;
            }

            return;
        }
        Router.Navigate.Execute(new YourWelcomeViewModel(this));
    }

    [RelayCommand]
    private async Task RegistrationButtonClicked()
    {
        Router.Navigate.Execute(new RegistrationViewModel(this));
    }

    private void Initialize()
    {
        ValidateProperties();
        timer = new Timer
        {
            Interval = TimeSpan.FromSeconds(15).TotalMilliseconds
        };
        timer.Elapsed += Timer_Elapsed;
        timer.AutoReset = false;
    }

    private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        tryesCounter = 0;
        IsButtonEnabled = true;
        ShowError = false;
    }

    private void ValidateProperties()
    {
        ValidateProperty(Login, nameof(Login));
        ValidateProperty(Password, nameof(Password));
    }

    public void Dispose()
    {
        timer.Dispose();
    }
}
