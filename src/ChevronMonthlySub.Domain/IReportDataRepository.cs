namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public interface IReportDataRepository
	{
		IEnumerable<FreightLine> FreightLines { get; }
		IEnumerable<ProductLine> ProductLines { get; }
		IEnumerable<ReportData<ProductLine>> GetProductReports(string InvoiceId);
		IEnumerable<ReportData<FreightLine>> GetFreightReports(string InvoiceId);
	}
}