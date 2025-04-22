using Microsoft.Playwright;

namespace acceptance_test_playwright.Pages;

public class ManagerPage
{
    private readonly IPage _page;

    public ManagerPage(IPage page)
    {
        _page = page;
    }
    public ILocator AddCustomerTab => _page.Locator("[ng-click='addCust()']");
    public ILocator OpenAccountTab => _page.Locator("[ng-click='openAccount()']");
    public ILocator CustomersTab => _page.Locator("[ng-click='showCust()']");
    
    public ILocator FirstNameInput => _page.Locator("[placeholder='First Name']");
    public ILocator LastNameInput => _page.Locator("[placeholder='Last Name']");
    public ILocator PostCodeInput => _page.Locator("[placeholder='Post Code']");
    public ILocator AddCustomerButton => _page.Locator("button[type='submit']");
    
    public ILocator SearchBarInput => _page.Locator("[placeholder='Search Customer']");
    
    public async Task AddCustomerAsync(string firstName, string lastName, string postCode)
    {
        await FirstNameInput.FillAsync(firstName);
        await LastNameInput.FillAsync(lastName);
        await PostCodeInput.FillAsync(postCode);
        await AddCustomerButton.ClickAsync();
    }
    
    public async Task ClickAddCustomerTabAsync()
    {
        await AddCustomerTab.ClickAsync();
    }
    
    public async Task SearchCustomerByNameAsync(string name)
    {
        await SearchBarInput.FillAsync(name);
    }
    
    public async Task ClickOpenAccountTabAsync()
    {
        await OpenAccountTab.ClickAsync();
    }
    
    public async Task ClickCustomersTabAsync()
    {
        await CustomersTab.ClickAsync();
    }
    
    public async Task<bool> IsCustomerListed(string firstName, string lastName, string postCode)
    {
        var rows = _page.Locator("table tbody tr");
        int count = await rows.CountAsync();

        for (int i = 0; i < count; i++)
        {
            var cells = rows.Nth(i).Locator("td");

            if (
                await cells.Nth(0).InnerTextAsync() == firstName &&
                await cells.Nth(1).InnerTextAsync() == lastName &&
                await cells.Nth(2).InnerTextAsync() == postCode
            )
            {
                return true;
            }
        }

        return false;
    }
    
    public async Task DeleteCustomer(string firstName, string lastName, string postCode)
    {
        var rows = _page.Locator("table tbody tr");
        int count = await rows.CountAsync();

        for (int i = 0; i < count; i++)
        {
            var cells = rows.Nth(i).Locator("td");

            if (
                await cells.Nth(0).InnerTextAsync() == firstName &&
                await cells.Nth(1).InnerTextAsync() == lastName &&
                await cells.Nth(2).InnerTextAsync() == postCode
            )
            {
                var deleteButton = rows.Nth(i).Locator("button:text('Delete')");
                await deleteButton.ClickAsync();
                return;
            }
        }

        Console.WriteLine($"Customer not found for deletion: {firstName} {lastName} {postCode}");
    }

    
}