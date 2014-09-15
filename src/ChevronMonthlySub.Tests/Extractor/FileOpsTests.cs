namespace ChevronMonthlySub.Tests.Extractor
{
  using System.IO;
  using System.Reflection;
  using ChevronMonthlySub.Extractor;
  using FluentAssertions;
  using NUnit.Framework;

  [TestFixture]
  public class FileOpsTests
  {
    private string _projectPath;
    private const string GoodFileName = "Chevron June FG 462988.xlsx";
    private const string BadFileName = "ServiceOrderKey.xlsx";


    [TestFixtureSetUp]
    public void Init()
    {
      _projectPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)
        .Replace("file:\\", "")
        .Replace("bin\\Debug", "DataFiles");
    }
    
    [Test]
    public void CanCreateFileOps()
    {
      var fileOps = new FileOps();

      fileOps.Should().NotBeNull();
    }

    [TestCase("beetles", Result = "")]
    [TestCase("beetles.xlsx", Result = ".xlsx")]
    public string GetExtensionTest(string test)
    {
      var fileOps = new FileOps();
      var ext = fileOps.GetExtension(test);

      return ext;
    }

    [TestCase(@"F:\Tic\Tac\Toe.eps", Result = "Toe")]
    [TestCase(@"F:\Tic\Tac\", Result = "")]
    public string GetFileNameTest(string test)
    {
      var fileOps = new FileOps();
      var nym = fileOps.GetFileName(test);

      return nym;
    }
  }
}