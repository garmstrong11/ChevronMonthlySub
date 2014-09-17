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
	using UI.Services;

	[TestFixture]
	public class PurchaseOrderTests
	{
		private OrderLineExtractor _extractor;
		private IShippingCostService _shipService;
		private ITemplatePathService _templatePathService;
		private IPurchaseOrderService _purchaseOrderService;
		private IEnumerable<FreightPurchaseOrder> _freightPurchaseOrders;
		private IEnumerable<ProductPurchaseOrder> _productPurchaseOrders;
			
		[TestFixtureSetUp]
		public void FixtureInit()
		{
			const string testFileName = @"DataFiles\Chevron June FG 462988.xlsx";
			var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

			if (directoryName == null) return;

			var projectPath = directoryName.Replace("file:\\", "").Replace("\\bin\\Debug", "");

			_extractor = new OrderLineExtractor();
			//_extractor.SourcePath = Path.Combine(projectPath, testFileName);
			_shipService = A.Fake<IShippingCostService>();
			_templatePathService = new HardCodedTemplatePathService();

			A.CallTo(() => _shipService.BoxFee).Returns(2.50m);
			A.CallTo(() => _shipService.PickPackFee).Returns(0.50m);

			_purchaseOrderService = new PurchaseOrderService(_extractor, _shipService, _templatePathService);
			_purchaseOrderService.SourcePath = Path.Combine(projectPath, testFileName);
			_freightPurchaseOrders = _purchaseOrderService.GetFreightPurchaseOrders("462988");
			_productPurchaseOrders = _purchaseOrderService.GetProductPurchaseOrders("462988");
		}

		[Test]
		public void TestCreate()
		{
			_purchaseOrderService.FreightLines.Count().Should().Be(130);
			_purchaseOrderService.ProductLines.Count().Should().Be(374);
		}

		[Test]
		public void ToString_ReturnsExpected()
		{
			var firstLine = _freightPurchaseOrders.First();
			var result = firstLine.ToString();
			result.Should().Be("462988 15145823 UK NET FRT.xlsx");
		}

		[Test]
		public void CanRunFreightReport()
		{
			var firstPo = _freightPurchaseOrders.First();
			firstPo.ConfigureReport();
			firstPo.RunReports();
		}

		[Test]
		public void CanRunProductReport()
		{
			var firstPo = _productPurchaseOrders.Skip(1).Take(1).First();
			firstPo.ConfigureReport();
			firstPo.RunReports();
		}

		[Ignore]
		[Test]
		public void RunsAllReports()
		{
			foreach (var fpo in _freightPurchaseOrders) {
				fpo.ConfigureReport();
				fpo.RunReports();
			}
			foreach (var ppo in _productPurchaseOrders) {
				ppo.ConfigureReport();
				ppo.RunReports();
			}
		}

		[TestCase("AL", Result = 22)]
		[TestCase("AZ", Result = 3)]
		[TestCase("CA", Result = 16)]
		[TestCase("GA", Result = 1)]
		[TestCase("LA", Result = 4)]
		[TestCase("MS", Result = 1)]
		[TestCase("NM", Result = 1)]
		[TestCase("NV", Result = 4)]
		[TestCase("UT", Result = 1)]
		[TestCase("WA", Result = 5)]
		public int AL15146759_BoxFees_MatchManualReport(string stateName)
		{
			var po15146759 = _productPurchaseOrders.First(p => p.PoNumber == "15146759");
			var state = po15146759.States.First(s => s.StateName == stateName);

			return state.BoxCount;
		}

		[TestCase("AL", Result = 111.0)]
		[TestCase("AZ", Result = 6.50)]
		[TestCase("CA", Result = 161.50)]
		[TestCase("GA", Result = 1.50)]
		[TestCase("LA", Result = 24.0)]
		[TestCase("MS", Result = 9.50)]
		[TestCase("NM", Result = 9.50)]
		[TestCase("NV", Result = 21.50)]
		[TestCase("UT", Result = 4.50)]
		[TestCase("WA", Result = 28.50)]
		public decimal AL15146759_PickPackFees_MatchManualReport(string stateName)
		{
			var po15146759 = _productPurchaseOrders.First(p => p.PoNumber == "15146759");
			var state = po15146759.States.First(s => s.StateName == stateName);

			return state.PickPackCharges;
		}

		[Test]
		public void UpdatePoNumber_AltersAllChildOrderLines()
		{
			var po15146759 = _freightPurchaseOrders.First(p => p.PoNumber == "15146759");

			po15146759.UpdatePoNumber("9669");

			var orderLines = po15146759.States.SelectMany(p => p.OrderLines);

			orderLines.All(o => o.PoNumber == "9669").Should().BeTrue();
			po15146759.PoNumber.Should().Be("9669");
		}
	}
}