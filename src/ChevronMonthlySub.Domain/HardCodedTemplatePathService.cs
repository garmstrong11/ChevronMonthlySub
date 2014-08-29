namespace ChevronMonthlySub.Domain
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class HardCodedTemplatePathService : ITemplatePathService
	{
		private const string TemplateDir = @"F:\Chevron\Monthly Subsequent Orders";
		private const string Freight = "FreightTemplate.xlsx";
		private const string FreightSummary = "FreightSummaryTemplate.xlsx";
		private const string Product = "ProductTemplate.xlsx";
		private const string ProductSummary = "ProductSummaryTemplate.xlsx";
		private readonly Type _freightType = typeof (FreightPurchaseOrder);
		private readonly Type _productType = typeof (ProductPurchaseOrder);

		public HardCodedTemplatePathService()
		{
			OutputDirectory = Path.Combine(TemplateDir, "TestReportOutput");
		}

		public string GetTemplatePath(bool isSummary, Type purchaseOrderType)
		{
			var templatelist = new List<ReportTemplate>
				{
				new ReportTemplate {IsSummary = false, Type = _freightType, Path = Path.Combine(TemplateDir, Freight)},
				new ReportTemplate {IsSummary = true, Type = _freightType, Path = Path.Combine(TemplateDir, FreightSummary)},
				new ReportTemplate {IsSummary = false, Type = _productType, Path = Path.Combine(TemplateDir, Product)},
				new ReportTemplate {IsSummary = true, Type = _productType, Path = Path.Combine(TemplateDir, ProductSummary)},
				};

			return templatelist
				.Single(t => t.IsSummary == isSummary && t.Type == purchaseOrderType)
				.Path;
		}

		public string OutputDirectory { get; set; }
  }
}