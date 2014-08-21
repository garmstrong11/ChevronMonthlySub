namespace ChevronMonthlySub.Tests.Domain
{
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using ChevronMonthlySub.Domain;
	using ChevronMonthlySub.Extractor;
	using FluentAssertions;
	using NUnit.Framework;

	[TestFixture]
	public class ReportDataRepositoryTests
	{
		private OrderLineExtractor _extractor;

		[TestFixtureSetUp]
		public void FixtureInit()
		{
			const string testFileName = @"DataFiles\Chevron June FG 462988.xlsx";
			var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

			if (directoryName == null) return;

			var projectPath = directoryName.Replace("file:\\", "").Replace("\\bin\\Debug", "");

			_extractor = new OrderLineExtractor(Path.Combine(projectPath, testFileName));
		}

		[Test]
		public void TestCreate()
		{
			var repo = new ReportDataRepository(_extractor);
			repo.FreightLines.Count().Should().Be(126);
			repo.ProductLines.Count().Should().Be(351);
		}
	}
}