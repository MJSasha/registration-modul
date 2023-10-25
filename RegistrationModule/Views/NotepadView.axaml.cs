using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RegistrationModul.ViewModels;
using System.Linq;

namespace RegistrationModul.Views
{
    public partial class NotepadView : ReactiveUserControl<NotepadViewModel>
    {
        public NotepadView()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }

        private async void OpenFileButton_Clicked(object sender, RoutedEventArgs args)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Text File",
                AllowMultiple = false
            });

            var file = files.FirstOrDefault();
            if (file == null) return;
            ViewModel.File = file;
        }
    }
}
