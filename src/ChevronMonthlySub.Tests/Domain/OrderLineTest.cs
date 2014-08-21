namespace ChevronMonthlySub.Tests.Domain
{
	using System;
	using System.Text;
	using ChevronMonthlySub.Domain;
	using FluentAssertions;
	using NUnit.Framework;

	[TestFixture]
	public class OrderLineTest
	{
		private FlexCelOrderLineDto _dto;
		
		[SetUp]
		public void Init()
		{
			_dto = new FlexCelOrderLineDto
				{
				DateShipped = new DateTime(2014, 6, 30),
				PoNumber = "15145823",
				OrderNumber = 388133,
				InventoryItemId = "7359-FCP",
				LineAmount = 57.96m,
				TaxAmount = 0.00m,
				QtyShipped = 17,
				LineDistribution = "Sales",
				FreightLineCount = 0,
				OrderLineCount = 1
				};

			var sb = new StringBuilder();
			sb.AppendLine("Q2/3 2014 Texaco Field Communication Guide");
			sb.AppendLine("Shipped to TEXACO #355290, FORT WORTH, TX.");

			_dto.LineDesc = sb.ToString();
		}

		[Test]
		public void Ctor_CapturesStateAndDestination_Multiline()
		{
			var line = new OrderLine(_dto);

			line.State.Should().Be("TX");
			line.Destination.Should().Be("TEXACO #355290, FORT WORTH, TX.");
		}

		[Test]
		public void Ctor_CapturesCityStateAndSite_Singleline()
		{
			_dto.LineDesc = "Freight - Non-Tax Shipped to CHEVRON #306168, Atlanta, GA.";
			var line = new OrderLine(_dto);

			line.State.Should().Be("GA");
			line.Destination.Should().Be("CHEVRON #306168, Atlanta, GA.");
		}

		[Test]
		public void Ctor_SetsTaxGroupCorrectly()
		{
			_dto.LineDesc = "Freight - Non-Tax Shipped to CHEVRON #306168, Atlanta, GA.";
			var line = new OrderLine(_dto);
			line.TaxType.Should().Be("NET");

			_dto.LineDesc = "Freight - Non-Tax Shipped to CHEVRON #306168, Atlanta, AK.";
			line = new OrderLine(_dto);
			line.TaxType.Should().Be("NOMAD");
		}

		[Test]
		public void ThrowsOnBadState()
		{
			_dto.LineDesc = "Freight - Non-Tax Shipped to CHEVRON #306168, Atlanta, ZZ.";

			Action act = () => new OrderLine(_dto);
			act.ShouldThrow<InvalidStateException>();
		}
	}
}