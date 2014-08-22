namespace ChevronMonthlySub.Reporter
{
	using Domain;
	using FlexCel.Report;

	public class FreightChevronReport : ChevronReportBase
	{
		private readonly ITemplatePathService _templatePathService;

		public FreightChevronReport(ITemplatePathService templatePathService)
		{
			_templatePathService = templatePathService;
		}

		public override void BuildReport(FlexCelReport report, PurchaseOrder po)
		{
			base.BuildReport(report, po);
			var templateFilePath = _templatePathService.GetTemplatePath(IsSummary, GetType());
		}
	}
}