namespace ChevronMonthlySub.Tests.Extractor
{
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using ChevronMonthlySub.Extractor;
	using FlexCel.XlsAdapter;
	using FluentAssertions;
	using NUnit.Framework;

	[TestFixture]
	public class TaxGroupExtractorTests
	{
		private XlsFile _xls;

		[TestFixtureSetUp]
		public void FixtureInit()
		{
			const string testFileName = @"DataFiles\TaxGroups.xlsx";
			var projectPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)
				.Replace("file:\\", "")
				.Replace("\\bin\\Debug", "");

			_xls = new XlsFile(Path.Combine(projectPath, testFileName), false);
		}

		[Test]
		public void CanGetTestFile()
		{
			_xls.SheetCount.Should().Be(1);
		}

		[Test]
		public void CanExtractTaxGroups()
		{
			var extractor = new TaxGroupExtractor(_xls);
			var groups = extractor.Extract();

			groups.Count.Should().Be(51);
		}

		[Test]
		public void GroupCountsAreCorrect()
		{
			var extractor = new TaxGroupExtractor(_xls);
			var groups = extractor.Extract();

			groups.Count(g => g.GroupName == "NOMAD").Should().Be(5);
			groups.Count(g => g.GroupName == "GROSS").Should().Be(16);
			groups.Count(g => g.GroupName == "NET").Should().Be(29);
			groups.Count(g => g.GroupName == "TX").Should().Be(1);
		}
	}
}