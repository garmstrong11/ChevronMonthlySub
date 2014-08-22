namespace ChevronMonthlySub.Reporter
{
	using Domain;
	using FlexCel.Report;

	public class ReportFactory : IReportFactory
	{
		private readonly ITemplatePathService _templatePathService;

		public ReportFactory(ITemplatePathService templatePathService)
		{
			_templatePathService = templatePathService;
		}

		public FreightChevronReport CreateFreightReport(
			FlexCelReport report, FreightPurchaseOrder purchaseOrder, bool isSummary, string outputDir)
		{
			var result = new FreightChevronReport(_templatePathService)
					{
					PurchaseOrder = purchaseOrder,
					Report = report,
					IsSummary = isSummary,
					};

				return result;
		}
	}
}