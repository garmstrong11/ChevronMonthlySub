﻿namespace ChevronMonthlySub.Tests.Domain
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
		public void Ctor_CapturesCityStateAndSite_Multiline()
		{
			var line = new OrderLine(_dto);

			line.City.Should().Be("FORT WORTH");
			line.State.Should().Be("TX");
			line.Site.Should().Be("TEXACO #355290");
			line.Product.Should().Be("Q2/3 2014 Texaco Field Communication Guide");
		}

		[Test]
		public void Ctor_CapturesCityStateAndSite_Singleline()
		{
			_dto.LineDesc = "Freight - Non-Tax Shipped to CHEVRON #306168, Atlanta, GA.";
			var line = new OrderLine(_dto);

			line.City.Should().Be("Atlanta");
			line.State.Should().Be("GA");
			line.Site.Should().Be("CHEVRON #306168");
			line.Product.Should().Be("Freight - Non-Tax");
		}

		[Test]
		public void Ctor_SetsTaxGroupCorrectly()
		{
			_dto.LineDesc = "Freight - Non-Tax Shipped to CHEVRON #306168, Atlanta, GA.";
			var line = new OrderLine(_dto);
			line.TaxGroup.Should().Be("NET");

			_dto.LineDesc = "Freight - Non-Tax Shipped to CHEVRON #306168, Atlanta, AK.";
			line = new OrderLine(_dto);
			line.TaxGroup.Should().Be("NOMAD");
		}
	}
}