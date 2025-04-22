using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace acceptance_test_playwright.Hooks;

[Binding]
public class Hooks
{
    private readonly ScenarioContext _scenarioContext;

    public Hooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public async Task BeforeScenarioAsync()
    {
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false, SlowMo = 100});
        var context = await browser.NewContextAsync(new () {ViewportSize = new ViewportSize() { Width = 1920, Height = 1080 }});
        var page = await context.NewPageAsync();

        _scenarioContext["Playwright"] = playwright;
        _scenarioContext["Browser"] = browser;
        _scenarioContext["Context"] = context;
        _scenarioContext["Page"] = page;
    }

    [AfterScenario]
    public async Task AfterScenarioAsync()
    {
        if (_scenarioContext.TryGetValue("Browser", out IBrowser browser))
            await browser.CloseAsync();

        if (_scenarioContext.TryGetValue("Playwright", out IPlaywright playwright))
            playwright.Dispose();
    }
}