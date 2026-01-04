using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorApp.Web.Components.Dialogs;

public partial class DeleteConfirmationDialog : ComponentBase
{
    [CascadingParameter]
    private IDialogReference? DialogReference { get; set; }

    [Parameter]
    public EventCallback OnConfirm { get; set; }

    private void Cancel()
    {
        DialogReference?.Close(DialogResult.Cancel());
    }

    private async Task Delete()
    {
        DialogReference?.Close(DialogResult.Ok(true));
        if (OnConfirm.HasDelegate)
        {
            await OnConfirm.InvokeAsync();
        }
    }
}
