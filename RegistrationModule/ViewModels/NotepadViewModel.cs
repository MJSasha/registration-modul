using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using RegistrationModul.Models;
using RegistrationModul.Services;
using RegistrationModule.Definitions;
using RegistrationModule.Models;
using RegistrationModule.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace RegistrationModul.ViewModels
{
    public partial class NotepadViewModel : ViewModelBase
    {
        #region Public props

        public IStorageFile File { get => file; set { this.RaiseAndSetIfChanged(ref file, value); if (value != null) OnFileOpened(); } }
        public ObservableCollection<UserWitPermission> Users { get => users; set => this.RaiseAndSetIfChanged(ref users, value); }
        public ObservableCollection<string> Roles { get => roles; set => this.RaiseAndSetIfChanged(ref roles, value); }
        public string Text { get => text; set => this.RaiseAndSetIfChanged(ref text, value); }
        public bool CanEdit { get => canEdit; set => this.RaiseAndSetIfChanged(ref canEdit, value); }
        public bool CanChangeRole { get => canChangeRole; set => this.RaiseAndSetIfChanged(ref canChangeRole, value); }

        #endregion

        #region Private props

        private CompaniesService companiesService;

        private IStorageFile file;
        private ObservableCollection<UserWitPermission> users;
        private ObservableCollection<string> roles;
        private string text;
        private bool canEdit;
        private bool canChangeRole;

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
        private async Task Save()
        {
            var fileHash = await Utils.HashFile(File);
            System.IO.File.WriteAllText(file.Path.AbsolutePath, Text);
            var newFilePath = Utils.ChangeFileExtension(file.Path.AbsolutePath);

            var currentCompany = companiesService.GetCurrentCompany();
            var files = currentCompany.FileCredentials;
            var existingFile = files.Find(f => f.Path == newFilePath);

            if (existingFile != null) existingFile.Hash = fileHash;
            else files.Add(new FileCredentials { Path = newFilePath, Hash = fileHash });
            currentCompany.FileCredentials = files;
            companiesService.Update(currentCompany);
        }

        [RelayCommand]
        private void SaveUsers()
        {
            var updatedUsers = Users.Select(u =>
            {
                u.User.Role = u.CanEdit ? UserRole.Editor : UserRole.Viewer;
                return u.User;
            }).ToList();
            updatedUsers.Add(AuthService.CurrentUser);
            var currentCompany = companiesService.GetCurrentCompany();
            currentCompany.Users = updatedUsers;
            companiesService.Update(currentCompany);

            Users = new ObservableCollection<UserWitPermission>(companiesService.GetCurrentCompany().Users
                .Where(u => u.Id != AuthService.CurrentUser?.Id)
                .Select(u => new UserWitPermission { User = u, CanEdit = u.Role == UserRole.Editor }));
        }

        #endregion

        private void Init()
        {
            companiesService = new();
            Users = new ObservableCollection<UserWitPermission>(companiesService.GetCurrentCompany().Users
                .Where(u => u.Id != AuthService.CurrentUser?.Id)
                .Select(u => new UserWitPermission { User = u, CanEdit = u.Role == UserRole.Editor }));

            Roles = new ObservableCollection<string>(Enum.GetValues<UserRole>().Select(v => v.ToString()));
            CanEdit = AuthService.CurrentUser.Role == UserRole.Editor && file != null;
            CanChangeRole = AuthService.CurrentUser.Role == UserRole.Editor;
        }

        private async Task OnFileOpened()
        {
            var files = companiesService.GetCurrentCompany().FileCredentials;
            var existingFile = files.Find(f => f.Path == file.Path.AbsolutePath);
            var fileHash = await Utils.HashFile(File);

            if (existingFile?.Hash != fileHash)
            {
                Text = "Incorrect file. Please reselect.";
                return;
            }

            using var stream = await file.OpenReadAsync();
            using var reader = new StreamReader(stream);
            Text = await reader.ReadToEndAsync();
            CanEdit = AuthService.CurrentUser.Role == UserRole.Editor && file != null;
        }
    }

    public class UserWitPermission
    {
        public User User { get; set; }
        public bool CanEdit { get; set; }
    }
}
