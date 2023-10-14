using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RegistrationModul.ViewModels;

namespace RegistrationModul.Views
{
    public partial class NotepadView : ReactiveUserControl<NotepadViewModel>
    {
        public NotepadView()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
