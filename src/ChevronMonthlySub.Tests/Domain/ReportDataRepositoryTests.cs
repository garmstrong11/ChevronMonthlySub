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
		private IRecipientRepository _recipRepo;

		[TestFixtureSetUp]
		public void FixtureInit()
		{
			const string testFileName = @"DataFiles\Chevron June FG 462988.xlsx";
			var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

			if (directoryName == null) return;

			var projectPath = directoryName.Replace("file:\\", "").Replace("\\bin\\Debug", "");

			_extractor = new OrderLineExtractor(Path.Combine(projectPath, testFileName));
			_recipRepo = new HardCodedRecipientRepository();
		}

		[Test]
		public void TestCreate()
		{
			var repo = new ReportDataRepository(_extractor, _recipRepo);
			repo.FreightLines.Count().Should().Be(126);
			repo.ProductLines.Count().Should().Be(351);
		}

		[Test]
		public void ReportWithNoFreightLines_GetsOneBox()
		{
			var repo = new ReportDataRepository(_extractor, _recipRepo);
			var itemsWithOneBoxFor15142183 = repo.ProductLines
				.Where(p => p.PoNumber == "15142183" && p.Boxes == 1);

			itemsWithOneBoxFor15142183.Count().Should().Be(1);
		}
	}
}