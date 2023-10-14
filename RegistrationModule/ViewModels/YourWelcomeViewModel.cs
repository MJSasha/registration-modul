using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System.IO;

namespace RegistrationModul.ViewModels
{
    public partial class NotepadViewModel : ViewModelBase
    {
        public string Text { get => text; set { this.RaiseAndSetIfChanged(ref text, value); } }

        private readonly string fileName = "text.txt";

        private string text;


        public NotepadViewModel(IScreen screen) : base(screen)
        {
            Init();
        }

        public NotepadViewModel()
        {
            Init();
        }

        [RelayCommand]
        private void Exit()
        {
            Router.Navigate.Execute(new LoginViewModel(this));
        }

        [RelayCommand]
        private void Save()
        {
            File.WriteAllText(fileName, Text);
        }

        private void Init()
        {
            if (File.Exists(this.fileName))
            {
                Text = File.ReadAllText(this.fileName);
            }
        }
    }
}
