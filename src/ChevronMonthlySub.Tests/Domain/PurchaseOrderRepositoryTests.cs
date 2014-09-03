namespace ChevronMonthlySub.Tests.Domain
{
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using ChevronMonthlySub.Domain;
	using ChevronMonthlySub.Extractor;
	using FakeItEasy;
	using FluentAssertions;
	using NUnit.Framework;

	[TestFixture]
	public class PurchaseOrderRepositoryTests
	{
		private OrderLineExtractor _extractor;
		private IRequestorService _recipRepo;

		[TestFixtureSetUp]
		public void FixtureInit()
		{
			const string testFileName = @"DataFiles\Chevron June FG 462988.xlsx";
			var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

			if (directoryName == null) return;

			var projectPath = directoryName.Replace("file:\\", "").Replace("\\bin\\Debug", "");

			_extractor = new OrderLineExtractor();
			_extractor.SourcePath = Path.Combine(projectPath, testFileName);
			_recipRepo = new HardCodedRequestorService();
		}

		//[Test]
		//public void TestCreate()
		//{
		//	var shipService = A.Fake<IShippingCostService>();
		//	A.CallTo(() => shipService.BoxFee).Returns(2.50m);
		//	A.CallTo(() => shipService.PickPackFee).Returns(0.50m);

		//	var repo = new PurchaseOrderService(_extractor, _recipRepo, shipService);
		//	repo.FreightLines.Count().Should().Be(126);
		//	repo.ProductLines.Count().Should().Be(351);
		//}

		//[Test]
		//public void ReportWithNoFreightLines_GetsOneBox()
		//{
		//	var shipService = A.Fake<IShippingCostService>();
		//	A.CallTo(() => shipService.BoxFee).Returns(2.50m);
		//	A.CallTo(() => shipService.PickPackFee).Returns(0.50m);

		//	var repo = new PurchaseOrderService(_extractor, _recipRepo, shipService);
		//	var itemsWithOneBoxFor15142183 = repo.ProductLines
		//		.Where(p => p.PoNumber == "15142183" && p.Boxes == 1);

		//	itemsWithOneBoxFor15142183.Count().Should().Be(1);
		//}
	}
}