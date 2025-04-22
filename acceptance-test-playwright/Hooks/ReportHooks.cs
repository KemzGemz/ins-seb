namespace acceptance_test_playwright.Hooks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using TechTalk.SpecFlow;

[Binding]
public sealed class ReportHooks
{
    private static ExtentReports _extent;

    [BeforeTestRun]
    public static void InitializeReport()
    {
        var reportPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "TestResults", "ExtentReport.html");
        Directory.CreateDirectory(Path.GetDirectoryName(reportPath)!);

        var reporter = new ExtentSparkReporter(Path.GetFullPath(reportPath));
        _extent = new ExtentReports();
        _extent.AttachReporter(reporter);
    }

    [BeforeFeature]
    public static void BeforeFeature(FeatureContext featureContext)
    {
        var feature = _extent.CreateTest(featureContext.FeatureInfo.Title);
        featureContext["ExtentFeature"] = feature;
    }

    [BeforeScenario]
    public void BeforeScenario(ScenarioContext scenarioContext, FeatureContext featureContext)
    {
        var feature = (ExtentTest)featureContext["ExtentFeature"];
        var scenario = feature.CreateNode(scenarioContext.ScenarioInfo.Title);
        scenarioContext["ExtentScenario"] = scenario;
    }

    [AfterStep]
    public void InsertReportingSteps(ScenarioContext scenarioContext)
    {
        var scenario = (ExtentTest)scenarioContext["ExtentScenario"];
        var stepText = scenarioContext.StepContext.StepInfo.Text;
        var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();

        if (scenarioContext.TestError == null)
        {
            scenario.CreateNode(stepType, stepText);
        }
        else
        {
            scenario.CreateNode(stepType, stepText).Fail(scenarioContext.TestError.Message);
        }
    }

    [AfterTestRun]
    public static void TearDownReport()
    {
        _extent.Flush();
    }
}