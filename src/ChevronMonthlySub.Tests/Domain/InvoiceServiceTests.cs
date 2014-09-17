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
	using UI.Services;

	[TestFixture]
	public class InvoiceServiceTests
	{
		private OrderLineExtractor _extractor;
		private IShippingCostService _shipService;
		private ITemplatePathService _templateService;
		private IInvoiceService _invoiceService;
		private string _projectPath;
		private const string TestFileName = "Chevron June FG 462988.xlsx";

		[TestFixtureSetUp]
		public void FixtureInit()
		{
			var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
			if (directoryName == null) return;

			_projectPath = directoryName.Replace("file:\\", "").Replace("bin\\Debug", "DataFiles");

			_extractor = new OrderLineExtractor();
			_shipService = A.Fake<IShippingCostService>();
			_templateService = A.Fake<ITemplatePathService>();

			_invoiceService = new InvoiceService(_extractor, _shipService, _templateService);
			_invoiceService.SourcePath = Path.Combine(_projectPath, TestFileName);

			A.CallTo(() => _shipService.PickPackFee).Returns(0.50m);
			A.CallTo(() => _shipService.BoxFee).Returns(2.50m);
		}

		[Test]
		public void TestCreate()
		{
			_invoiceService.FreightLines.Count().Should().Be(130);
			_invoiceService.ProductLines.Count().Should().Be(374);
		}

		[Test]
		public void ReportWithNoFreightLines_GetsOneBox()
		{
			var itemsWithOneBoxFor15142183 = _invoiceService.ProductLines
				.Where(p => p.PoNumber == "15142183" && p.Boxes == 1);

			itemsWithOneBoxFor15142183.Count().Should().Be(1);
		}

		[Test]
		public void TotalFreight_MatchesExpected()
		{
			var freightFee = _invoiceService.FreightFee;

			freightFee.Should().Be(1921.28m);
		}

		[Test]
		public void SalesLines_MatchesExpected()
		{
			_invoiceService.SalesLines.Should().Be(374);
		}

		[Test]
		public void PickPackCount_MatchesExpected()
		{
			_invoiceService.PickPackCount.Should().Be(8771);
		}

		[Test]
		public void PickPackFee_MatchesExpected()
		{
			_invoiceService.PickPackFee.Should().Be(4385.50m);
		}

		[Test]
		public void BoxCount_MatchesExpected()
		{
			_invoiceService.BoxCount.Should().Be(130);
		}

		[Test]
		public void BoxFee_MatchesExpected()
		{
			_invoiceService.BoxFee.Should().Be(325.00m);
		}

		[Test]
		public void TotalInvoice_MatchesExpected()
		{
			_invoiceService.TotalInvoice.Should().Be(6631.78m);
		}
	}
}