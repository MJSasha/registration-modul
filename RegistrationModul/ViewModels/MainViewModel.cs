using ReactiveUI;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RegistrationModul.ViewModels;

public class MainViewModel : MainWindowViewModel
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
        GoNext.Execute();
    }
}
