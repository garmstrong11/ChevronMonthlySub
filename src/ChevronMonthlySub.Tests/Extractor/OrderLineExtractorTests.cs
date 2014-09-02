namespace ChevronMonthlySub.Tests.Extractor
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using ChevronMonthlySub.Domain;
	using ChevronMonthlySub.Extractor;
	using FluentAssertions;
	using NUnit.Framework;

	[TestFixture]
	public class OrderLineExtractorTests
	{
		private OrderLineExtractor _extractor;
		private IList<FlexCelOrderLineDto> _dtos;

		[TestFixtureSetUp]
		public void FixtureInit()
		{
			const string testFileName = @"DataFiles\Chevron June FG 462988.xlsx";
			var projectPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)
				.Replace("file:\\", "")
				.Replace("\\bin\\Debug", "");

			_extractor = new OrderLineExtractor();
			_extractor.SourcePath = Path.Combine(projectPath, testFileName);
			_dtos = _extractor.Extract();
		}

		[Test]
		public void SourcePath_ReturnsCorrectPath()
		{
			_extractor.SourcePath.Should().Be(@"F:\CODE\ChevronMonthlySub\src\ChevronMonthlySub.Tests\DataFiles");
		}

		[Test]
		public void DateShippedIsCorrectDate()
		{
			var expected = new DateTime(2014, 6, 17);
			_dtos[0].DateShipped.Should().Be(expected);
		}

		[Test]
		public void PoNumberIsCorrect()
		{
			const string expected = "15142183";
			_dtos[0].PoNumber.Should().Be(expected);
		}

		[Test]
		public void OrderNumberIsCorrect()
		{
			const int expected = 388022;
			_dtos[0].OrderNumber.Should().Be(expected);
		}

		[Test]
		public void InventoryItemId_IsCorrect()
		{
			const string expected = "7351-X3";
			_dtos[0].InventoryItemId.Should().Be(expected);
		}

		[Test]
		public void LineDesc_IsCorrectAndContainsNoNewlines()
		{
			const string expected = "2014 Grand Opening Dual Branded 3x3 Shipped to KP Corporation, West Sacramento, CA.";
			_dtos[0].LineDesc.Should().Be(expected);
			_dtos.Any(v => v.LineDesc.Contains("\n")).Should().BeFalse();
		}

		[Test]
		public void LineAmount_IsCorrect()
		{
			const decimal expected = 57.96m;
			_dtos[21].LineAmount.Should().Be(expected);
		}

		[Test]
		public void TaxAmount_IsCorrect()
		{
			const decimal expected = 0.00m;
			_dtos[0].TaxAmount.Should().Be(expected);
		}

		[Test]
		public void QtyShipped_IsCorrect()
		{
			const int expected = 986;
			_dtos[0].QtyShipped.Should().Be(expected);
		}

		[Test]
		public void LineDistribution_IsCorrect()
		{
			const string expected = "Sales";
			_dtos[0].LineDistribution.Should().Be(expected);
		}

		[Test]
		public void FreightLineCount_IsCorrect()
		{
			const int expected = 1;
			_dtos[27].FreightLineCount.Should().Be(expected);
		}

		[Test]
		public void OrderLineCount_IsCorrect()
		{
			const int expected = 1;
			_dtos[0].OrderLineCount.Should().Be(expected);
		}
	}
}