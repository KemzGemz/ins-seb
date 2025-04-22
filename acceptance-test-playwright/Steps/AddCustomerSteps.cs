using TechTalk.SpecFlow;
using acceptance_test_playwright.Pages;
using Microsoft.Playwright;

namespace acceptance_test_playwright.Steps;

[Binding]
public class AddCustomerSteps
{
    private readonly IPage _page;

    private HomePage _homePage;
    private ManagerPage _managerPage;
    private CustomerPage _customerPage;
    
    private readonly List<(string FirstName, string LastName, string PostCode)> _customers = new()
    {
        ("Christopher", "Connely", "L789C349"),
        ("Frank", "Christopher", "A897N450"),
        ("Christopher", "Minka", "M098Q585"),
        ("Connely", "Jackson", "L789C349"),
        ("Jackson", "Frank", "L789C349"),
        ("Minka", "Jackson", "A897N450"),
        ("Jackson", "Connely", "L789C349")
    };
    
    private readonly List<(string FirstName, string LastName, string PostCode)> _lostcustomers = new()
    {
        ("Jackson", "Frank", "L789C349"),
        ("Christopher", "Connely", "L789C349")
    };

    public AddCustomerSteps(ScenarioContext scenarioContext)
    {
        _page = (IPage)scenarioContext["Page"];
    }

    [Given(@"I am logged in as manager")]
    public async Task GivenIAmLoggedInAsManager()
    {
        _homePage = new HomePage(_page);
        await _page.GotoAsync("https://www.globalsqa.com/angularJs-protractor/BankingProject/#/login");
        await _homePage.ClickManagerLoginAsync();
    }

    [When(@"I add all customers from the list")]
    public async Task WhenIAddAllCustomersFromTheList()
    {
        _managerPage = new ManagerPage(_page);
        await _managerPage.ClickAddCustomerTabAsync();
        foreach (var customer in _customers)
        {
            await _managerPage.AddCustomerAsync(customer.FirstName, customer.LastName, customer.PostCode);
        }
    }
    
    [Then(@"all of them should appear in the customer table")]
    public async Task ThenAllOfThemShouldAppearInTheTable()
    {
        await _managerPage.ClickCustomersTabAsync();
        foreach (var customer in _customers)
        {
            await _managerPage.SearchCustomerByNameAsync(customer.FirstName);
            Assert.True(
                await _managerPage.IsCustomerListed(customer.FirstName, customer.LastName, customer.PostCode),
                $"Customer not found: {customer.FirstName} {customer.LastName} {customer.PostCode}");
        }
        
    }
    
    [Then(@"specific customers are deleted")]
    public async Task WhenAllOfThemShouldAppearInTheTable()
    {
        await _managerPage.ClickCustomersTabAsync();
        foreach (var customer in _lostcustomers)
        {
            await _managerPage.SearchCustomerByNameAsync(customer.FirstName);
            await _managerPage.DeleteCustomer(customer.FirstName, customer.LastName, customer.PostCode);
        }
        
    }
    
    [Then(@"the deleted customers are no longer in the customer table")]
    public async Task ThenTheDeletedCustomersAreNoLongerInTheTable()
    {
        await _managerPage.ClickCustomersTabAsync();
        foreach (var customer in _lostcustomers)
        {
            await _managerPage.SearchCustomerByNameAsync(customer.FirstName);
            Assert.True(
                !await _managerPage.IsCustomerListed(customer.FirstName, customer.LastName, customer.PostCode),
                $"Customer found: {customer.FirstName} {customer.LastName} {customer.PostCode}");
        }
        
    }
}
