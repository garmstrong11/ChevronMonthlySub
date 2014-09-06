namespace ChevronMonthlySub.Tests.Extractor
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using ChevronMonthlySub.Domain;
	using ChevronMonthlySub.Extractor;
	using FluentAssertions;
	using NUnit.Framework;

	[TestFixture]
	public class OrderKeyExtractorTests
	{
		private OrderKeyExtractor _extractor;
		private IList<OrderKeyRowDto> _dtos;

		[TestFixtureSetUp]
		public void FixtureInit()
		{
			const string testFileName = @"DataFiles\ServiceOrderKey.xlsx";
			var projectPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)
				.Replace("file:\\", "")
				.Replace("\\bin\\Debug", "");

			_extractor = new OrderKeyExtractor {SourcePath = Path.Combine(projectPath, testFileName)};
			_dtos = _extractor.Extract();
		}

		[Test]
		public void CanExtract()
		{
			_dtos.Count.Should().Be(17);
		}

		[Test]
		public void ProductId_ReturnsExpected()
		{
			var firstRow = _dtos.FirstOrDefault();

			firstRow.Should().NotBeNull();
			if (firstRow != null) firstRow.ProductId.Should().Be("15142183");
		}

		[Test]
		public void FreightId_ReturnsExpected()
		{
			var secondRow = _dtos.Skip(1).Take(1).FirstOrDefault();

			secondRow.Should().NotBeNull();
			if (secondRow != null) secondRow.FreightId.Should().Be("15145822");
		}

		[Test]
		public void Description_ReturnsExpected()
		{
			var thirdRow = _dtos.Skip(2).FirstOrDefault();
			thirdRow.Should().NotBeNull();
			if (thirdRow != null) thirdRow.Description.Should().Be("Texaco Brand April-June 2014 Subsequent Orders");
		}

		[Test]
		public void RequestorInitials_ReturnsExpected()
		{
			var fourthRow = _dtos.Skip(3).FirstOrDefault();

			fourthRow.Should().NotBeNull();
			if (fourthRow != null) fourthRow.RequestorInitials.Should().Be("AA");
		}
	}
}