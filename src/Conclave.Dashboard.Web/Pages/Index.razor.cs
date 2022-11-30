using System.ComponentModel;
using Conclave.Dashboard.Web.Services;
using Microsoft.AspNetCore.Components;

namespace Conclave.Dashboard.Web.Pages;

public partial class Index
{
    [Inject]
    public AppStateService? AppStateService { get; set; }

    public int Counter { get; set; }

    protected override void OnInitialized()
    {
        if (AppStateService is not null)
            AppStateService.PropertyChanged += OnAppStatePropertyChanged;
        base.OnInitialized();
    }

    private async void OnAppStatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        await InvokeAsync(StateHasChanged);
    }
}