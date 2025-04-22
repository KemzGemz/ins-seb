using Microsoft.Playwright;

namespace acceptance_test_playwright.Pages;

public class AccountPage
{
    private readonly IPage _page;

    public AccountPage(IPage page)
    {
        _page = page;
    }
    
    public ILocator TransactionsTab => _page.Locator("[ng-click='transactions()']");
    public ILocator DepositTab => _page.Locator("[ng-click='deposit()']");
    public ILocator WithdrawalTab => _page.Locator("[ng-click='withdrawl()']");
    
    public ILocator AmountInput => _page.Locator("[placeholder='amount']");
    public ILocator SubmitButton => _page.Locator("button[type='submit']");
    
    public async Task SelectAccountAsync(string accountNo)
    {
        await _page.SelectOptionAsync("#accountSelect", new SelectOptionValue { Label = accountNo });
    }
    
    public async Task DepositFundAsync(string depositAmount)
    {
        await DepositTab.ClickAsync();
        await ExecuteTransactionsAsync(depositAmount);
    }
    
    public async Task WithdrawFundAsync(string withdrawalAmount)
    {
        await WithdrawalTab.ClickAsync();
        await ExecuteTransactionsAsync(withdrawalAmount);
    }

    private async Task ExecuteTransactionsAsync(string amount)
    {
        await Task.Delay(500);
        await AmountInput.FillAsync(amount);
        await Task.Delay(500);
        await SubmitButton.ClickAsync();
        await Task.Delay(500);
    }
    
    public async Task<int> GetDisplayedBalanceAsync()
    {
        var balanceText = await _page
            .Locator("//div[@ng-hide='noAccount']/strong[2]")
            .InnerTextAsync();
        return int.Parse(balanceText);
    }

}