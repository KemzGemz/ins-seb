using Microsoft.Playwright;

namespace acceptance_test_playwright.Pages;

public class HomePage
{
    private readonly IPage _page;

    public HomePage(IPage page)
    {
        _page = page;
    }

    // Selectors
    public ILocator CustomerLoginButton => _page.Locator("text=Customer Login");
    public ILocator ManagerLoginButton => _page.Locator("text=Bank Manager Login");
    
    public async Task ClickCustomerLoginAsync()
    {
        await CustomerLoginButton.ClickAsync();
    }
    
    public async Task ClickManagerLoginAsync()
    {
        await ManagerLoginButton.ClickAsync();
    }
}