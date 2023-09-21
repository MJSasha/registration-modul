using ReactiveUI;
using System.Reactive;

namespace RegistrationModul.ViewModels;

public class ViewModelBase : ReactiveObject, IScreen
{
    public RoutingState Router { get; } = new RoutingState();

    public ReactiveCommand<Unit, IRoutableViewModel> GoBack => Router.NavigateBack;
}
