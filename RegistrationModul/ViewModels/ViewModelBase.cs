using ReactiveUI;
using System;
using System.Reactive;

namespace RegistrationModul.ViewModels;

public class ViewModelBase : ReactiveObject, IScreen, IRoutableViewModel
{
    public RoutingState Router { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack => Router.NavigateBack;
    public IScreen HostScreen { get; }
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];


    public ViewModelBase()
    {
        Router = new RoutingState();
    }

    public ViewModelBase(IScreen screen)
    {
        HostScreen = screen;
        Router = screen.Router;
    }
}
