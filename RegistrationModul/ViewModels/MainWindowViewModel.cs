using ReactiveUI;
using System.Reactive;

namespace RegistrationModul.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; } = new RoutingState();

        public ReactiveCommand<Unit, IRoutableViewModel> GoNext { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoBack => Router.NavigateBack;

        public MainWindowViewModel()
        {
            GoNext = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new SignedInViewModel(this)));
        }
    }
}
