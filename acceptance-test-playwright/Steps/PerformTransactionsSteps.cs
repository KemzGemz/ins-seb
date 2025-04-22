using TechTalk.SpecFlow;
using acceptance_test_playwright.Pages;
using Microsoft.Playwright;

namespace acceptance_test_playwright.Steps;

[Binding]
public class PerformTransactionsSteps
{
    private readonly IPage _page;

    private HomePage _homePage;
    private CustomerPage _customerPage;
    private AccountPage _accountPage;
    private int _expectedBalance = 0;
    
    private readonly List<(string Type, string Amount)> _transactions = new()
    {
        ("Deposit", "50000"),
        ("Withdraw", "3000"),
        ("Withdraw", "2000"),
        ("Deposit", "5000"),
        ("Withdraw", "10000"),
        ("Withdraw", "15000"),
        ("Deposit", "1500")
    };

    public PerformTransactionsSteps(ScenarioContext scenarioContext)
    {
        _page = (IPage)scenarioContext["Page"];
    }

    [Given(@"I am logged in as ""(.*)""")]
    public async Task GivenIAmLoggedInAs(string customerName)
    {
        _homePage = new HomePage(_page);
        _customerPage = new CustomerPage(_page);
        _accountPage = new AccountPage(_page);
        
        await _page.GotoAsync("https://www.globalsqa.com/angularJs-protractor/BankingProject/#/login");
        await _homePage.ClickCustomerLoginAsync();
        await _customerPage.SelectCustomerAsync(customerName);
    }
    
    [Given(@"I select account ""(.*)""")]
    public async Task WhenISelectAccount(string accountNumber)
    {
        await _accountPage.SelectAccountAsync(accountNumber);
    }
    
    [When(@"I perform multiple transactions")]
    public async Task WhenIPerformMultipleTransactions()
    {
        foreach (var (type, amount) in _transactions)
        {
            if (type == "Deposit")
            {
                await _accountPage.DepositFundAsync(amount);
                _expectedBalance += int.Parse(amount);
            }
            else
            {
                await _accountPage.WithdrawFundAsync(amount);
                _expectedBalance -= int.Parse(amount);
            }
            Assert.Equal(await _accountPage.GetDisplayedBalanceAsync(), _expectedBalance);
            Console.WriteLine($"After transaction {amount}, balance = {_expectedBalance}");
        }
    }
    
    [Then(@"The current balance should tally with Balance section")]
    public async Task ThenTheBalanceShouldTallyWithBalanceSection()
    {
        int currentBalance = 0;
        foreach (var (type, amount) in _transactions)
        {
            if (type == "Deposit")
                currentBalance = currentBalance + int.Parse(amount);
            else
                currentBalance = currentBalance - int.Parse(amount);
        }
        
        Assert.Equal(await _accountPage.GetDisplayedBalanceAsync(), currentBalance);
        Console.WriteLine($"Current balance {currentBalance} is tally with Balance section");
    }
    
    
    
}
