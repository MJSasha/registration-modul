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
            SignedInViewModel context => new SignedInView { DataContext = context },
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
    }
}
