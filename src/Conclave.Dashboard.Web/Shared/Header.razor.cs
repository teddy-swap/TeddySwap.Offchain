using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Conclave.Dashboard.Web.Services;

namespace Conclave.Dashboard.Web.Shared;

public partial class Header : IDisposable
{
    [Inject]
    public AppStateService? AppStateService { get; set; }

    [Parameter]
    public EventCallback OnBurgerMenuClicked { get; set; }

    [Parameter]
    public string WalletAddress { get; set; } = string.Empty;

    public bool IsDrawerOpen
    {
        get => AppStateService?.IsDrawerOpen ?? false;
        set
        {
            if (AppStateService is not null) AppStateService.IsDrawerOpen= value;
        }
    }

    private bool _isDropDownVisible { get; set; }

    private void OnDropDownClicked() => _isDropDownVisible = !_isDropDownVisible;

    private void ToggleDrawer()
    {
        ArgumentNullException.ThrowIfNull(AppStateService);
        AppStateService.IsDrawerOpen = !AppStateService.IsDrawerOpen;
        AppStateService.PropertyChanged += OnAppStatePropertyChanged; 
        Console.WriteLine(AppStateService.IsDrawerOpen);
    }

    private async void OnAppStatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        ArgumentNullException.ThrowIfNull(AppStateService);
        AppStateService.PropertyChanged -= OnAppStatePropertyChanged; 
    }
}
