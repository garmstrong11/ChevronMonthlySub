namespace ChevronMonthlySub.Tests.Domain
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using ChevronMonthlySub.Domain;
	using ChevronMonthlySub.Extractor;
	using FlexCel.XlsAdapter;
	using FluentAssertions;
	using NUnit.Framework;

	[TestFixture]
	public class LineFactoryTests
	{
		private XlsFile _xls;
		private OrderLineExtractor _extractor;
		private IList<FlexCelOrderLineDto> _dtos;

		[TestFixtureSetUp]
		public void FixtureInit()
		{
			const string testFileName = @"DataFiles\Chevron June FG 462988.xlsx";
			var projectPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)
				.Replace("file:\\", "")
				.Replace("\\bin\\Debug", "");

			_xls = new XlsFile(Path.Combine(projectPath, testFileName), false);
			_extractor = new OrderLineExtractor(_xls);
			_dtos = _extractor.Extract();
		}

		[Test]
		public void TestCreate()
		{
			var factory = new LineFactory();
			var lines = _dtos.Select(factory.Create).ToList();

			lines.Count.Should().Be(477);
		}

		[Test]
		public void SetBoxCount_ReturnsCorrectTotal()
		{
			var factory = new LineFactory();
			var lines = _dtos.Select(factory.Create).ToList();

			factory.AssignBoxCount(lines);

			var boxCount = lines.OfType<ProductLine>().Sum(b => b.Boxes);

			boxCount.Should().Be(128);
		}
	}
}