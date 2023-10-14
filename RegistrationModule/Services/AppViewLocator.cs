using ReactiveUI;
using RegistrationModul.ViewModels;
using RegistrationModul.Views;
using System;

namespace RegistrationModul.Services
{
    public class AppViewLocator : IViewLocator
    {
        public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
        {
            LoginViewModel context => new LoginView { DataContext = context },
            RegistrationViewModel context => new RegistrationView { DataContext = context },
            NotepadViewModel context => new NotepadView { DataContext = context },
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
    }
}
