using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using RegistrationModul.Services;
using RegistrationModule.Definitions;
using System.IO;

namespace RegistrationModul.ViewModels
{
    public partial class NotepadViewModel : ViewModelBase
    {
        #region Public props

        public string Text { get => text; set => this.RaiseAndSetIfChanged(ref text, value); }
        public bool CanEdit { get => canEdit; set => this.RaiseAndSetIfChanged(ref canEdit, value); }

        #endregion

        #region Private props

        private readonly string fileName = "text.txt";

        private string text;
        private bool canEdit;

        #endregion

        public NotepadViewModel(IScreen screen) : base(screen)
        {
            Init();
        }

        public NotepadViewModel()
        {
            Init();
        }

        #region Relay commands

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

        #endregion

        private void Init()
        {
            CanEdit = AuthService.CurrentUser.Role == UserRole.Editor;
            if (File.Exists(this.fileName))
            {
                Text = File.ReadAllText(this.fileName);
            }
        }
    }
}
