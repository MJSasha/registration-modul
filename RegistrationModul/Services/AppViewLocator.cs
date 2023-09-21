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
            RegistrationViewModel context => new RegistrationView { DataContext = context },
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
    }
}
