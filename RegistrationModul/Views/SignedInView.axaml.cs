using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RegistrationModul.ViewModels;

namespace RegistrationModul.Views
{
    public partial class SignedInView : ReactiveUserControl<SignedInViewModel>
    {
        public SignedInView()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
