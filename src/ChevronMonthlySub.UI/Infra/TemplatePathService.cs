namespace ChevronMonthlySub.UI.Infra
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using Domain;

	public class TemplatePathService : ITemplatePathService
	{
		private readonly string _templateDir = Properties.Settings.Default.TemplateDir;
		private readonly string _freight = Properties.Settings.Default.FreightTemplateName;
		private readonly string _freightSummary = Properties.Settings.Default.SummaryFreightTemplateName;
		private readonly string _product = Properties.Settings.Default.ProductTemplateName;
		private readonly string _productSummary = Properties.Settings.Default.SummaryProductTemplateName;
		private readonly Type _freightType = typeof (FreightPurchaseOrder);
		private readonly Type _productType = typeof (ProductPurchaseOrder);

		public TemplatePathService()
		{
			OutputDirectory = Path.Combine(_templateDir, "TestReportOutput");
		}

		public string GetTemplatePath(bool isSummary, Type purchaseOrderType)
		{
			var templatelist = new List<ReportTemplate>
				{
				new ReportTemplate {IsSummary = false, Type = _freightType, Path = Path.Combine(_templateDir, _freight)},
				new ReportTemplate {IsSummary = true, Type = _freightType, Path = Path.Combine(_templateDir, _freightSummary)},
				new ReportTemplate {IsSummary = false, Type = _productType, Path = Path.Combine(_templateDir, _product)},
				new ReportTemplate {IsSummary = true, Type = _productType, Path = Path.Combine(_templateDir, _productSummary)},
				};

			return templatelist
				.Single(t => t.IsSummary == isSummary && t.Type == purchaseOrderType)
				.Path;
		}

		public string OutputDirectory { get; set; }
	}
}