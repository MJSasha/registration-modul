using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;

namespace RegistrationModul.ViewModels;

public class ViewModelBase : ReactiveObject, IScreen, IRoutableViewModel
{
    public RoutingState Router { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack => Router.NavigateBack;
    public IScreen HostScreen { get; }
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];

    public bool IsButtonEnabled => validationErrors.Count == 0;

    public Dictionary<string, string> ValidationErrors
    {
        get => validationErrors;
        private set => this.RaiseAndSetIfChanged(ref validationErrors, value);

    }
    private Dictionary<string, string> validationErrors = new();


    public ViewModelBase()
    {
        Router = new RoutingState();
    }

    public ViewModelBase(IScreen screen)
    {
        HostScreen = screen;
        Router = screen.Router;
    }

    protected void ValidateProperty(object value, string propertyName)
    {
        var validationContext = new ValidationContext(this) { MemberName = propertyName };
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateProperty(value, validationContext, validationResults))
        {
            ValidationErrors[propertyName] = validationResults.FirstOrDefault()?.ErrorMessage;
        }
        else
        {
            ValidationErrors.Remove(propertyName);
        }

        this.RaisePropertyChanged(nameof(IsButtonEnabled));
        this.RaisePropertyChanged(nameof(ValidationErrors));
    }
}
