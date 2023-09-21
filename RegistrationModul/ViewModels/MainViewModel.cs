using ReactiveUI;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RegistrationModul.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string Login { get; set; }
    public string Password { get; set; }

    public ICommand SubmitButtonClickedCommand { get; }

    public MainViewModel()
    {
        SubmitButtonClickedCommand = ReactiveCommand.CreateFromTask(SubmitButtonClicked);
    }

    private async Task SubmitButtonClicked()
    {
        Router.Navigate.Execute(new RegistrationViewModel(this));
    }
}
