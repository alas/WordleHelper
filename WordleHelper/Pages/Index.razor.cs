namespace WordleHelper.Pages;

using Microsoft.AspNetCore.Components.Forms;
using WordleHelper.Code;

public partial class Index
{
    private Model Model { get; set; } = new();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private WordsLists WordsLists;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private string[]? Results;

    protected override async Task OnInitializedAsync()
    {
        WordsLists = await WordsLists.FactoryAsync();
    }

    private void FormSubmitted(EditContext editContext)
    {
        Results = WordsLists.GetAllWords(Model);
    }
}
