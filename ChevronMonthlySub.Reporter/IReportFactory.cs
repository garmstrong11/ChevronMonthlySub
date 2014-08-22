namespace ChevronMonthlySub.Reporter
{
	using Domain;
	using FlexCel.Report;

	public interface IReportFactory
	{
		FreightChevronReport CreateFreightReport(
			FlexCelReport report, FreightPurchaseOrder purchaseOrder, bool isSummary, string outputDir);
	}
}