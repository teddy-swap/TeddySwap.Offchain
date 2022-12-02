using Microsoft.AspNetCore.Components;

namespace Conclave.Dashboard.Web.Components;

public partial class Header
{
    [Parameter]
    public EventCallback OnBurgerMenuClicked { get; set; }

    [Parameter]
    public string WalletAddress { get; set; } = string.Empty;
}
