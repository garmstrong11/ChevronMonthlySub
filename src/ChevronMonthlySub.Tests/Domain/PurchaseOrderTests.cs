namespace ChevronMonthlySub.Tests.Domain
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using ChevronMonthlySub.Domain;
	using ChevronMonthlySub.Extractor;
	using FakeItEasy;
	using FluentAssertions;
	using NUnit.Framework;

	[TestFixture]
	public class PurchaseOrderTests
	{
		private OrderLineExtractor _extractor;
		private IRecipientRepository _recipRepo;
		private IShippingCostService _shipService;
		private ITemplatePathService _templatePathService;
		private PurchaseOrderRepository _repo;
		private IEnumerable<FreightPurchaseOrder> _freightPurchaseOrders;
		private IEnumerable<ProductPurchaseOrder> _productPurchaseOrders;
			
		[TestFixtureSetUp]
		public void FixtureInit()
		{
			const string testFileName = @"DataFiles\Chevron June FG 462988.xlsx";
			var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

			if (directoryName == null) return;

			var projectPath = directoryName.Replace("file:\\", "").Replace("\\bin\\Debug", "");

			_extractor = new OrderLineExtractor(Path.Combine(projectPath, testFileName));
			_recipRepo = new HardCodedRecipientRepository();
			_shipService = A.Fake<IShippingCostService>();
			_templatePathService = new HardCodedTemplatePathService();

			A.CallTo(() => _shipService.BoxFee).Returns(2.50m);
			A.CallTo(() => _shipService.PickPackFee).Returns(0.50m);

			_repo = new PurchaseOrderRepository(_extractor, _recipRepo, _shipService, _templatePathService);
			_freightPurchaseOrders = _repo.GetFreightPurchaseOrders("462988");
			_productPurchaseOrders = _repo.GetProductPurchaseOrders("462988");
		}

		[Test]
		public void TestCreate()
		{
			_repo.FreightLines.Count().Should().Be(126);
			_repo.ProductLines.Count().Should().Be(351);
		}

		[Test]
		public void ToString_ReturnsExpected()
		{
			var firstLine = _freightPurchaseOrders.First();
			var result = firstLine.ToString();
			result.Should().Be("462988 15145822 NET ML FRT.xlsx");
		}

		[Test]
		public void CanRunFreightReport()
		{
			var firstPo = _freightPurchaseOrders.First();
			firstPo.ConfigureReport();
			firstPo.RunReports();
		}

		[Test]
		public void CanRunProductReports()
		{
			var firstPo = _productPurchaseOrders.First();
			firstPo.ConfigureReport();
			firstPo.RunReports();
		}
	}
}