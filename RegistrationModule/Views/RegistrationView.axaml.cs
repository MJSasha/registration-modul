using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RegistrationModul.ViewModels;

namespace RegistrationModul.Views
{
    public partial class RegistrationView : ReactiveUserControl<RegistrationViewModel>
    {
        public RegistrationView()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
