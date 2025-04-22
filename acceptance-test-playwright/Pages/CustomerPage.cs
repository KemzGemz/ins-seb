using Microsoft.Playwright;

namespace acceptance_test_playwright.Pages;

public class CustomerPage
{
    private readonly IPage _page;

    public CustomerPage(IPage page)
    {
        _page = page;
    }
    public ILocator AccountLoginButton => _page.Locator("text=Login");
    public async Task SelectCustomerAsync(string customerName)
    {
        await _page.SelectOptionAsync("#userSelect", new SelectOptionValue { Label = customerName });
        await AccountLoginButton.ClickAsync();
    }

}