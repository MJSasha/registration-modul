using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using RegistrationModul.Services;
using RegistrationModule.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Timers;

namespace RegistrationModul.ViewModels;

public partial class LoginViewModel : ViewModelBase, IDisposable
{
    #region Public props

    [Required]
    [EmailAddress]
    public string Login { get => login; set { this.RaiseAndSetIfChanged(ref login, value); ValidateProperty(value, nameof(Login)); } }
    [Required]
    public string Password { get => password; set { this.RaiseAndSetIfChanged(ref password, value); ValidateProperty(value, nameof(Password)); } }

    public string ErrorMessage { get => errorMessage; set => this.RaiseAndSetIfChanged(ref errorMessage, value); }
    public bool ShowError { get => showError; set => this.RaiseAndSetIfChanged(ref showError, value); }
    public bool IsDevicePermitted { get => devicePermitted; set => this.RaiseAndSetIfChanged(ref devicePermitted, value); }
    public bool IsButtonEnabled { get => isButtonEnabled; set => this.RaiseAndSetIfChanged(ref isButtonEnabled, value); }
    public string UuidTooltip { get => uuidTooltip; set => this.RaiseAndSetIfChanged(ref uuidTooltip, value); }

    #endregion

    #region Private props

    private const int WAIT_TIME_IN_SECONDS = 15;
    private CompaniesService companiesService;

    private string login;
    private string password;

    private bool showError;
    private bool devicePermitted;
    private bool isButtonEnabled = true;
    private string errorMessage;
    private string uuidTooltip;

    private Timer timer;
    private int triesCounter;
    private int currentWaitTimeInSeconds;

    #endregion

    public LoginViewModel(IScreen screen) : base(screen)
    {
        Initialize();
    }

    public LoginViewModel()
    {
        Initialize();
    }

    #region Ralay commands

    [RelayCommand]
    private async Task SubmitButtonClicked()
    {
        triesCounter++;

        var storageService = new AuthService();
        var isUserExist = await storageService.CheckUserExist(Login, Password);

        if (!isUserExist)
        {
            if (triesCounter > 4)
            {
                IsButtonEnabled = false;
                ErrorMessage = $"Exceeded the number of attempts, wait {WAIT_TIME_IN_SECONDS} seconds";
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
        Router.Navigate.Execute(new NotepadViewModel(this));
    }

    [RelayCommand]
    private async Task RegistrationButtonClicked()
    {
        Router.Navigate.Execute(new RegistrationViewModel(this));
    }

    #endregion

    private void Initialize()
    {
        companiesService = new CompaniesService();

        IsDevicePermitted = companiesService.CheckDevicePermitted();
        UuidTooltip = $"Your UUID: {Utils.GetUUID()} (click to copy)";
        ValidateProperties();
        timer = new Timer
        {
            Interval = TimeSpan.FromSeconds(1).TotalMilliseconds
        };
        timer.Elapsed += Timer_Elapsed;
        timer.AutoReset = true;
    }

    private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        currentWaitTimeInSeconds++;

        if (currentWaitTimeInSeconds >= WAIT_TIME_IN_SECONDS)
        {
            timer.Stop();
            triesCounter = 0;
            currentWaitTimeInSeconds = 0;
            IsButtonEnabled = true;
            ShowError = false;
        }
        else
        {
            ErrorMessage = $"Exceeded the number of attempts, wait {WAIT_TIME_IN_SECONDS - currentWaitTimeInSeconds} seconds";
        }
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
